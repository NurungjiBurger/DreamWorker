using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabSlot;

    private GameData data;
    private bool acquirable = true;

    private GameObject player;

    private List<Slot> possessItemList = new List<Slot>();
    private int possessItemNumber;

    public bool Acquirable { get { return acquirable; } }
    public int ItemCount { get { return possessItemList.Count; } }
    public List<Slot> PossessItemList { get { return possessItemList; } }

    public void GoldText()
    {
          transform.Find("HandMoneyBackground").transform.Find("HandMoney").GetComponent<TextMeshProUGUI>().text = player.GetComponent<PlayerStatus>().HandMoney.ToString();
    }

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

    public void AddToInventory(Slot slot)
    {

        if (possessItemList.Count < 30)
        {
            possessItemList.Add(slot);

            slot.transform.SetParent(transform.Find("Background").transform);
            slot.GetComponent<RectTransform>().sizeDelta = new Vector2(36, 36);
            slot.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
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
        possessItemNumber = 30;
    }

    void Update()
    {
        if (data == null) data = GameObject.Find("Data").GetComponent<DataController>().GameData;
        if (possessItemList.Count < possessItemNumber) acquirable = true;
        else acquirable = false;

        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        else GoldText();
    }
}
