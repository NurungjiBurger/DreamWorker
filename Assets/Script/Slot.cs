using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabUI;

    private GameObject ui;
    private GameObject slotItem;

    private Sprite itemImage;

    private GameObject player;
    private GameObject inventory;
    private GameObject inspector;
    private GameObject exchanger;

    private GameObject originParent = null;
    private int originIndex = -1;

    public GameObject SlotItem { get { return slotItem; } }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void CompareMountItem()
    {
        if (transform.parent.gameObject == inventory)
        {
            if (inspector.transform.Find(slotItem.GetComponent<ItemStatus>().MountingPart).childCount == 1)
            {
                if (inspector.transform.Find(slotItem.GetComponent<ItemStatus>().MountingPart).transform.Find("Slot(Clone)").GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().ItemGrade > slotItem.GetComponent<ItemStatus>().ItemGrade)
                {
                    if (!ui) ui = Instantiate(prefabUI, transform);
                }
                else
                {
                    if (ui) Destroy(ui.gameObject);
                }
            }
        }
        else
        {
            if (ui) Destroy(ui.gameObject);
        }
    }

    
    public void PutInExchanger()
    {
        if (!originParent && originIndex == -1)
        {
            originParent = transform.parent.gameObject;
            originIndex = transform.GetSiblingIndex();

            exchanger.GetComponent<Exchanger>().AddToExchanger(this);
        }
    }

    public void PullOutExchanger()
    {
        if (originParent && originIndex != -1)
        {
            exchanger.GetComponent<Exchanger>().DiscardToExchanger(transform.GetSiblingIndex());
            transform.SetParent(originParent.transform);
            transform.SetSiblingIndex(originIndex);

            originParent = null;
            originIndex = -1;
        }
    }
    

    public void Mounting()
    {
        inventory.GetComponent<Inventory>().DiscardToInventory(transform.GetSiblingIndex());
        inspector.GetComponent<Inspector>().AddToInspector(this);
    }

    public void DisMounting()
    {
        inspector.GetComponent<Inspector>().DiscardToInspector(slotItem.GetComponent<ItemStatus>().MountingPart);
        inventory.GetComponent<Inventory>().AddToInventory(this);
    }


    public void InsertImage(GameObject item)
    {
        slotItem = item;
        switch(slotItem.GetComponent<ItemStatus>().ItemGrade)
        {
            case 0:
                // 노멀 하양
                transform.Find("Background").GetComponent<Image>().color = new Color(255/255f, 255/255f, 255/255f, 0.5f);
                break;
            case 1:
                // 레어 하늘
                transform.Find("Background").GetComponent<Image>().color = new Color(23 / 255f, 224 / 255f, 224 / 255f, 0.5f);
                break;
            case 2:
                // 에픽 보라
                transform.Find("Background").GetComponent<Image>().color = new Color(159 / 255f, 14 / 255f, 224 / 255f, 0.5f);
                break;
            case 3:
                // 유니크 노랑
                transform.Find("Background").GetComponent<Image>().color = new Color(233 / 255f, 241 / 255f, 35 / 255f, 0.5f);
                break;
            case 4:
                // 레전더리 초록
                transform.Find("Background").GetComponent<Image>().color = new Color(96 / 255f, 236 / 255f, 39 / 255f, 0.5f);
                break;
        }
        transform.Find("Background").transform.Find("Item").GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").transform.Find("Background").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").transform.Find("Background").gameObject;
        if (!exchanger) exchanger = GameObject.Find("Canvas").transform.Find("Exchanger").transform.Find("Background").gameObject;
        CompareMountItem();
    }
}
