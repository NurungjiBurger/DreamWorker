using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class monster : MonoBehaviour
{
    // 몬스터의 스탯 & 상태
    static int size = 2;
    string enemyName;
    int maxHP;
    int nowHP;
    int atkDmg;
    int atkSpeed;
    int recognition_range;
    float jumpPower;
    public bool attacked = false;

    public float height = 0.7f;
    float time;

    // 오브젝트
    public GameObject prfHpBar;
    public GameObject canvas;
    public GameObject objmonster;
    Animator animator;
    RectTransform hpBar;
    public character player;
    void AttackTrue()
    {
        attacked = true;
    }
    void AttackFalse()
    {
        attacked = false;
    }
    private void SetEnemyStatus(string _enemyName, int _maxHP, int _atkDmg, int _atkSpd, int _recognition_range, float _jumpPower)
    {
        enemyName = _enemyName;
        maxHP = _maxHP;
        nowHP = _maxHP;
        atkDmg = _atkDmg;
        atkSpeed = _atkSpd;
        recognition_range = _recognition_range;
        jumpPower = _jumpPower;
    }

    Image nowHPbar;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        // hpBar = Instantiate(prfHpBar, transform.position, Quaternion.identity, transform);
        canvas = Instantiate(canvas);
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        //hpBar = canvas.transform).GetComponent<RectTransform>();
        // hpBar = Instantiate(prfHpBar, this.transform.position, Quaternion.identity);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<character>();
        animator = GetComponent<Animator>();
        if (name.Equals("mushroom(Clone)"))
        {
            SetEnemyStatus("주황버섯", 100, 10, 1, 3, 0.015f);
        }
        else if (name.Equals("blue_snail(Clone)"))
        {
            SetEnemyStatus("파란달팽이", 50, 5, 1, 2, 0.015f);
        }
        else if (name.Equals("pig(Clone)"))
        {
            SetEnemyStatus("돼지", 100, 15, 2, 4, 0.015f);
        }
        nowHPbar = hpBar.transform.GetChild(0).GetComponent<Image>();
        time = 0;
        attacked = false;
    }
    void FixedUpdate()
    {
        

    }
    // Update is called once per frame
    void Update()
    {

        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        hpBar.transform.position = _hpBarPos;
        nowHPbar.fillAmount = (float)nowHP / (float)maxHP;

        int random = Random.Range(0, 10);

        time += Time.deltaTime;

        float px, py, pz;
        float mx, my, mz;

        px = player.transform.position.x;
        py = player.transform.position.y;
        pz = player.transform.position.z;

        mx = objmonster.transform.position.x;
        my = objmonster.transform.position.y;
        mz = objmonster.transform.position.z;

        if (time >= atkSpeed) attacked = false;

        if (attacked)
        {
            animator.SetTrigger("attack");
            objmonster.transform.Translate(0, jumpPower, 0);
        }
        else
        {
            if (Mathf.Abs(py) - Mathf.Abs(my) <= 0.5)
            {
                if (Vector3.Distance(player.transform.position, objmonster.transform.position) <= (float)recognition_range) // 몬스터의 사거리 안에서
                {
                    if (px - 0.5 < mx) // 몬스터가 플레이어보다 오른쪽
                    {
                        objmonster.transform.localScale = new Vector3(size, size, size);
                        animator.SetBool("move", true);
                        objmonster.transform.Translate(-0.001f, 0, 0);
                    }
                    else if (px + 0.5 > mx) // 몬스터가 플레이어보다 왼쪽
                    {
                        objmonster.transform.localScale = new Vector3(-size, size, size);
                        animator.SetBool("move", true);
                        objmonster.transform.Translate(0.001f, 0, 0);
                    }

                    if (Mathf.Abs(px - mx) <= 1.0) // 몬스터와 플레이어의 위치가 같아지면 ( 리치가 닿는다면 )
                    {
                        if (!attacked)
                        {
                            animator.SetTrigger("attack");
                            attacked = true;
                            time = 0;
                        }
                    }
                }
                else animator.SetBool("move", false);   // 플레이어가 사거리 밖
            }
            else animator.SetBool("move", false);   // 플레이어가 사거리 밖
        }

    }
}
