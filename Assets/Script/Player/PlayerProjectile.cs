using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    private GameObject player;

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
        if (collision.CompareTag("Monster"))
        {
            Instantiate(prefabEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        flip = player.GetComponent<SpriteRenderer>().flipX;

        GetComponent<SpriteRenderer>().flipX = flip;

        startPosition = transform.position;

        cnt = 0;

        if (isMove) flag = 1;
        else flag = 0;

        if (flip) dir = -1;
        else dir = 1;

    }

    private void Update()
    {
        // 사출된 자리에 고정
        if (flag == 0)
        {
            if ((startPosition.x + (attackRange * dir)) != this.transform.position.x && cnt < (attackRange * 60))
            {
                gameObject.transform.position = new Vector3(player.transform.position.x + (-0.55f * dir), player.transform.position.y, player.transform.position.z);
                flip = player.GetComponent<SpriteRenderer>().flipX;
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
