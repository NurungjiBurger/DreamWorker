using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class monster : MonoBehaviour
{
    // ������ ���� & ����
    static int size = 2;
    string enemyName;
    int maxHP;
    int nowHP;
    int atkDmg;
    int atkSpeed;
    int recognition_range;
    float jumpPower;
    float dashPower;
    public bool attacked = false;
    bool atkdone = false;
    bool isground = false;
    float dir;
    int attacktype;

    public float height = 0.7f;
    int mvrandom;
    float atktime;
    float mvtime;

    float px, py, pz;
    float mx, my, mz;
    float atkx, atky, atkz;

    // ������Ʈ
    public GameObject prfHpBar;
    public GameObject canvas;
    public GameObject objmonster;
    Animator animator;
    RectTransform hpBar;
    public P_info player;
    Image nowHPbar;

    BoxCollider2D col2D;
    Rigidbody2D rigid2D;

    void AttackTrue()
    {
        attacked = true;
    }
    void AttackFalse()
    {
        attacked = false;
    }

    void Attackmotion()
    {

        if (attacktype == 0) // ��������
        {
            if (!atkdone)
            {
                if (my < atky) objmonster.transform.Translate(0, jumpPower, 0);
                else atkdone = true;
            }
            else
            {
                col2D.isTrigger = false;
                rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (atktime >= atkSpeed)
                {
                    attacked = false;
                }
            }
        }
        else if (attacktype == 1)
        {
            if (dir < 0)
            {
                if (mx > atkx)
                {
                    animator.SetBool("move", true);
                    objmonster.transform.Translate(-dashPower, 0, 0);
                }
                else
                {
                    col2D.isTrigger = false;
                    rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                    if (atktime >= atkSpeed)
                    {
                        animator.SetBool("move", false);
                        attacked = false;
                    }
                }
            }
            else if (dir > 0)
            {
                if (mx < atkx)
                {
                    animator.SetBool("move", true);
                    objmonster.transform.Translate(dashPower, 0, 0);
                }
                else
                {
                    col2D.isTrigger = false;
                    rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                    if (atktime >= atkSpeed)
                    {
                        animator.SetBool("move", false);
                        attacked = false;
                    }
                }
            }
        }
    }

    private void SetEnemyStatus(string _enemyName, int _maxHP, int _atkDmg, int _atkSpd, int _recognition_range, int _attacktype, float _jump_dashPower)
    {
        enemyName = _enemyName;
        maxHP = _maxHP;
        nowHP = _maxHP;
        atkDmg = _atkDmg;
        atkSpeed = _atkSpd;
        recognition_range = _recognition_range;
        attacktype = _attacktype;
        if (attacktype == 0) jumpPower = _jump_dashPower;
        else if (attacktype == 1) dashPower = _jump_dashPower;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Attack_Judgment"))
        {
            if (player.attacked)
            {
                animator.SetTrigger("hit");
                nowHP -= player.atkDmg;
                player.attacked = false;
                if (nowHP <= 0)
                {
                    Destroy(canvas);
                    Destroy(gameObject);
                    Destroy(hpBar.gameObject);
                }
            }
        }
        
        /*
        if (col.CompareTag("Player"))
        {
            print("ĳ���ʹ�");
            col2D.isTrigger = true;
        }
        */

        if (col.CompareTag("Ground"))
        {
            col2D.isTrigger = false;
            isground = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = Instantiate(canvas);
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_info>();
        animator = GetComponent<Animator>();

        if (name.Equals("mushroom(Clone)"))
        {
            SetEnemyStatus("��Ȳ����", 100, 10, 3, 3, 0, 0.035f);
        }
        else if (name.Equals("blue_snail(Clone)"))
        {
            SetEnemyStatus("�Ķ�������", 50, 5, 5, 2, 1, 0.015f);
        }
        else if (name.Equals("pig(Clone)"))
        {
            SetEnemyStatus("����", 100, 15, 5, 4, 1, 0.015f);
        }
        else if (name.Equals("slime(Clone)"))
        {
            SetEnemyStatus("������", 100, 10, 3, 3, 0, 0.015f);
        }
        nowHPbar = hpBar.transform.GetChild(0).GetComponent<Image>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();

        mvrandom = 2;

        atkx = atky = atkz = 0;

        mvtime = 0;
        atktime = 0;
        attacked = false;
    }
    void FixedUpdate()
    {
        px = player.transform.position.x;
        py = player.transform.position.y;
        pz = player.transform.position.z;

        mx = objmonster.transform.position.x;
        my = objmonster.transform.position.y;
        mz = objmonster.transform.position.z;

        if (attacked)
        {
            animator.SetTrigger("attack");
            Attackmotion();
        }
        else
        {
            if (Vector3.Distance(player.transform.position, objmonster.transform.position) <= (float)recognition_range) // ������ ��Ÿ� �ȿ���
            {
                if (px - 0.5 < mx) // ���Ͱ� �÷��̾�� ������
                {
                    objmonster.transform.localScale = new Vector3(size, size, size);
                    animator.SetBool("move", true);
                    objmonster.transform.Translate(-0.01f, 0, 0);
                }
                else if (px + 0.5 > mx) // ���Ͱ� �÷��̾�� ����
                {
                    objmonster.transform.localScale = new Vector3(-size, size, size);
                    animator.SetBool("move", true);
                    objmonster.transform.Translate(0.01f, 0, 0);
                }
                if (Mathf.Abs(px - mx) <= 1.5) // ���Ϳ� �÷��̾��� ��ġ�� �������� ( ��ġ�� ��´ٸ� )
                {
                    if (!attacked && rigid2D.velocity.y == 0) // ������ �ٴڿ� �پ��������� ����
                    {
                        atkdone = false;
                        col2D.isTrigger = true;
                        rigid2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                        animator.SetTrigger("attack");
                        attacked = true;
                        atktime = 0;
                        isground = true;
                        if (attacktype == 1)
                        {
                            if (px < mx)
                            {
                                objmonster.transform.localScale = new Vector3(size, size, size);
                                atkx = px - 1.5f;
                                dir = -0.01f;
                            }
                            else
                            {
                                objmonster.transform.localScale = new Vector3(-size, size, size);
                                atkx = px + 1.5f;
                                dir = 0.01f;
                            }
                        }
                        else atky = py + 1.5f;

                    }
                }
            }
            else
            {
                if (mvtime >= atkSpeed)
                {
                    mvtime = 0;
                    mvrandom = Random.Range(0, 3);    // �÷��̾ ��Ÿ� �ۿ� ������ ���Ҽ� �ִ� �ൿ 3����
                }
                if (mvrandom == 0) // ��� �̵�
                {
                    objmonster.transform.localScale = new Vector3(size, size, size);
                    animator.SetBool("move", true);
                    objmonster.transform.Translate(-0.01f, 0, 0);
                }
                else if (mvrandom == 1) // �·� �̵� 
                {
                    objmonster.transform.localScale = new Vector3(-size, size, size);
                    animator.SetBool("move", true);
                    objmonster.transform.Translate(0.01f, 0, 0);
                }
                else animator.SetBool("move", false); // stand �ִϸ��̼� ���

            }
        }
        // �ӵ� ����
        if (rigid2D.velocity.x >= 1.5f) rigid2D.velocity = new Vector2(1.5f, rigid2D.velocity.y);
        else if (rigid2D.velocity.x <= -1.5f) rigid2D.velocity = new Vector2(-1.5f, rigid2D.velocity.y);

    }
    // Update is called once per frame
    void Update()
    {

        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        hpBar.transform.position = _hpBarPos;
        nowHPbar.fillAmount = (float)nowHP / (float)maxHP;

        atktime += Time.deltaTime;
        mvtime += Time.deltaTime;


    }
}
