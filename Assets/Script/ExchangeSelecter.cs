using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExchangeSelecter : MonoBehaviour
{
    private GameObject inventory;
    private GameObject inspector;
    private GameObject exchanger;

    private List<Slot> itemList = new List<Slot>();

    private List<bool> selectedList = new List<bool>();
    private int selectedNumber = 0;

    public List<bool> SelectedList { get { return selectedList; } }

    public void EnrollItem()
    {
        for (int i = 0; i < selectedList.Count; i++)
        {

            itemList[i].GetComponent<Slot>().SetOriginColor();

            if (itemList[i].SlotItem.GetComponent<ItemStatus>().IsMount) inspector.GetComponent<Inspector>().AddToInspector(itemList[i]);
            else inventory.GetComponent<Inventory>().AddToInventory(itemList[i].GetComponent<Slot>());

            if (selectedList[i])
            {
                Debug.Log(itemList[i]);
                itemList[i].GetComponent<Slot>().PutInExchanger();
            }
        }

        exchanger.transform.Find("ButtonBackground").transform.Find("ChangeableButton").GetComponent<ButtonUI>().UIActive();
    }

    public void DeleteAllList()
    {
        EnrollItem();

        int size = itemList.Count;

        if (size != 0)
        {
            for (int i = size - 1; i >= 0; i--)
            {
                itemList.Remove(itemList[i]);
            }
        }

        size = selectedList.Count;

        if (size != 0)
        {
            for (int i = size - 1;i >= 0;i--)
            {
                selectedList.Remove(selectedList[i]);
            }
        }

        selectedNumber = 0;
    }

    public void SelectItem(Slot slot, int index)
    {
        if (!selectedList[index])
        {
            if (selectedNumber < 4)
            {
                selectedNumber++;
                selectedList[index] = true;

                slot.SetTransParentColor();

            }
        }
        else
        {
            if (selectedNumber > 0)
            {
                selectedNumber--;
                selectedList[index] = false;

                slot.SetOriginColor();
            }
        }
    }

    public void LoadAllItem()
    {
        int size;

        // exchanger
        size = exchanger.GetComponent<Exchanger>().ItemCount;
        for(int i = 0;i < size;i++)
        {
            Slot tmp;
            tmp = exchanger.GetComponent<Exchanger>().ExchangeItemList[0];
            if (tmp.SlotItem.GetComponent<ItemStatus>().IsMount)
            {
                exchanger.GetComponent<Exchanger>().ExchangeItemList.Remove(tmp);
                tmp.GetComponent<Slot>().PullOutExchanger();
                inspector.GetComponent<Inspector>().DiscardToInspector(tmp);
            }
            else
            {
                exchanger.GetComponent<Exchanger>().ExchangeItemList.Remove(tmp);
                tmp.GetComponent<Slot>().PullOutExchanger();
                inventory.GetComponent<Inventory>().DiscardToInventory(inventory.GetComponent<Inventory>().ItemCount - 1);
            }
            itemList.Add(tmp);
            tmp.transform.SetParent(transform.Find("Background").transform);
            tmp.GetComponent<RectTransform>().sizeDelta = new Vector2(36, 36);
            tmp.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
            selectedList.Add(false);
            SelectItem(tmp, i);
        }

        // inspector
        size = inspector.GetComponent<Inspector>().ItemCount;
        for(int i = 0;i < size;i++)
        {
            itemList.Add(inspector.GetComponent<Inspector>().EquipItemList[0]);
            inspector.GetComponent<Inspector>().EquipItemList[0].transform.SetParent(transform.Find("Background").transform);
            inspector.GetComponent<Inspector>().EquipItemList[0].GetComponent<RectTransform>().sizeDelta = new Vector2(36, 36);
            inspector.GetComponent<Inspector>().EquipItemList[0].transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
            inspector.GetComponent<Inspector>().DiscardToInspector(inspector.GetComponent<Inspector>().EquipItemList[0]);
            selectedList.Add(false);
        }

        // inventory
        size = inventory.GetComponent<Inventory>().ItemCount;
        for (int i = 0;i < size;i++)
        {
            itemList.Add(inventory.GetComponent<Inventory>().PossessItemList[0]);
            inventory.GetComponent<Inventory>().PossessItemList[0].transform.SetParent(transform.Find("Background").transform);
            inventory.GetComponent<Inventory>().PossessItemList[0].GetComponent<RectTransform>().sizeDelta = new Vector2(36, 36);
            inventory.GetComponent<Inventory>().PossessItemList[0].transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
            inventory.GetComponent<Inventory>().DiscardToInventory(0);
            selectedList.Add(false);
        }
    }

    private void TextUpdate()
    {
        transform.Find("SelectNumberBackground").transform.Find("SelectNumber").GetComponent<TextMeshProUGUI>().text = selectedNumber.ToString() + " / 4"; //(exchanger.GetComponent<Exchanger>().ItemCount + selectedNumber).ToString() + " / 4"; 
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (!exchanger) exchanger = GameObject.Find("Canvas").transform.Find("Exchanger").gameObject;

        TextUpdate();
    }
}
