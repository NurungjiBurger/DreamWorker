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

    [SerializeField]
    private GameObject prefabSlotButton;

    private List<GameObject> equipItemList = new List<GameObject>();
    private List<GameObject> possessItemList = new List<GameObject>();
    private List<bool> selectedList = new List<bool>();
    private int selectedNumber = 0;

    private void DeleteList(int size, List<GameObject> list)
    {
        if (size != 0)
        {
            GameObject tmp;
            for (int i = size - 1; i >= 0; i--)
            {
                tmp = list[i];
                list.Remove(possessItemList[i]);
                Destroy(tmp.gameObject);
            }
        }
    }

    public void DeleteAllList()
    {
        int size;
        size = equipItemList.Count;
        DeleteList(size, equipItemList);

        size = possessItemList.Count;
        DeleteList(size, possessItemList);

        size = selectedList.Count;
        if (size != 0) selectedList.RemoveAll(x => true);
    }

    public void EnrollItem()
    {
        int i = 0;

        for (i = 0; i < selectedList.Count; i++)
        {
            if (selectedList[i])
            {
                if (equipItemList.Count > i) equipItemList[i].GetComponent<Slot>().PutInExchanger();
                else possessItemList[i - equipItemList.Count].GetComponent<Slot>().PutInExchanger();
            }
        }

        DeleteAllList();
        exchanger.transform.Find("ButtonBackground").transform.Find("ChangeableButton").GetComponent<ButtonUI>().UIActive();
    }

    public void SelectItem(int index)
    {
        Color originColor;

        if (!selectedList[index])
        {
            if (selectedNumber < 4)
            {
                selectedNumber++;
                selectedList[index] = true;

                if (equipItemList.Count > index)
                {
                    originColor = equipItemList[index].GetComponent<Image>().color;
                    equipItemList[index].GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 100f / 255f);
                    originColor = equipItemList[index].transform.Find("Background").GetComponent<Image>().color;
                    equipItemList[index].transform.Find("Background").GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 100f / 255f);
                    originColor = equipItemList[index].transform.Find("Background").transform.Find("Item").GetComponent<Image>().color;
                    equipItemList[index].transform.Find("Background").transform.Find("Item").GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 100f / 255f);
                }
                else
                {
                    originColor = possessItemList[index - equipItemList.Count].GetComponent<Image>().color;
                    possessItemList[index - equipItemList.Count].GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 100f / 255f);
                    originColor = possessItemList[index - equipItemList.Count].transform.Find("Background").GetComponent<Image>().color;
                    possessItemList[index - equipItemList.Count].transform.Find("Background").GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 100f / 255f);
                    originColor = possessItemList[index - equipItemList.Count].transform.Find("Background").transform.Find("Item").GetComponent<Image>().color;
                    possessItemList[index - equipItemList.Count].transform.Find("Background").transform.Find("Item").GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 100f / 255f);
                }

            }
        }
        else
        {
            if (selectedNumber > 0)
            {
                selectedNumber--;
                selectedList[index] = false;

                if (equipItemList.Count > index)
                {
                    originColor = equipItemList[index].GetComponent<Image>().color;
                    equipItemList[index].GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 255f / 255f);
                    originColor = equipItemList[index].transform.Find("Background").GetComponent<Image>().color;
                    equipItemList[index].transform.Find("Background").GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 255f / 255f);
                    originColor = equipItemList[index].transform.Find("Background").transform.Find("Item").GetComponent<Image>().color;
                    equipItemList[index].transform.Find("Background").transform.Find("Item").GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 255f / 255f);
                }
                else
                {
                    originColor = possessItemList[index - equipItemList.Count].GetComponent<Image>().color;
                    possessItemList[index - equipItemList.Count].GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 255f / 255f);
                    originColor = possessItemList[index - equipItemList.Count].transform.Find("Background").GetComponent<Image>().color;
                    possessItemList[index - equipItemList.Count].transform.Find("Background").GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 255f / 255f);
                    originColor = possessItemList[index - equipItemList.Count].transform.Find("Background").transform.Find("Item").GetComponent<Image>().color;
                    possessItemList[index - equipItemList.Count].transform.Find("Background").transform.Find("Item").GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 255f / 255f);
                }
            }
        }
    }

    public void LoadAllItem()
    {
        int size;
        size = inspector.GetComponent<Inspector>().ItemCount;

        for(int i=0;i<size;i++)
        {
            GameObject tmp;
            tmp = Instantiate(prefabSlotButton, transform.Find("Background").transform);
            tmp.GetComponent<Slot>().InsertImage(inspector.GetComponent<Inspector>().EquipItemList[i].SlotItem);
            equipItemList.Add(tmp);
            selectedList.Add(false);
        }

        size = inventory.GetComponent<Inventory>().ItemCount;

        for (int i=0;i<size;i++)
        {
            GameObject tmp;
            tmp = Instantiate(prefabSlotButton, transform.Find("Background").transform);
            tmp.GetComponent<Slot>().InsertImage(inventory.GetComponent<Inventory>().PossessItemList[i].SlotItem);
            possessItemList.Add(tmp);
            selectedList.Add(false);
        }
    }

    private void TextUpdate()
    {
        transform.Find("SelectNumberBackground").transform.Find("SelectNumber").GetComponent<TextMeshProUGUI>().text = selectedNumber.ToString() + " / 4"; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (!exchanger) exchanger = GameObject.Find("Canvas").transform.Find("Exchanger").gameObject;

        TextUpdate();
    }
}
