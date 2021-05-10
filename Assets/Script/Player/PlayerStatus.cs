using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{
    [SerializeField]
    private string occupation;
    [SerializeField]
    private GameObject basicItem;

    private bool acquirable = true;

    [SerializeField]
    List<GameObject> possessItemList = new List<GameObject>();
    //private GameObject[] possessItemList;
    private int possessItemNumber;
    [SerializeField]
    List<GameObject> equipItemList = new List<GameObject>();
    private int equipItemNumber;

    public List<GameObject> EquipItemList { get { return equipItemList; } }
    public List<GameObject> PossessItemList { get { return possessItemList; } }
    public void check() { Debug.Log(acquirable);  }
    public bool Acquirable { get { return acquirable; } }
    public string Occupation { get { return occupation; } }

    public void EquipItem(GameObject item)
    {
        // �������� ���������� ������ ���� ����Ʈ�� �ű�
        // �ش� �������� �ɼ��� ������ͼ� �÷��̾�ȿ� ���ϰų� ����.
        for (int i = possessItemList.Count - 1; i >= 0; i--)
        {
            if (possessItemList[i] == item)
            {
                equipItemList.Add(item);
                possessItemList.Remove(possessItemList[i]);
                item.GetComponent<ItemStatus>().EquipCheck(equipItemList);
            }
        }
    }

    public void DiscardItem()
    {
        // �������� ������ ������ǰ����Ʈ���� �ش� ��ǰ ����.
    }

    public void AcquireItem(GameObject item)
    {
        // �������� ������ ������ǰ����Ʈ�� �߰�.
        possessItemList.Add(item);
        GameObject.Find("Canvas").transform.Find("Inventory").transform.Find("InventoryBackground").gameObject.GetComponent<Inventory>().AddToInventory();
        GameObject.Find("Canvas").transform.Find("Inventory").transform.Find("InventoryBackground").transform.GetChild(possessItemList.Count-1).gameObject.GetComponent<Slot>().InsertImage(item);
        if (possessItemList.Count == 1 ) EquipItem(item);
    }

    // Start is called before the first frame update
    void Start()
    {
        possessItemNumber = 15;
        equipItemNumber = 5;

        GameObject obj;
        obj = Instantiate(basicItem, new Vector3(-1,-1,0), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        if (possessItemList.Count == possessItemNumber) acquirable = false;
        else acquirable = true;
    }
}
