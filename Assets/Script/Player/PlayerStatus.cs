using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    private Dictionary<MountingPart, GameObject> equipItemList = new Dictionary<MountingPart, GameObject>();
    private int equipItemNumber;

    enum MountingPart { Head, Suit, Glove, Shoe, Weapon }

    private MountingPart part = MountingPart.Weapon;

    public Dictionary<MountingPart, GameObject> EquipItemList { get { return equipItemList; } }
    public List<GameObject> PossessItemList { get { return possessItemList; } }
    public void check() { Debug.Log(acquirable);  }
    public bool Acquirable { get { return acquirable; } }
    public string Occupation { get { return occupation; } }

    public int ReturnIndex(GameObject item)
    {
        switch (item.GetComponent<ItemStatus>().MountingPart)
        {
            case "head":
                part = MountingPart.Head;
                break;
            case "suit":
                part = MountingPart.Suit;
                break;
            case "glove":
                part = MountingPart.Glove;
                break;
            case "shoe":
                part = MountingPart.Shoe;
                break;
            case "weapon":
                part = MountingPart.Weapon;
                break;
        }

        return (int)part;
    }

    public void CalCulateStat(GameObject item, int how)
    {
        defenseCapability += item.GetComponent<ItemStatus>().Defense * how;
        MaxHP += item.GetComponent<ItemStatus>().MaxHP * how;
        Power += item.GetComponent<ItemStatus>().Power * how;
        if (how == 1)
        {
            AttackSpeed *= item.GetComponent<ItemStatus>().AttackSpeed;
            jumpPower *= item.GetComponent<ItemStatus>().JumpPower;
            moveSpeed *= item.GetComponent<ItemStatus>().MoveSpeed;
        }
        else
        {
            AttackSpeed /= item.GetComponent<ItemStatus>().AttackSpeed;
            jumpPower /= item.GetComponent<ItemStatus>().JumpPower;
            moveSpeed /= item.GetComponent<ItemStatus>().MoveSpeed;
        }

    }

    public void EquipItem(int index)
    {
        Debug.Log(index);
        // 아이템을 장착했을때 아이템 장착 리스트로 옮김
        GameObject tmp;

        ReturnIndex(possessItemList[index]);

        if (!equipItemList.ContainsKey(part))
        {
            Debug.Log("널이니까 넣어");
            equipItemList.Add(part, possessItemList[index]);
            CalCulateStat(possessItemList[index], 1);   // +
            possessItemList.Remove(possessItemList[index]);
            GameObject.Find("Canvas").transform.Find("Inventory(Clone)").transform.Find("InventoryBackground").transform.GetChild(index).GetComponent<Slot>().DestroyObject();
        }
        else
        {
            Debug.Log("교체");
            tmp = equipItemList.ContainsKey(part).
            equipItemList[(int)part] = possessItemList[index];
            possessItemList[index] = tmp;
            CalCulateStat(equipItemList[(int)part], -1);    // -
            CalCulateStat(possessItemList[index], 1);   // +
        }
    }

    public void DiscardItem(int index)
    {
        // 아이템을 버리면 소유물품리스트에서 해당 물품 삭제.
        possessItemList.Remove(possessItemList[index]);
    }

    public void AcquireItem(GameObject item)
    {
        // 아이템을 먹으면 소유물품리스트에 추가.
        possessItemList.Add(item);
        GameObject.Find("Canvas").transform.Find("Inventory(Clone)").transform.Find("InventoryBackground").gameObject.GetComponent<Inventory>().AddToInventory();
        GameObject.Find("Canvas").transform.Find("Inventory(Clone)").transform.Find("InventoryBackground").transform.GetChild(possessItemList.Count - 1).GetComponent<Slot>().InsertImage(item);
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
