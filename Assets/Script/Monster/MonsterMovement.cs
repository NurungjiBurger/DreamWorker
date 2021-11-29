using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField]
    private float recognitionRange;
    [SerializeField]
    private float[] offsetX;
    [SerializeField]
    private float[] offsetY;
    [SerializeField]
    private GameObject prefabTimer;

    private bool jumping = false;
    private bool isGround = true;
    private bool trigger = false;
    private bool isMove = false;

    private int moveRandom;
    private float lastYVelocity = 0;

    private GameObject player;

    private Animator animator;
    private Timer moveTimer;
    private GameController gameController;
    enum Direction { Left, Right, Up, Stop };
    private Direction dir = Direction.Stop;

    private void OnEnable()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public bool Trigger { get { return trigger; } set { trigger = value; } }
    public bool IsGround { get { return isGround; } set { isGround = value; } }

    public void DestroyAll()
    {
        moveTimer.DestroyAll();
    }

    private void Moving()
    {
        // 가속력이 0에 가까워지면서 공중에 체공하는 시간이 길어질수록 해당 오브젝트가 중력을 안받는것처럼 보임.
        // 강제로 -가속력을 주어 해당 체공시간을 없앰.
        if (GetComponent<Rigidbody2D>().velocity.y > 0 && lastYVelocity > 0) lastYVelocity = GetComponent<Rigidbody2D>().velocity.y;

        // 공격중이 아닌 경우 움직일 수 있음
        if (!GetComponent<MonsterAttack>().Attack)
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

                GetComponent<Rigidbody2D>().AddForce(Vector2.up * GetComponent<MonsterStatus>().Data.jumpPower, ForceMode2D.Impulse);

                lastYVelocity = GetComponent<Rigidbody2D>().velocity.y;
            }
        }


        // 속도 제한 Y, X
        if (lastYVelocity < 2f && lastYVelocity > 0) //GetComponent<Rigidbody2D>().velocity.y < 0 && lastYVelocity > 1.5f)
        {
            lastYVelocity = -1;
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * (GetComponent<MonsterStatus>().Data.jumpPower / 1.1f), ForceMode2D.Impulse);
        }

        if (GetComponent<Rigidbody2D>().velocity.x >= 1.5f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(1.5f, GetComponent<Rigidbody2D>().velocity.y);
        }
        else if (GetComponent<Rigidbody2D>().velocity.x <= -1.5f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-1.5f, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    // 움직임 결정 함수
    private void DecideMove()
    {
        if (moveTimer.CooldownCheck()) isMove = false;

        // 인식 범위 안일때
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


        // 다 정했졌으므로 움직임
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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        moveTimer = Instantiate(prefabTimer).GetComponent<Timer>();
        moveTimer.SetCooldown(1.0f);
    }
    private void FixedUpdate()
    {
        if (!gameController.IsPause)
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

    private void Update()
    {
        if (!gameController.IsPause)  DecideMove();
    }
}
