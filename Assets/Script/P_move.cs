using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_move : MonoBehaviour
{
    private Rigidbody2D rigid2D;
    private BoxCollider2D col2D;
    private Transform obj;

    private P_info info;
    
    // Start is called before the first frame update
    void Start()
    {

        info = GetComponent<P_info>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();
        obj = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (!info.Getiswall() || info.Getisground())
        {
            if (info.Getinputright())
            {
                info.Setinputright(false);
                rigid2D.AddForce(Vector2.right * info.Getmvspd());
            }
            if (info.Getinputleft())
            {
                info.Setinputleft(false);
                rigid2D.AddForce(Vector2.left * info.Getmvspd());
            }
        }
        if (info.Getinputjump())
        {
            info.Setinputjump(false);
            rigid2D.AddForce(Vector2.up * info.Getjumppower());
        }
        if (info.Getinputdash())
        {
            info.Setinputdash(false);
            obj.transform.position = new Vector3(info.Getp_position("plax") + (info.Getdir() * -2), info.Getp_position("play"), info.Getp_position("plaz"));
            info.Settime("dashtime", 0);
            info.Setusedash(true);
        }
        if (info.Getinputup())
        {
            if (info.Getportal() && info.Getgonext())
            {
                info.Setportal(false);
                info.Setgonext(false);
            }
            info.Setinputup(false);
        }
        // 속도 제한
        if (rigid2D.velocity.x >= 2.5f) rigid2D.velocity = new Vector2(2.5f, rigid2D.velocity.y);
        else if (rigid2D.velocity.x <= -2.5f) rigid2D.velocity = new Vector2(-2.5f, rigid2D.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        // 땅 밟기      
        RaycastHit2D raycastHit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.down, 0.02f, LayerMask.GetMask("Ground"));
        if (raycastHit.collider != null) info.animator.SetBool("move", false);
        else info.animator.SetBool("move", true);
        
        // 캐릭터 이동 ( 좌우 )
        if (Input.GetKey(KeyCode.RightArrow))
        {
            info.Setdir(-1);
            info.Setinputright(true);
            obj.transform.localScale = new Vector3(info.Getsize() * info.Getdir(), info.Getsize(), info.Getsize());
            info.animator.SetBool("move", true);
            col2D.isTrigger = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            info.Setdir(1);
            info.Setinputleft(true);
            obj.transform.localScale = new Vector3(info.Getsize() * info.Getdir(), info.Getsize(), info.Getsize());
            info.animator.SetBool("move", true);
            col2D.isTrigger = true;
        }
        else info.animator.SetBool("move", false);

        // 캐릭터 이동 ( 점프 )
        if (Input.GetKey(KeyCode.D) && info.Getisground())//rigid2D.velocity.y == 0) // 점프입력이 들어왔을때
        {
            // if (jumpcnt != 0) // 점프 횟수가 남아 있다면
            // jumpcnt--;
            info.Setinputjump(true);
            info.Setisground(false);
            obj.transform.localScale = new Vector3(info.Getsize() * info.Getdir(), info.Getsize(), info.Getsize());
            info.animator.SetBool("move", true);
            col2D.isTrigger = true;


        }

        // 캐릭터 회피 ( 대쉬 )
        if (Input.GetKey(KeyCode.Space) && !info.Getusedash())
        {
            info.Setinputdash(true);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            info.Setinputup(true);
        }

        // 대쉬 쿨다운 체크
        if (info.Getusedash())
        {
            info.Setusedash(info.CalCoolDown(info.Gettime("dashtime"), 5.0f));
        }

        if (rigid2D.velocity.y < 0)
        {
            col2D.isTrigger = false;
        }
    }
}
