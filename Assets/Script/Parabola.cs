using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola : MonoBehaviour
{
    private Vector3 targetPoint;
    public float m_Speed = 10;
    public float m_HeightArc = 1;
    private Vector3 m_StartPosition;
    private bool isArrived = false;
    private bool isGround = false;

    public bool Arrived { get { return isArrived; } }

    void Start()
    {
        int x;
        if (Random.Range(0, 2) == 0) x = Random.Range(-5, 0);
        else x = Random.Range(1, 6);
        targetPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z);
        m_StartPosition = transform.position;
    }

    void Update()
    {
        if (GameObject.Find("GameController").GetComponent<GameController>().GoNext) Destroy(gameObject);

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
    }

    Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            //isarrived = true;
            GetComponent<Collider2D>().isTrigger = false;
            isGround = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }

    }
}
