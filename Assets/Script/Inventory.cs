using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabSlot;

    private bool acquirable = true;

    private int possessItemNumber;

    private GameObject player;

    private GameData data;
    private List<Slot> possessItemList = new List<Slot>();

    public bool Acquirable { get { return acquirable; } }
    public int ItemCount { get { return possessItemList.Count; } }
    public List<Slot> PossessItemList { get { return possessItemList; } }

    // 소지금 및 아이템조각 표시
    public void StatusText()
    {
        transform.Find("HandMoneyBackground").transform.Find("HandMoney").GetComponent<TextMeshProUGUI>().text = player.GetComponent<PlayerStatus>().HandMoney.ToString();
        transform.Find("ItemPiece").transform.Find("Number").transform.Find("Text").GetComponent<TextMeshProUGUI>().text = player.GetComponent<PlayerStatus>().Data.itemPiece.ToString();
    }

    // 인벤토리에서 빼냄
    public Slot DiscardToInventory(int index)
    {
        if (possessItemList.Count != 0)
        {
            Slot tmp;
            tmp = possessItemList[index];
            possessItemList.Remove(possessItemList[index]);
            return tmp;
        }
        else return null;
    }

    // 아이템 획득
    public void AddToInventory(Slot slot)
    {

        if (possessItemList.Count < 20)
        {
            possessItemList.Add(slot);

            slot.transform.SetParent(transform.Find("Background").transform);
            slot.SlotItem.GetComponent<ItemStatus>().IsMount = false;
            slot.SlotItem.SetActive(false);
        }
    }

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;
    }

    void Start()
    {
        possessItemNumber = 20;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (data == null) data = GameObject.Find("Data").GetComponent<DataController>().GameData;
        if (possessItemList.Count < possessItemNumber) acquirable = true;
        else acquirable = false;

        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        else StatusText();
    }
}
