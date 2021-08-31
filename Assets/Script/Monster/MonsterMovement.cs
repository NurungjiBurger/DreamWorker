using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    enum Direction { Left, Right, Up, Stop };

    private Direction dir = Direction.Stop;

    private bool jumping = false;

    private bool isGround = true;
    private bool trigger = false;
    private bool isMove = false;
    private bool animating = false;
    private int moveRandom;

    private GameObject player;
    private Animator animator;

    [SerializeField]
    private float recognitionRange;
    [SerializeField]
    private GameObject prefabTimer;
    [SerializeField]
    private float[] offsetX;
    [SerializeField]
    private float[] offsetY;

    private Timer moveTimer;

    public bool Trigger { get { return trigger; } set { trigger = value; } }
    public bool IsGround { get { return isGround; } set { isGround = value; } }

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
                    transform.Translate(0.005f * GetComponent<MonsterStatus>().Data.moveSpeed, 0, 0);
                    break;
                case Direction.Left:
                    animator.SetBool("move", true);
                    transform.Translate(-0.005f * GetComponent<MonsterStatus>().Data.moveSpeed, 0, 0);
                    break;
                case Direction.Up:
                    break;
                case Direction.Stop:
                    animator.SetBool("move", false);
                    break;
                default:
                    break;
            }

            if (jumping)
            {
                jumping = false;
                trigger = true;

                GetComponent<Rigidbody2D>().AddForce(Vector2.up * GetComponent<MonsterStatus>().Data.jumpPower);
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

            if (!isMove)
            {
                moveRandom = Random.Range(0, 4);
                moveTimer.TimerSetZero();
                isMove = true;
            }
        }

        // 방향 바꾸기
        if (dir == Direction.Right)
        {
            GetComponent<ObjectFlip>().flip('x', true);
            GetComponent<BoxCollider2D>().offset = new Vector2(-offsetX[0], offsetY[0]);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(-offsetX[1], offsetY[1]);
        }
        else if (dir == Direction.Left)
        {
            GetComponent<ObjectFlip>().flip('x', false);
            GetComponent<BoxCollider2D>().offset = new Vector2(offsetX[0], offsetY[0]);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(offsetX[1], offsetY[1]);
        }


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
                if (isGround)
                {
                    dir = Direction.Up;
                    jumping = true;
                }
                break;
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
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
            Moving();
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)  DecideMove();
    }
}
