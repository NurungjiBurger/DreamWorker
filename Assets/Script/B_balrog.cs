using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_balrog : MonoBehaviour
{
    private M_info info;
    private P_info player;
    private BoxCollider2D col2D;
    private Rigidbody2D rigid2D;

    [SerializeField]
    private GameObject[] prf_attack_effect;
    [SerializeField]
    private GameObject[] prf_attack_judgement;

    private float mx, my, mz;


    void attack_judgement_summon()
    {
        // ���� ���������� ���� ����Ʈ �ִϸ��̼� ���
    }

    void attack_effect()
    {
        // ���� ����Ʈ�� ����Ǹ鼭 ���� ���� ��ȯ�� ����
        attack_judgement_summon();
    }

    void attackmotion()
    {
        // ���� ����� ���۵Ǹ鼭 ���� ����Ʈ ����
        if (info.Gettime("atktime") >= info.GetatkSpd()) info.Setattacked(false);

        attack_effect();
    }

    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<M_info>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_info>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (info.Getattacked())
        {
            attackmotion();
        }
        else
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) <= (float)info.Getattackrange()) // �÷��̾���� �Ÿ��� ���� ��Ÿ����� ª�� ���
            {
                // ���ݻ�Ÿ� ���� �ִ� ��� �̹Ƿ� ���� ����
                if (!info.Getattacked() && rigid2D.velocity.y == 0) // ������ �ٴڿ� �پ��������� ����
                {
                    info.animator.SetBool("move", false);
                    info.Setatkdone(false);
                    info.Setrandom("atkrandom", 0, 2);
                    if (info.Getrandom("atkrandom") == 1) info.animator.SetTrigger("attack1");
                    else info.animator.SetTrigger("attack");
                    info.Setattacked(true);
                    info.Settime("atktime", 0);
                    info.Setisground(true);
                    
                }
            }
            else if (Vector3.Distance(player.transform.position, this.transform.position) <= (float)info.Getrecogrange()) // �÷��̾���� �Ÿ��� ���� ��Ÿ����� ��� �ν� ��Ÿ����� ª�� ���
            {
                // ���ݻ�Ÿ��� ���Ե��� �����Ƿ� ������ �� ���� ����
                // �νĻ�Ÿ��� ���ԵǹǷ� ���Ͱ� ������ ���� ���� �ٰ���
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
            }
            else    // �÷��̾���� �Ÿ��� �νĻ�Ÿ��� ��� ���
            {
                // �Ϻ��� ��Ÿ� ���̹Ƿ� ���ʹ� �������� �ൿ
                // ��Ÿ� �ۿ� �÷��̾ ���� ��� ���Ͱ� �� �� �ִ� �ൿ 3������ �з�

                if (info.Gettime("mvtime") >= info.GetatkSpd())
                {
                    info.Settime("mvtime", 0);
                    info.Setrandom("mvrandom", 0, 3);
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
    }

    void Update()
    {
        mx = this.transform.position.x;
        my = this.transform.position.y;
        mz = this.transform.position.z;
    }

    /*
    ���� 2
    x : -1
    y : -0.25
    



    

    */
}
