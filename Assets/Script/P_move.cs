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
        // �ӵ� ����
        if (rigid2D.velocity.x >= 2.5f) rigid2D.velocity = new Vector2(2.5f, rigid2D.velocity.y);
        else if (rigid2D.velocity.x <= -2.5f) rigid2D.velocity = new Vector2(-2.5f, rigid2D.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        // �� ���      
        RaycastHit2D raycastHit = Physics2D.BoxCast(col2D.bounds.center, col2D.bounds.size, 0f, Vector2.down, 0.02f, LayerMask.GetMask("Ground"));
        if (raycastHit.collider != null) info.animator.SetBool("move", false);
        else info.animator.SetBool("move", true);
        
        // ĳ���� �̵� ( �¿� )
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

        // ĳ���� �̵� ( ���� )
        if (Input.GetKey(KeyCode.D) && info.Getisground())//rigid2D.velocity.y == 0) // �����Է��� ��������
        {
            // if (jumpcnt != 0) // ���� Ƚ���� ���� �ִٸ�
            // jumpcnt--;
            info.Setinputjump(true);
            info.Setisground(false);
            obj.transform.localScale = new Vector3(info.Getsize() * info.Getdir(), info.Getsize(), info.Getsize());
            info.animator.SetBool("move", true);
            col2D.isTrigger = true;


        }

        // ĳ���� ȸ�� ( �뽬 )
        if (Input.GetKey(KeyCode.Space) && !info.Getusedash())
        {
            info.Setinputdash(true);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            info.Setinputup(true);
        }

        // �뽬 ��ٿ� üũ
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
