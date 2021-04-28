using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    enum Direction { Left, Right, Up, Stop };

    private Direction dir = Direction.Stop;

    private bool isGround = true;
    private bool jumping = false;

    private bool isMove = false;
    private int moveRandom;

    private GameObject player;
    private Animator animator;

    [SerializeField]
    private float recognitionRange = 4.0f;
    [SerializeField]
    private GameObject prefabTimer;
    [SerializeField]
    private float moveSpeed = 0.1f;
    [SerializeField]
    private float jumpPower = 0.4f;

    private Timer moveTimer;

    public float Range
    {
        get
        {
            return recognitionRange;
        }
        set
        {
            Debug.Log("값을 설정할 수 없습니다.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        moveTimer = Instantiate(prefabTimer).GetComponent<Timer>();
        moveTimer.SetCooldown(1.0f);
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
                break;
            case Direction.Stop:
            default:
                //GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0f);
                break;
        }

        if (jumping)
        {
            jumping = false;
            isGround = false;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPower);
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

    // Update is called once per frame
    void Update()
    {
        if(isMove) isMove = !moveTimer.CooldownCheck();
        //Vector3.Distance(player.transform.position, this.transform.position) <= (float)info.Getattackrange()

        // 인식 범위 안이고 공격 범위 밖일때
        if (Vector3.Distance(player.transform.position, transform.position) <= recognitionRange)
        {
            if (player.transform.position.x < transform.position.x) moveRandom = 1;
            else if (player.transform.position.x > transform.position.x) moveRandom = 0;
            else moveRandom = 3;

            if (player.transform.position.y > transform.position.y) moveRandom = 2;
        }
        // 인식 범위 밖일때
        else
        {
            // 움직이고 있지 않으면 1초 동안 어떻게 움직일지 정해줌
            if (!isMove)    
            {
                moveRandom = Random.Range(0, 4);
                isMove = true;
            }
        }

        // 다 정했으니 움직이자!
        switch (moveRandom)
        {
            case 0:
                dir = Direction.Right;
                GetComponent<SpriteRenderer>().flipX = true;
                animator.SetBool("move", true);
                break;
            case 1:
                dir = Direction.Left;
                GetComponent<SpriteRenderer>().flipX = false;
                animator.SetBool("move", true);
                break;
            case 2:
                if (isGround)
                {
                    dir = Direction.Up;
                    jumping = true;
                }
                break;
            case 3:
            default:
                dir = Direction.Stop;
                animator.SetBool("move", false);
                break;
        }
    }
}
