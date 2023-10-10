using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola : MonoBehaviour
{
    private bool isArrived = true;
    private bool isGround = false;

    public float m_Speed = 10;
    public float m_HeightArc = 1;

    private GameObject room;

    private Vector3 targetPoint;
    private Vector3 m_StartPosition;

    public bool Arrived { get { return isArrived; } }

    void Start()
    {

        int x;
        if (Random.Range(0, 2) == 0) x = Random.Range(-5, 0);
        else x = Random.Range(1, 6);
        targetPoint = new Vector3(transform.position.x + x, transform.position.y + 1, transform.position.z);
        m_StartPosition = transform.position;

        for (int idx = 0; idx < GameObject.Find("GameController").GetComponent<GameController>().Room.Count; idx++)
        {
            room = GameObject.Find("GameController").GetComponent<GameController>().Room[idx];

            if (transform.position.x <= room.transform.position.x + 11.0f && transform.position.x >= room.transform.position.x - 11.0f)
            {
                if (transform.position.y <= room.transform.position.y + 7.5f && transform.position.y >= room.transform.position.y - 7.5f)
                {
                    break;
                }
            }
        }
    }

    void Update()
    {
        //if (GameObject.Find("GameController").GetComponent<GameController>().GoNext) Destroy(gameObject);

        if (!isArrived)
        {
            float x0 = m_StartPosition.x;
            float x1 = targetPoint.x;
            float distance = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, m_Speed * Time.deltaTime);
            float baseY = Mathf.Lerp(m_StartPosition.y, targetPoint.y, (nextX - x0) / distance);
            float arc = m_HeightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
            Vector3 nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);

            transform.rotation = LookAt2D(nextPosition - transform.position);
            transform.position = nextPosition;

            if (nextPosition == targetPoint) isArrived = true;
        }
        else
        {
            if (!isGround) GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (!room.GetComponent<Room>().isPlayer) Destroy(gameObject);
    }

    Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            GetComponent<Collider2D>().isTrigger = false;
            isArrived = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            GetComponent<Collider2D>().isTrigger = true;
        }

        if (collision.CompareTag("Ground") && GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            isGround = true;
            GetComponent<Collider2D>().isTrigger = false;
            isArrived = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<Collider2D>().isTrigger = true;

            switch(name)
            {
                case "골드(Clone)":
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().CalCulateHandMoney(10,'+');
                    break;
                case "골드주머니(Clone)":
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().CalCulateHandMoney(30, '+');
                    break;
                case "스캐럽(Clone)":
                    Debug.Log("스캐럽+1");
                    break;
                case "스캐럽주머니(Clone)":
                    Debug.Log("스캐럽+3");
                    break;
                default:
                    break;
            }

            if (gameObject.tag == "Wealth") Destroy(gameObject);
        }
    }
}
