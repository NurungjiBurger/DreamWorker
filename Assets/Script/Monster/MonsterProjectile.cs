using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProjectile : Projectile
{
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� Ÿ�ݼ���!");
            if (prefabEffect != null)
            {
                Instantiate(prefabEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            }
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player.transform.position.x - transform.position.x > 0) flip = true;
        else flip = false;

        GetComponent<SpriteRenderer>().flipX = flip;

        startPosition = transform.position;

        cnt = 0;
        flag = 1;

        if (flip) dir = -1;
        else dir = 1;

    }

    private void Update()
    {
        // ����Ǿ� �־���
        if (flag == 1)
        {
            if ((startPosition.x + (attackRange * dir)) != this.transform.position.x && cnt < (attackRange * 60))
            {
                gameObject.transform.Translate(-0.02f * dir, 0, 0);
                cnt++;
            }
            else
            {
                // ���� ��ȯ
                if (isComBack)
                {
                    flag = 2;
                    GetComponent<SpriteRenderer>().flipX = !flip;
                }
                else flag = 3;
            }
        }

        // ������ ��ġ�� ���ƿ�
        else if (flag == 2)
        {
            if (startPosition.x != this.transform.position.x && cnt < (attackRange * 60 * 2))
            {
                gameObject.transform.Translate(0.02f * dir, 0, 0);
                cnt++;
            }
            else
            {
                // ��
                flag = 3;
            }
        }

        else if (flag == 3)
        {
            Destroy(gameObject);
        }
    }
}
