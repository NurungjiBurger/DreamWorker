using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalAttack : Projectile
{
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            Instantiate(prefabEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        flip = player.GetComponent<SpriteRenderer>().flipX;

        GetComponent<SpriteRenderer>().flipX = flip;

        startPosition = transform.position;

        cnt = 0;
        flag = 1;

        if (flip) dir = -1;
        else dir = 1;

    }

    private void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    private void Update()
    {
        // �÷��̾�Լ� ����Ǿ� �־���
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
                flag = 2;
                GetComponent<SpriteRenderer>().flipX = !flip;
            }
        }

        // ���� �÷��̾��� ��ġ�� ���ƿ�
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
                Destroy(gameObject);
            }
        }
    }
}
