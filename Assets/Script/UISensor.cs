using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISensor : MonoBehaviour
{
    /*
    enum Direction { Left, Right, Up, Down, Stop };

    private Direction dir = Direction.Stop;
    */
    private bool mountAble = false;
    private bool toInventory = false;

    private int direction = 0;

    private GameObject inspector;
    private GameObject inventory;

    public bool MountAble { get { return mountAble ; } }
    public bool ToInventory { get { return toInventory; } }
    public int Direction { get { return direction; } }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (name.Equals("Lever"))
        {
            if (collision.gameObject.name == "RightBox" || Input.GetKey(KeyCode.RightArrow)) direction = 1;
            else if (collision.gameObject.name == "LeftBox" || Input.GetKey(KeyCode.LeftArrow)) direction = 2;
            else if (collision.gameObject.name == "UpBox") direction = 3;
            else if (collision.gameObject.name == "DownBox" || Input.GetKey(KeyCode.DownArrow)) direction = 4;
            else //if (collision.gameObject.name == "StopBox") direction = 0;
            {
                direction = 0;
            }
        }
    }

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
