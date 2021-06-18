using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject player;
    private Portal connectPortal;

    public Vector3 ConnectPosition { get { return connectPortal.transform.position; } }

    public void PositionSave(Portal portal)
    {
        connectPortal = portal;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player").gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
