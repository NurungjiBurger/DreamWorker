using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISensor : MonoBehaviour
{
    private bool mountAble = false;
    private bool toInventory = false;

    private GameObject inspector;
    private GameObject inventory;

    public bool MountAble { get { return mountAble ; } }
    public bool ToInventory { get { return toInventory; } }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (name.Equals("Slot(Clone)"))
        {
            if (collision.collider.name.Equals("Inventory"))
            {
                mountAble = false;
                toInventory = true;
            }
            else if (collision.collider.name.Equals("Inspector"))
            {
                mountAble = true;
                toInventory = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        /*
        if (name.Equals("Slot(Clone)"))
        {
            if (collision.collider.name.Equals("Inventory") || collision.collider.name.Equals("Inspector"))
            {
                Debug.Log("빠져나감");
                mountAble = false;
                toInventory = false;
            }
        }
        */
    }

    void Start()
    {
        
    }


    void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
    }
}
