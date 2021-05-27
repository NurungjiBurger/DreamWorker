using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject player;

    private void checkdelete()
    {
        if(player.GetComponent<PlayerMovement>().GetGoNext())
        {
            Destroy(gameObject);
        }

    }
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player").gameObject;

        checkdelete();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
