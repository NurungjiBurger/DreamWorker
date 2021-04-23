using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_move : MonoBehaviour
{
    private M_info info;
    private M_attack atk;
    private P_info player;
    private BoxCollider2D col2D;
    private Rigidbody2D rigid2D;

    private float mx, my, mz;
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<M_info>();
        atk = GetComponent<M_attack>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_info>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();

    }

    private void FixedUpdate()
    {
        if (info.Getattacked())
        {
            info.animator.SetTrigger("attack");
            atk.Attackmotion();
        }
        else
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) <= (float)info.Getrecogrange()) // ������ ��Ÿ� �ȿ���
            {
                if (player.transform.position.x - 0.5 < this.transform.position.x) // ���Ͱ� �÷��̾�� ������
                {
                    this.transform.localScale = new Vector3(info.Getsize(), info.Getsize(), info.Getsize());
                    info.animator.SetBool("move", true);
                    this.transform.Translate(-0.01f, 0, 0);
                }
                else if (player.transform.position.x + 0.5 > this.transform.position.x) // ���Ͱ� �÷��̾�� ����
                {
                    this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                    info.animator.SetBool("move", true);
                    this.transform.Translate(0.01f, 0, 0);
                }
                if (Mathf.Abs(player.transform.position.x - this.transform.position.x) <= 1.5) // ���Ϳ� �÷��̾��� ��ġ�� �������� ( ��ġ�� ��´ٸ� )
                {
                    if (!info.Getattacked() && rigid2D.velocity.y == 0) // ������ �ٴڿ� �پ��������� ����
                    {
                        info.Setatkdone(false);
                        col2D.isTrigger = true;
                        rigid2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                        info.animator.SetTrigger("attack");
                        info.Setattacked(true);
                        info.Settime("atktime", 0);
                        info.Setisground(true);
                        if (info.Getatktype() == 1)
                        {
                            if (player.transform.position.x < mx)
                            {
                                this.transform.localScale = new Vector3(info.Getsize(), info.Getsize(), info.Getsize());
                                info.Setatkposition("atkx", -1.5f);
                                info.Setdir(-0.01f);
                            }
                            else
                            {
                                this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                                info.Setatkposition("atkx", 1.5f);
                                info.Setdir(0.01f);
                            }
                        }
                        else info.Setatkposition("atky", 1.5f);

                    }
                }
            }
            else
            {
                if (info.Gettime("mvtime") >= info.GetatkSpd())
                {
                    info.Settime("mvtime", 0);
                    info.Setrandom("mvrandom", 0, 3);    // �÷��̾ ��Ÿ� �ۿ� ������ ���Ҽ� �ִ� �ൿ 3����
                }
                if (info.Getrandom("mvrandom") == 0) // ��� �̵�
                {
                    this.transform.localScale = new Vector3(info.Getsize(), info.Getsize(), info.Getsize());
                    info.animator.SetBool("move", true);
                    this.transform.Translate(-0.01f, 0, 0);
                }
                else if (info.Getrandom("mvrandom") == 1) // �·� �̵� 
                {
                    this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                    info.animator.SetBool("move", true);
                    this.transform.Translate(0.01f, 0, 0);
                }
                else info.animator.SetBool("move", false); // stand �ִϸ��̼� ���

            }
        }
        // �ӵ� ����
        if (rigid2D.velocity.x >= 1.5f) rigid2D.velocity = new Vector2(1.5f, rigid2D.velocity.y);
        else if (rigid2D.velocity.x <= -1.5f) rigid2D.velocity = new Vector2(-1.5f, rigid2D.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        mx = this.transform.position.x;
        my = this.transform.position.y;
        mz = this.transform.position.z;
    }
}
