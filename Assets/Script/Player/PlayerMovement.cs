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
    private Direction dir = Direction.Stop;

    private float time;

    private bool dashing = false;
    private bool jumping = false;

    private bool isGoNext = false;

    private Animator animator;


    public bool GetGoNext()
    {
        return isGoNext;
    }

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();

        dashTimer = Instantiate(prefabTimer).GetComponent<Timer>();

        dashTimer.SetCooldown(5.0f);

    }

    private void FixedUpdate()
    {
        switch (dir)
        {
            case Direction.Right:
                GetComponent<SpriteRenderer>().flipX = true;
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveSpeed);
                break;
            case Direction.Left:
                GetComponent<SpriteRenderer>().flipX = false;
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * moveSpeed);
                break;
            case Direction.Up:
                isGoNext = true;
                break;
            case Direction.Stop:
            default:
                //GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0f);
                break;
        }

        if (jumping)
        {
            jumping = false;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPower);
        }
        if (dashing)
        {
            dashTimer.TimerSetZero();
            dashing = false;
            if (GetComponent<SpriteRenderer>().flipX) GetComponent<Transform>().Translate(1.5f,0,0);
            else GetComponent<Transform>().Translate(-1.5f, 0, 0);
        }

        // �ӵ� ����
        if (GetComponent<Rigidbody2D>().velocity.x >= 2.5f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(2.5f, GetComponent<Rigidbody2D>().velocity.y);
        }
        else if (GetComponent<Rigidbody2D>().velocity.x <= -2.5f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-2.5f, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        jumpPower = GetComponent<PlayerStatus>().JumpPower;
        moveSpeed = GetComponent<PlayerStatus>().MoveSpeed;

        animator.SetBool("move", false);
        // �������̵�
        if (Input.GetKey(KeyCode.RightArrow))
        {
            dir = Direction.Right;
            animator.SetBool("move", true);
        }
        // �����̵�
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Direction.Left;
            animator.SetBool("move", true);
        }
        // ��Ż��ȣ�ۿ�?
        if (Input.GetKey(KeyCode.UpArrow) && GetComponent<PlayerSensor>().Portal)
        {
            dir = Direction.Up;
            animator.SetBool("move", true);
        }
        // ����
        if (Input.GetKeyDown(jump) && GetComponent<PlayerSensor>().Ground)
        {
            jumping = true;
        }
        // �뽬
        if (Input.GetKey(KeyCode.Space) && dashTimer.CooldownCheck())
        {
            dashing = true;
        }
        // ����
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            dir = Direction.Stop;
            isGoNext = false;
        }
        //

        //Debug.Log(GetComponent<PlayerSensor>().Ground);
    }
}