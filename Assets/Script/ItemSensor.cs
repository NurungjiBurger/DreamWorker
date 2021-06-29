using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSensor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
        {
            GetComponent<Collider2D>().isTrigger = false;
        }

        if (collision.CompareTag("Player"))
        {
            // 플레이어가 아이템을 획득할 수 있는 상태라면
                if (collision.GetComponent<PlayerStatus>().Acquirable)
                {
                    // 아이템은 사라진다.
                    gameObject.SetActive(false);
                }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // 플레이어가 아이템을 획득할 수 있는 상태라면
                if (collision.collider.GetComponent<PlayerStatus>().Acquirable)
                {
                    // 아이템은 사라진다.
                    gameObject.SetActive(false);
                    //Destroy(gameObject);
                }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
