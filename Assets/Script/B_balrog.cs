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
        if (info.Getrandom("atkrandom") == 0)
        {
            // x dir ��ŭ 1 �����ϰ� dir ��ŭ 0~1
            // y 0.029 ���ϰ� 0~1
            // ���׿�
            //Instantiate(prf_attack_judgement[0], new Vector3(-(mx + (-2 * -info.Getdir()) + (-Random.Range(1.0f, 2.0f) * -info.Getdir())), my + 0.03f + Random.Range(0.0f, 1.0f), mz), Quaternion.identity);
        }
        else
        {
            // ������
            // �����̸� -1 , -0.4
            // �������̸� 1 , -0.4
            Instantiate(prf_attack_judgement[1], new Vector3(mx - ( 1.5f * info.Getdir() ) , my - 0.8f, mz), Quaternion.identity);
        }

        info.Setjudgement(true);
    }

    void attack_effect()
    {
        // print("now" + mx + " , " + my + " , " + mz);
        // ���� ����Ʈ�� ����Ǹ鼭 ���� ���� ��ȯ�� ����
        if (!info.Getjudgement())
        {
            if (info.Getrandom("atkrandom") == 0)
            {
                info.animator.SetTrigger("attack");
                //Instantiate(prf_attack_effect[0], new Vector3(-(mx + (-2 * -info.Getdir())), my - 0.25f, mz), Quaternion.identity);
            }
            else
            {
                //���� 2
                // x: -1
                // y: -0.25
                info.animator.SetTrigger("attack1");
                Instantiate(prf_attack_effect[1], new Vector3(mx , my , mz), Quaternion.identity);              
            }

            info.Seteffect(true);

            attack_judgement_summon();
        }

    }

    void attackmotion()
    {
        // ���� ����� ���۵Ǹ鼭 ���� ����Ʈ ����
        if (!info.Geteffect()) attack_effect();

        if (info.Gettime("atktime") >= info.GetatkSpd()) info.Setattacked(false);
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
                    if (player.transform.position.x > mx)
                    {
                        info.Setdir(-1);
                        this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                    }
                    else
                    {
                        info.Setdir(1);
                        this.transform.localScale = new Vector3(info.Getsize(), info.Getsize(), info.Getsize());
                    }

                    info.animator.SetBool("move", false);
                    info.Setatkdone(false);
                    info.Setrandom("atkrandom", 1, 2);
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
                    info.Setdir(-1);
                    this.transform.Translate(info.Getdir() * 0.01f, 0, 0);
                }
                else if (player.transform.position.x + 0.5 > this.transform.position.x) // ���Ͱ� �÷��̾�� ����
                {
                    this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                    info.animator.SetBool("move", true);
                    info.Setdir(1);
                    this.transform.Translate(info.Getdir() * 0.01f, 0, 0);
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
                    info.Setdir(-1);
                    this.transform.Translate(info.Getdir() * 0.01f, 0, 0);
                }
                else if (info.Getrandom("mvrandom") == 1) // �·� �̵� 
                {
                    this.transform.localScale = new Vector3(-info.Getsize(), info.Getsize(), info.Getsize());
                    info.animator.SetBool("move", true);
                    info.Setdir(1);
                    this.transform.Translate(info.Getdir() * 0.01f, 0, 0);
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
}
