using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum Direction { Left, Right, Up, Stop, Down };

    private float moveSpeed;
    private float jumpPower;

    [SerializeField]
    private GameObject prefabTimer;
    private Timer dashTimer;

    private GameController gameController;
    private UISensor joyStick;

    private int action;
    private Direction dir = Direction.Stop;

    private float time;

    private bool dashing = false;
    private bool jumping = false;
    private bool trigger = false;
    private bool isGround = true;
    private bool downjump = false;
    private float lastYVelocity = 0;

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
        action = joyStick.Direction;

        jumpPower = GetComponent<PlayerStatus>().Data.jumpPower;
        moveSpeed = GetComponent<PlayerStatus>().Data.moveSpeed;

        if (Input.GetKey(KeyCode.RightArrow)) action = 1;
        else if (Input.GetKey(KeyCode.LeftArrow)) action = 2;
        else if (Input.GetKey(KeyCode.UpArrow)) action = 3;
        else if (Input.GetKey(KeyCode.DownArrow)) action = 4;
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow)) action = 0;

        animator.SetBool("move", true);

        switch (action)
        {
            case 0:
                dir = Direction.Stop;
                animator.SetBool("move", false);
                break;
            case 1:
                dir = Direction.Right;
                break;
            case 2:
                dir = Direction.Left;
                break;
            case 3:
                dir = Direction.Up;
                break;
            case 4:
                dir = Direction.Down;
                break;
            default:
                break;
        }

        if (GameObject.Find("Canvas").transform.Find("EntranceButton").GetComponent<ButtonUI>().OnOff && dashTimer.CooldownCheck())
        {
            if (GetComponent<PlayerSensor>().Portal) transform.position = GetComponent<PlayerSensor>().TeleportPosition;
            GameObject.Find("Canvas").transform.Find("EntranceButton").GetComponent<ButtonUI>().UIActive();
        }

        if (GameObject.Find("Canvas").transform.Find("JumpButton").GetComponent<ButtonUI>().OnOff || (isGround && Input.GetKeyDown(KeyCode.D)))
        {
            if (isGround) jumping = true;
            animator.SetBool("move", true);
            GameObject.Find("Canvas").transform.Find("JumpButton").GetComponent<ButtonUI>().UIActive();
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
                //if (GetComponent<PlayerSensor>().Portal) transform.position = GetComponent<PlayerSensor>().TeleportPosition;
                break;
            case Direction.Stop:
                break;
            case Direction.Down:
                downjump = true;
                break;
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
            if (GetComponent<ObjectFlip>().flipX) transform.position += new Vector3(1.5f, 0f, 0f);
            else transform.position -= new Vector3(1.5f, 0f, 0f);
        }

        // 속도 제한
        if (lastYVelocity < 2f && lastYVelocity > 0) //GetComponent<Rigidbody2D>().velocity.y < 0 && lastYVelocity > 1.5f)
        {
            //
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
            if (!joyStick) joyStick = GameObject.Find("Canvas").transform.Find("JoyStick").transform.Find("Lever").GetComponent<UISensor>();
            else KeyInput();
        }
    }
}