using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISensor : MonoBehaviour
{
    private bool discardAble = false;
    private bool mountAble = false;
    private bool toInventory = false;

    private GameObject inspector;
    private GameObject inventory;

    public bool DiscardAble { get { return discardAble; } }
    public bool MountAble { get { return mountAble ; } }
    public bool ToInventory { get { return toInventory; } }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �������� ������ ��� �ִ� ������ ��쿡�� ��ȭ�� ����.
        if (name.Equals("Slot(Clone)"))
        {
             if (collision.collider.name.Equals("Slot(Clone)"))
            {
               // Debug.Log("����");
                discardAble = false;
                if (collision.collider.transform.name == inspector.name) mountAble = true;
                else mountAble = false;
                if (collision.collider.transform.name == inventory.name) toInventory = true;
                else toInventory = false;
            }
            else if (collision.collider.name.Equals("Inventory"))
            {
              // Debug.Log("�κ�");
                discardAble = false;
                mountAble = false;
                toInventory = true;
            }
            else if (collision.collider.name.Equals("Inspector"))
            {
               // Debug.Log("������");
                discardAble = false;
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
            if (collision.collider.name.Equals("Inventory(Clone)") || collision.collider.name.Equals("Inspector(Clone)") || collision.collider.name.Equals("Exchanger(Clone)"))
            {
                discardAble = true;
                mountAble = false;
                toInventory = false;
                changeAble = false;
            }
        }
        */
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
    }
}
