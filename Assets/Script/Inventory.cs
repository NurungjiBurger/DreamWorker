using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabSlot;

    private List<Slot> possessItemList = new List<Slot>();
   // private List<GameObject> inventoryItemList = new List<GameObject>();
    private bool acquirable = true;

    private GameObject player;

    private int possessItemNumber;

    public bool Acquirable { get { return acquirable; } }
    public int ItemCount { get { return possessItemList.Count; } }

    public void GoldText()
    {
          transform.parent.transform.Find("HandMoneyBackground").transform.Find("HandMoney").GetComponent<TextMeshProUGUI>().text = player.GetComponent<PlayerStatus>().HandMoney.ToString();
    }

    public void DiscardToInventory(int index)
    {
        if (possessItemList.Count != 0) possessItemList.Remove(possessItemList[index]);
    }

    public void AddToInventory(Slot slot)
    {
        if (possessItemList.Count < 16)
        {
            possessItemList.Add(slot);

            slot.transform.SetParent(transform);
            slot.transform.position = slot.transform.parent.position;
            slot.GetComponent<RectTransform>().sizeDelta = new Vector2(36, 36);
            slot.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        possessItemNumber = 16;
    }

    // Update is called once per frame
    void Update()
    {
        if (possessItemList.Count < possessItemNumber) acquirable = true;
        else acquirable = false;

        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        GoldText();
    }
}
