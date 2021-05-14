using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISensor : MonoBehaviour
{
    // �κ����� �����ͷ� ����
    // �����Ϳ��� �κ����� Ż�� // �����Ϳ��� �κ����� ��ȯŻ��?
    // �κ����� ������ ������

    private bool discardAble = false;
    private bool mountAble = false;
    private bool toInventory = false;
    private bool changeAble = false;

    private GameObject inspector;
    private GameObject inventory;

    public bool ChangeAble { get { return changeAble; } }
    public bool DiscardAble { get { return discardAble; } }
    public bool MountAble { get { return mountAble ; } }
    public bool ToInventory { get { return toInventory; } }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �������� ������ ��� �ִ� ������ ��쿡�� ��ȭ�� ����.
        if (name.Equals("Slot(Clone)"))
        {
            if (collision.collider.name.Equals("Head") || collision.collider.name.Equals("Glove") || collision.collider.name.Equals("Suit") || collision.collider.name.Equals("Shoe") || collision.collider.name.Equals("Weapon"))
            {
               // Debug.Log("���");
                discardAble = false;
                mountAble = true;
                toInventory = false;
                changeAble = false;
            }
            else if (collision.collider.name.Equals("Slot(Clone)"))
            {
               // Debug.Log("����");
                discardAble = false;
                if (collision.collider.transform.parent == inspector) mountAble = true;
                else mountAble = false;
                if (collision.collider.transform.parent == inventory) toInventory = true;
                else toInventory = false;
                changeAble = false;
            }
            else if (collision.collider.name.Equals("Inventory(Clone)"))
            {
               // Debug.Log("�κ�");
                discardAble = false;
                mountAble = false;
                toInventory = true;
                changeAble = false;
            }
            else if (collision.collider.name.Equals("Inspector(Clone)"))
            {
               // Debug.Log("������");
                discardAble = false;
                mountAble = true;
                toInventory = false;
                changeAble = false;
            }
            else if (collision.collider.name.Equals("Exchanger(Clone)"))
            {
               // Debug.Log("ü����");
                discardAble = false;
                mountAble = false;
                toInventory = false;
                changeAble = true;
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
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory(Clone)").transform.Find("InventoryBackground").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector(Clone)").transform.Find("Background").gameObject;
    }
}
