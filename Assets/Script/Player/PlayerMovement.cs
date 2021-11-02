using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum Direction { Left, Right, Up, Stop };

    [SerializeField]
    private KeyCode jump = KeyCode.D;

    private float moveSpeed;
    private float jumpPower;

    [SerializeField]
    private GameObject prefabTimer;
    private Timer dashTimer;


    private float test = 5.5f;

    private GameController gameController;

    private Direction dir = Direction.Stop;

    private float time;

    private bool dashing = false;
    private bool jumping = false;
    private bool trigger = false;
    private bool isGround = true;
    private bool downjump = false;
    private float lastYVelocity = 1;

    private bool isGoNext = false;

    private Animator animator;

    public bool IsGround { get { return isGround; } set { isGround = value; } }
    public bool Trigger { get { return trigger; } set { trigger = value; } }
    public bool Jumping { get { return jumping; } }

    public bool GetGoNext()
    {
        return isGoNext;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        dashTimer = Instantiate(prefabTimer).GetComponent<Timer>();

        dashTimer.SetCooldown(5.0f);

    }

    private void KeyInput()
    {
        jumpPower = GetComponent<PlayerStatus>().Data.jumpPower;
        moveSpeed = GetComponent<PlayerStatus>().Data.moveSpeed;

        animator.SetBool("move", false);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            dir = Direction.Right;
            animator.SetBool("move", true);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Direction.Left;
            animator.SetBool("move", true);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && GetComponent<PlayerSensor>().Portal)
        {
            transform.position = GetComponent<PlayerSensor>().TeleportPosition;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            jumping = true;
            downjump = true;
        }
        if (Input.GetKeyDown(jump) && isGround)
        {
            jumping = true;
        }
        if (Input.GetKey(KeyCode.Space) && dashTimer.CooldownCheck())
        {
            dashing = true;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            dir = Direction.Stop;
        }
    }

    private void Moving()
    {
        if (GetComponent<Rigidbody2D>().velocity.y > 0 && lastYVelocity > 0) lastYVelocity = GetComponent<Rigidbody2D>().velocity.y;

        switch (dir)
        {
            case Direction.Right:
                GetComponent<ObjectFlip>().flip('x', true);
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveSpeed, ForceMode2D.Impulse);
                break;
            case Direction.Left:
                GetComponent<ObjectFlip>().flip('x', false);
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * moveSpeed, ForceMode2D.Impulse);
                break;
            case Direction.Up:
                break;
            case Direction.Stop:
            default:
                break;
        }

        if (jumping)
        {
            jumping = false;
            trigger = true;

            if (downjump) downjump = false;
            else GetComponent<Rigidbody2D>().AddForce(Vector2.up * (jumpPower), ForceMode2D.Impulse);

            lastYVelocity = GetComponent<Rigidbody2D>().velocity.y;
        }
        if (dashing)
        {
            dashTimer.TimerSetZero();
            dashing = false;
            if (GetComponent<SpriteRenderer>().flipX) GetComponent<Transform>().Translate(1.5f, 0, 0);
            else GetComponent<Transform>().Translate(-1.5f, 0, 0);
        }

        // 속도 제한
        //
        if (lastYVelocity < 2f && lastYVelocity > 0) //GetComponent<Rigidbody2D>().velocity.y < 0 && lastYVelocity > 1.5f)
        {
            //Debug.Log("실행");
            lastYVelocity = -1;
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * (jumpPower / 1.1f), ForceMode2D.Impulse);
        }
        
        if (GetComponent<Rigidbody2D>().velocity.x >= 2.5f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(2.5f, GetComponent<Rigidbody2D>().velocity.y);
        }
        else if (GetComponent<Rigidbody2D>().velocity.x <= -2.5f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-2.5f, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
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
        if (!gameController.IsPause)
        {
            KeyInput();
        }
    }
}