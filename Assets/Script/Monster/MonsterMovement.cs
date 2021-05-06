using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    enum Direction { Left, Right, Up, Stop };

    private Direction dir = Direction.Stop;

    private bool jumping = false;

    private bool isMove = false;
    private int moveRandom;

    private GameObject player;
    private Animator animator;

    [SerializeField]
    private float recognitionRange;
    [SerializeField]
    private GameObject prefabTimer;

    private Timer moveTimer;

    public void DestroyAll()
    {
        moveTimer.DestroyAll();
    }

    private void Moving()
    {
        if (!GetComponent<MonsterAttack>().Attack) // 몬스터가 공격중이 아니라면 무빙 가능
        {
            switch (dir)
            {
                case Direction.Right:
                    animator.SetBool("move", true);
                    transform.Translate(0.005f * GetComponent<MonsterStatus>().MoveSpeed, 0, 0);
                    break;
                case Direction.Left:
                    animator.SetBool("move", true);
                    transform.Translate(-0.005f * GetComponent<MonsterStatus>().MoveSpeed, 0, 0);
                    break;
                case Direction.Up:
                    break;
                case Direction.Stop:
                    animator.SetBool("move", false);
                    break;
                default:
                    //GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0f);
                    break;
            }

            if (jumping)
            {
                jumping = false;
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * GetComponent<MonsterStatus>().JumpPower);
                //transform.Translate(0, 0.005f * GetComponent<MonsterStatus>().JumpPower, 0);
            }
        }

        // 속도 제한
        if (GetComponent<Rigidbody2D>().velocity.x >= 1.5f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(1.5f, GetComponent<Rigidbody2D>().velocity.y);
        }
        else if (GetComponent<Rigidbody2D>().velocity.x <= -1.5f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-1.5f, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    private void DecideMove()
    {
        if (moveTimer.CooldownCheck()) isMove = false;

        // 인식 범위 안
        if (Vector3.Distance(player.transform.position, transform.position) <= recognitionRange)
        {
            if (GetComponent<MonsterSensor>().LastColliderGround != player.GetComponent<PlayerSensor>().LastColliderGround && transform.position.y < player.transform.position.y)
            {
                moveRandom = 2;
            }
            else
            {
                if (player.transform.position.x < transform.position.x) moveRandom = 1;
                else if (player.transform.position.x > transform.position.x) moveRandom = 0;
                else moveRandom = 3;
            }
        }
        // 인식 범위 밖일때
        else
        {
            // 움직이고 있지 않으면 1초 동안 어떻게 움직일지 정해줌
            if (!isMove)
            {
                moveRandom = Random.Range(0, 4);
                moveTimer.TimerSetZero();
                isMove = true;
            }
        }

        // 방향 바꾸기
        if(dir == Direction.Right) GetComponent<SpriteRenderer>().flipX = true;
        else if (dir == Direction.Left) GetComponent<SpriteRenderer>().flipX = false;

        // 다 정했으니 움직이자!
        switch (moveRandom)
        {
            case 0:
                dir = Direction.Right;
                break;
            case 1:
                dir = Direction.Left;
                break;
            case 2:
                if (GetComponent<MonsterSensor>().Ground)
                {
                    dir = Direction.Up;
                    jumping = true;
                }
                break;
            case 3:
            default:
                dir = Direction.Stop;
                break;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        moveTimer = Instantiate(prefabTimer).GetComponent<Timer>();
        moveTimer.SetCooldown(1.0f);
    }
    private void FixedUpdate()
    {
        Moving();
    }

    // Update is called once per frame
    private void Update()
    {
        DecideMove();
    }
}
