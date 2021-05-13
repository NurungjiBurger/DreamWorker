using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabSlot;

    private List<GameObject> possessItemList = new List<GameObject>();
   // private List<GameObject> inventoryItemList = new List<GameObject>();
    private bool acquirable = true;

    private GameObject player;

    private int possessItemNumber;

    public bool Acquirable { get { return acquirable; } }
    public int ItemCount { get { return possessItemList.Count; } }

    public void GoldText()
    {
        transform.parent.GetChild(1).transform.Find("Handmoney").GetComponent<Text>().text = player.GetComponent<PlayerStatus>().HandMoney.ToString();
    }

    public void MoveItem(GameObject item)
    {
        if (possessItemList.Count < 9)
        {
            possessItemList.Add(item);
            RerangeInventory();
        }
    }

    public void DiscardItem(int index)
    {
        // �������� ������ǰ����Ʈ���� ����.
        if (possessItemList.Count != 0)
        {
           // Debug.Log("index " + index);
            possessItemList.Remove(possessItemList[index]);
            RerangeInventory();
        }
    }

    public void AcquireItem(GameObject item)
    {
        // �������� ������ ������ǰ����Ʈ�� �߰�.
        AddToInventory(item);
        //RerangeInventory();
        //GameObject.Find("Canvas").transform.Find("Inventory(Clone)").transform.Find("InventoryBackground").transform.GetChild(possessItemList.Count - 1).GetComponent<Slot>().InsertImage(item);
    }

    public void RerangeInventory()
    {
        /*
        Debug.Log("����");
        int maxindex = possessItemList.Count;
        // �κ��丮 ������ ��ü ����
        Debug.Log(maxindex);
        for (int i = 0; i < maxindex; i++)
        {
            Destroy(possessItemList[i]);
        }

        // �κ��丮 ������ ��ü �߰�
        for (int i=0;i<maxindex;i++)
        {
            AddToInventory(possessItemList[i]);
        }
        */
    }

    public void AddToInventory(GameObject item)
    {
        GameObject tmp;
        tmp = Instantiate(prefabSlot, transform);
        possessItemList.Add(tmp);
        tmp.GetComponent<Slot>().InsertImage(item);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        possessItemNumber = 15;

    }

    // Update is called once per frame
    void Update()
    {
        if (possessItemList.Count == possessItemNumber) acquirable = false;
        else acquirable = true;

        GoldText();
    }
}
