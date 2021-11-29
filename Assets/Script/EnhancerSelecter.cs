using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnhancerSelecter : MonoBehaviour
{
    [SerializeField]
    private GameObject inventory;
    [SerializeField]
    private GameObject inspector;
    [SerializeField]
    private GameObject Enhancer;

    private int selectedNumber = 0;

    private List<Slot> itemList = new List<Slot>();
    private List<bool> selectedList = new List<bool>();

    public List<bool> SelectedList { get { return selectedList; } }


    public void EnrollItem()
    {
        for (int i = 0; i < selectedList.Count; i++)
        {
            if (itemList[i].SlotItem.GetComponent<ItemStatus>().IsMount) inspector.GetComponent<Inspector>().AddToInspector(itemList[i]);
            else inventory.GetComponent<Inventory>().AddToInventory(itemList[i].GetComponent<Slot>());

            if (selectedList[i])
            {
                itemList[i].GetComponent<Slot>().PutInEnhancer();
            }
        }

        Enhancer.transform.Find("ButtonBackground").transform.Find("LoadButton").GetComponent<ButtonUI>().UIActive();
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

                slot.ShowSelect(true);

            }
        }
        else
        {
            if (selectedNumber > 0)
            {
                selectedNumber--;
                selectedList[index] = false;

                slot.ShowSelect(false);
            }
        }
    }

    // 모든 아이템 표기
    public void LoadAllItem()
    {
        Update();

        int size;

        size = Enhancer.GetComponent<Enhancer>().ItemCount;

        for(int i = 0;i < size;i++)
        {
            Slot tmp;
            tmp = Enhancer.GetComponent<Enhancer>().DevoteItemList[0];
            if (tmp.SlotItem.GetComponent<ItemStatus>().IsMount)
            {
                Enhancer.GetComponent<Enhancer>().DevoteItemList.Remove(tmp);
                tmp.GetComponent<Slot>().PullOutEnhancer();
                inspector.GetComponent<Inspector>().DiscardToInspector(tmp);
            }
            else
            {
                Enhancer.GetComponent<Enhancer>().DevoteItemList.Remove(tmp);
                tmp.GetComponent<Slot>().PullOutEnhancer();
                inventory.GetComponent<Inventory>().DiscardToInventory(inventory.GetComponent<Inventory>().ItemCount - 1);
            }
            itemList.Add(tmp);
            tmp.transform.SetParent(transform.Find("Background").transform);
            selectedList.Add(false);
            SelectItem(tmp, i);
        }

        // inspector 아이템 
        size = inspector.GetComponent<Inspector>().ItemCount;
        for (int i = size-1; i >= 0; i--)
        {
            if (Enhancer.GetComponent<Enhancer>().EnhanceItem == inspector.GetComponent<Inspector>().EquipItemList[i]) continue;

            itemList.Add(inspector.GetComponent<Inspector>().EquipItemList[i]);
            inspector.GetComponent<Inspector>().EquipItemList[i].transform.SetParent(transform.Find("Background").transform);
            inspector.GetComponent<Inspector>().DiscardToInspector(inspector.GetComponent<Inspector>().EquipItemList[i]);
            selectedList.Add(false);
        }

        // inventory 아이템
        size = inventory.GetComponent<Inventory>().ItemCount;
        for (int i = 0;i < size;i++)
        {

            if (Enhancer.GetComponent<Enhancer>().EnhanceItem == inventory.GetComponent<Inventory>().PossessItemList[0]) continue;

            itemList.Add(inventory.GetComponent<Inventory>().PossessItemList[0]);
            inventory.GetComponent<Inventory>().PossessItemList[0].transform.SetParent(transform.Find("Background").transform);
            inventory.GetComponent<Inventory>().DiscardToInventory(0);
            selectedList.Add(false);
        }
    }

    private void TextUpdate()
    {
        transform.Find("SelectNumberBackground").transform.Find("SelectNumber").GetComponent<TextMeshProUGUI>().text = selectedNumber.ToString() + " / 4"; //(Enhancer.GetComponent<Enhancer>().ItemCount + selectedNumber).ToString() + " / 4"; 
    }

    void Start()
    {

    }

    void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (!Enhancer) Enhancer = GameObject.Find("Canvas").transform.Find("Enhancer").gameObject;

        TextUpdate();
    }
}
