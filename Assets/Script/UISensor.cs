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
            if (collision.collider.name.Equals("Head") || collision.collider.name.Equals("Glove") || collision.collider.name.Equals("Suit") || collision.collider.name.Equals("Shoe") || collision.collider.name.Equals("Weapon"))
            {
                discardAble = false;
                mountAble = true;
                toInventory = false;
            }
            if (collision.collider.name.Equals("Slot(Clone)"))
            {
                discardAble = false;
                if (collision.collider.transform.parent == inspector) mountAble = false;
                else mountAble = true;
                if (collision.collider.transform.parent == inventory) toInventory = true;
                else toInventory = false;
            }
            if (collision.collider.name.Equals("Inventory(Clone)"))
            {
                discardAble = false;
                mountAble = false;
                toInventory = true;
            }
            if (collision.collider.name.Equals("Inspector(Clone)"))
            {
                discardAble = false;
                mountAble = false;
                toInventory = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (name.Equals("Slot(Clone)"))
        {
            if (collision.collider.name.Equals("Inventory(Clone)"))
            {
                discardAble = true;
                mountAble = false;
                toInventory = false;
            }
            if (collision.collider.name.Equals("Inspector(Clone)"))
            {
                discardAble = true;
                mountAble = false;
                toInventory = false;
            }
        }
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
