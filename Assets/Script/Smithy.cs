using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Smithy : MonoBehaviour
{
    private GameController controller;

    private Slot[] slots;
    private List<GameObject> items = new List<GameObject>();

    public void ItemPayment(GameObject obj)
    {
        for(int idx = 0; idx < 4; idx++)
        {
            // ������ ��������
            if (obj == items[0])
            {
                items.Remove(items[0]);
            }
            else
            {
                GameObject tmp;
                tmp = items[0];
                items.Remove(items[0]);
                tmp.GetComponent<ItemStatus>().DestoryAll();
            }
        }

        obj.transform.position = new Vector3(300, 300, 0);
        
        // �÷��̾� ��� ����
        // ���������� ��� �������� ���� ���ݺ��� 50% ���ε� �������� ���� ����.
    }

    private void ProductDisplay()
    {
        int itemidx;

        for (int idx = 0; idx < 4; idx++)
        {
            itemidx = Random.Range(0, controller.Items.Length);

            items.Add(Instantiate(controller.Items[itemidx], new Vector3(300, 310, 0), Quaternion.identity));
            items[idx].GetComponent<ItemStatus>().itemPrfNumber = itemidx;

            slots[idx].GetComponent<Slot>().InsertImage(items[idx]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {//
        if (gameObject.activeSelf == true && items.Count == 0)
        {
            if (slots == null)
            {
                slots = new Slot[4];

                for (int idx = 0; idx < 4; idx++) slots[idx] = transform.GetChild(0).GetChild(idx).gameObject.GetComponent<Slot>();
            }

            ProductDisplay();
        }
    }
}
