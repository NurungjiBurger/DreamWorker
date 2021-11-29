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
            for (int idx = 0; idx < 3; idx++)
            {
                GameObject tmp;
                tmp = items[0];
                items.Remove(items[0]);
                tmp.GetComponent<ItemStatus>().DestoryAll();
            }

            blackSmith.UIActive();
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

            slots[idx].GetComponent<Slot>().InsertImage(items[idx]);
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
        }

    }
}
