using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject player;

    private void checkdelete()
    {

        if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().GetGoNext())
        {
            Destroy(gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        checkdelete();
    }
}
