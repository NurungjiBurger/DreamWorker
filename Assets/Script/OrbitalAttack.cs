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
    void Start()
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
    void Update()
    {
        // 플레이어에게서 사출되어 멀어짐
        if (flag == 1)
        {
            if ((startPosition.x + (attackRange * dir)) != this.transform.position.x && cnt < (attackRange * 60))
            {
                gameObject.transform.Translate(-0.02f * dir, 0, 0);
                cnt++;
            }
            else
            {
                // 방향 전환
                flag = 2;
                GetComponent<SpriteRenderer>().flipX = !flip;
            }
        }

        // 원래 플레이어의 위치로 돌아옴
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
                Destroy(gameObject);
            }
        }
    }
}
