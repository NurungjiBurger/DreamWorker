using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_move : MonoBehaviour
{
    private static int size = 2;

    private bool detectwall = false;
    private bool ground = false;
    private bool inputRight = false;
    private bool inputLeft = false;
    private bool inputJump = false;
    private bool inputDash = false;
    private float moveSpeed;
    private float jumpPower;
    private float DashTime;
    private bool useDash = false;

    public bool portal = false;
    public bool inputUp = false;
    public bool gonext = false;


    private Rigidbody2D rigid2D;
    private BoxCollider2D col2D;
    private Transform obj;

    private P_info info;
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Portal")) // 포탈과 충돌하면
        {
            portal = true;
        }
        if (col.CompareTag("Ground")) // 땅과 닿아있다면
        {
            detectwall = false;
            ground = true;
        }
        if (col.CompareTag("Wall")) // 벽과 충돌하면
        {
            detectwall = true;
            col2D.isTrigger = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 0.1f;
        jumpPower = 0.4f;
        DashTime = 0;

        info = GetComponent<P_info>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();
        obj = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (!detectwall || ground)
        {
            if (inputRight)
            {
                inputRight = false;
                rigid2D.AddForce(Vector2.right * moveSpeed);
            }
            if (inputLeft)
            {
                inputLeft = false;
                rigid2D.AddForce(Vector2.left * moveSpeed);
            }
        }
        if (inputJump)
        {
            inputJump = false;
            rigid2D.AddForce(Vector2.up * jumpPower);
        }
        if (inputDash)
        {
            inputDash = false;
            obj.transform.position = new Vector3(info.plax + (info.dir * -2), info.play, info.plaz);
            DashTime = info.timerOn();
            useDash = true;
        }
        if (inputUp)
        {
            if (portal && gonext)
            {
                portal = false;
                gonext = false;
            }
            inputUp = false;
        }
        // 속도 제한
        if (rigid2D.velocity.x >= 2.5f) rigid2D.velocity = new Vector2(2.5f, rigid2D.velocity.y);
        else if (rigid2D.velocity.x <= -2.5f) rigid2D.velocity = new Vector2(-2.5f, rigid2D.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        DashTime += Time.deltaTime;

        // 땅 밟기      
        RaycastHit2D raycastHit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.down, 0.02f, LayerMask.GetMask("Ground"));
        if (raycastHit.collider != null) info.animator.SetBool("move", false);
        else info.animator.SetBool("move", true);
        
        // 캐릭터 이동 ( 좌우 )
        if (Input.GetKey(KeyCode.RightArrow))
        {
            info.dir = -1;
            inputRight = true;
            obj.transform.localScale = new Vector3(size * info.dir, size, size);
            info.animator.SetBool("move", true);
            col2D.isTrigger = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            info.dir = 1;
            inputLeft = true;
            obj.transform.localScale = new Vector3(size * info.dir, size, size);
            info.animator.SetBool("move", true);
            col2D.isTrigger = true;
        }
        else info.animator.SetBool("move", false);

        // 캐릭터 이동 ( 점프 )
        if (Input.GetKey(KeyCode.D) && ground)//rigid2D.velocity.y == 0) // 점프입력이 들어왔을때
        {
            // if (jumpcnt != 0) // 점프 횟수가 남아 있다면
            // jumpcnt--;
            inputJump = true;
            ground = false;
            obj.transform.localScale = new Vector3(size * info.dir, size, size);
            info.animator.SetBool("move", true);
            col2D.isTrigger = true;


        }

        // 캐릭터 회피 ( 대쉬 )
        if (Input.GetKey(KeyCode.Space) && !useDash)
        {
            inputDash = true;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            inputUp = true;
        }

        // 대쉬 쿨다운 체크
        if (useDash)
        {
            useDash = info.CalCoolDown(DashTime, 5.0f);
        }

        if (rigid2D.velocity.y < 0)
        {
            col2D.isTrigger = false;
        }
    }
}
