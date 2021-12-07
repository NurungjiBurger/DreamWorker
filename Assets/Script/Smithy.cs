using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Smithy : MonoBehaviour
{
    private GameObject player;

    private GameController controller;
    private ButtonUI blackSmith;
    private Slot[] slots;
    private List<GameObject> items = new List<GameObject>();

    public void Clear()
    {
        int length = items.Count;
        for (int idx = 0; idx < length; idx++)
        {
            GameObject tmp;
            tmp = items[0];
            items.Remove(items[0]);
            tmp.GetComponent<ItemStatus>().DestoryAll();
        }
    }

    // ������ ����
    public void ItemPayment(GameObject obj)
    {
        bool clear = false;

        for (int idx = 0; idx < 4; idx++)
        {
            // ������ ��������
            if (obj == items[idx])
            {
                // �÷��̾� ��� ����
                //// ���������� ��� �������� ���� ���ݺ��� 50% ���ε� �������� ���� ����. ////
                if (player.GetComponent<PlayerStatus>().CalCulateHandMoney(obj.GetComponent<ItemStatus>().Price, '-'))
                {
                    items.Remove(items[idx]);
                    obj.transform.position = new Vector3(300, 300, 0);
                    obj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

                    clear = true;
                    blackSmith.UIActive();
                }
                else
                {
                    return;
                }
                break;
            }
        }

        if (clear)
        {
            Clear();
            blackSmith.BlackSmith();
        }
    }

    // ������ ǥ��
    private void ProductDisplay()
    {
        int itemidx;

        // ������ 4�� ����
        for (int idx = 0; idx < 4; idx++)
        {
            itemidx = Random.Range(0, controller.Items.Length);

            items.Add(Instantiate(controller.Items[itemidx], new Vector3(300, 310, 0), Quaternion.identity));
            items[idx].GetComponent<ItemStatus>().itemPrfNumber = itemidx;
            items[idx].GetComponent<ItemStatus>().Start();

            //GameObject.Find("GameController").GetComponent<GameController>().CreateItemSlot(items[idx], false);
            //slots[idx].GetComponent<Slot>().InsertImage(items[idx]);
            if (slots[idx] == null) slots[idx] = transform.GetChild(0).GetChild(idx).gameObject.GetComponent<Slot>();

            slots[idx].InsertImage(items[idx]);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        if (!controller) controller = GameObject.Find("GameController").GetComponent<GameController>();
        if (!blackSmith) blackSmith = GameObject.Find("BlackSmith").transform.Find("BlackSmithButton").GetComponent<ButtonUI>();

        if (items.Count == 0)
        {
            
            if (slots == null)
            {
                slots = new Slot[4];

                for (int idx = 0; idx < 4; idx++) slots[idx] = transform.GetChild(0).GetChild(idx).gameObject.GetComponent<Slot>();
            }
            else ProductDisplay();
            
           // ProductDisplay();
        }

    }
}
