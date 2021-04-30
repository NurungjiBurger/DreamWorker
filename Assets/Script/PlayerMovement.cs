using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum Direction { Left, Right, Up, Stop };

    [SerializeField]
    private KeyCode jump = KeyCode.D;

    [SerializeField]
    private float moveSpeed = 0.1f;

    [SerializeField]
    private float jumpPower = 0.4f;

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
    void Start()
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
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveSpeed);
                break;
            case Direction.Left:
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

        // 속도 제한
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
    void Update()
    {
        animator.SetBool("move", false);
        // 오른쪽이동
        if (Input.GetKey(KeyCode.RightArrow))
        {
            dir = Direction.Right;
            GetComponent<SpriteRenderer>().flipX = true;
            animator.SetBool("move", true);
        }
        // 왼쪽이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Direction.Left;
            GetComponent<SpriteRenderer>().flipX = false;
            animator.SetBool("move", true);
        }
        // 포탈상호작용?
        if (Input.GetKey(KeyCode.UpArrow) && GetComponent<PlayerSensor>().Portal)
        {
            dir = Direction.Up;
            animator.SetBool("move", true);
        }
        // 점프
        if (Input.GetKeyDown(jump) && GetComponent<PlayerSensor>().Ground)
        {
            jumping = true;
        }
        // 대쉬
        if (Input.GetKey(KeyCode.Space) && dashTimer.CooldownCheck())
        {
            dashing = true;
        }
        // 멈춤
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            dir = Direction.Stop;
            isGoNext = false;
        }
        //

        //Debug.Log(GetComponent<PlayerSensor>().Ground);
    }
}