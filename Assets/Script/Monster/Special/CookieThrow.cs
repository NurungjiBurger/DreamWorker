using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieThrow : OneWayThrow
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { 
            Instantiate(prefabEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        SetPosition();
    }

    // Update is called once per frame
    private void Update()
    {
        OnGoingOneWay();
    }
}
