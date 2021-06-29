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
            // �÷��̾ �������� ȹ���� �� �ִ� ���¶��
                if (collision.GetComponent<PlayerStatus>().Acquirable)
                {
                    // �������� �������.
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
            // �÷��̾ �������� ȹ���� �� �ִ� ���¶��
                if (collision.collider.GetComponent<PlayerStatus>().Acquirable)
                {
                    // �������� �������.
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
