using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField]
    protected GameObject prefabEffect;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected bool isComBack;
    [SerializeField]
    protected bool isMove;
    
    protected int cnt;
    protected int flag;
    protected int dir;

    protected Vector3 startPosition;
    protected Animator animator;
    protected bool flip;

    private GameObject entity;
    private GameObject weapon;

    float x, y, sizex, sizey;

    public void Inputx(float mx) { x = mx; }
    public void Inputy(float my) { y = my; }
    public void Inputsizex(float msizex) { sizex = msizex; }
    public void Inputsizey(float msizey) { sizey = msizey; }

    public void ModifyTrigger()
    {
        GetComponent<BoxCollider2D>().offset = new Vector3(x, y, 0);
        GetComponent<BoxCollider2D>().size = new Vector3(sizex, sizey, 0);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && gameObject.tag == "Player_attack_judgement")
        {
           // Instantiate(prefabEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }

        if (collision.CompareTag("Player") && gameObject.tag == "Monster_attack_judgement")
        {
           // Debug.Log("플레이어피격!");
            if (prefabEffect != null)
            {
               // Instantiate(prefabEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            }
        }
    }

    public void EntitySetting(GameObject obj)
    {
        entity = obj;

        flip = entity.GetComponent<ObjectFlip>().flipX;

        GetComponent<SpriteRenderer>().flipX = !flip;

        startPosition = transform.position;

        cnt = 0;

        if (isMove) flag = 1;
        else flag = 0;

        if (flip) dir = -1;
        else dir = 1;
    }

    public void WeaponSetting(GameObject obj)
    {
        weapon = obj;

        flip = weapon.GetComponent<ObjectFlip>().flipX;

        GetComponent<SpriteRenderer>().flipX = !flip;

    }

    private void Start()
    {

    }

    private void Update()
    {
        // 사출된 자리에 고정
        if (flag == 0)
        {
            if ((startPosition.x + (attackRange * dir)) != this.transform.position.x && cnt < (attackRange * 60))
            {
               // gameObject.transform.position = weapon.transform.position;
                GetComponent<SpriteRenderer>().flipX = flip;
                cnt++;
            }
            else flag = 3;
        }

        // 사출되어 멀어짐
        else if (flag == 1)
        {
            if ((startPosition.x + (attackRange * dir)) != this.transform.position.x && cnt < (attackRange * 60))
            {
                gameObject.transform.Translate(-0.02f * dir, 0, 0);
                cnt++;
            }
            else
            {
                // 방향 전환
                if (isComBack)
                {
                    flag = 2;
                    GetComponent<SpriteRenderer>().flipX = !flip;
                }
                else flag = 3;
            }
        }

        // 원래의 위치로 돌아옴
        else if (flag == 2)
        {
            if (startPosition.x != this.transform.position.x && cnt < (attackRange * 60 * 2))
            {
                gameObject.transform.Translate(0.02f * dir, 0, 0);
                cnt++;
            }
            else
            {
                // 끝
                flag = 3;
            }
        }

        else if (flag == 3)
        {
            Destroy(gameObject);
        }
    }
}
