using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject prefabUI;

    private GameObject ui;
    private GameObject slotItem;
    private Color[] originColor = new Color[3];
    private Sprite itemImage;

    private GameObject player;
    private GameObject inventory;
    private GameObject inspector;
    private GameObject itemInform;
    private GameObject enhancer;
    private GameObject enhancerSelecter;
    private GameObject selecter;
    private GameObject smithy;
    private bool hovering = false;
    
    private GameObject originParent = null;
    private int originIndex = -1;

    public GameObject SlotItem { get { return slotItem; } }

    public void ItemSelect()
    {
        smithy.GetComponent<Smithy>().ItemPayment(slotItem);
        itemInform.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInform.GetComponent<ItemInformation>().InputInformation(gameObject);
        hovering = !hovering;
        itemInform.SetActive(hovering);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.parent == selecter.transform.GetChild(0))
        {
            if (eventData.button == PointerEventData.InputButton.Left) selecter.GetComponent<EnhancerSelecter>().SelectItem(this, transform.GetSiblingIndex());
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }


    public void SetOriginColor()
    {
        GetComponent<Image>().color = originColor[0];
        transform.Find("Background").GetComponent<Image>().color = originColor[1];
        transform.Find("Background").transform.Find("Item").GetComponent<Image>().color = originColor[2];
    }

    public void SetTransParentColor()
    {
        GetComponent<Image>().color = new Color(originColor[0].r, originColor[0].g, originColor[0].b, 100f / 255f);
        transform.Find("Background").GetComponent<Image>().color = new Color(originColor[1].r, originColor[1].g, originColor[1].b, 100f / 255f);
        transform.Find("Background").transform.Find("Item").GetComponent<Image>().color = new Color(originColor[2].r, originColor[2].g, originColor[2].b, 100f / 255f);
    }

    private void CompareMountItem()
    {
        if (transform.parent.transform.parent == inventory.transform)
        {
            if (inspector.GetComponent<Inspector>().ItemCount > 0)
            {
                if (inspector.GetComponent<Inspector>().FindInInspector(this).SlotItem.GetComponent<ItemStatus>().ItemGrade < slotItem.GetComponent<ItemStatus>().ItemGrade)
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
    
    public void PutInEnhancer()
    {
        if (!originParent && originIndex == -1)
        {
            originParent = transform.parent.gameObject;
            originIndex = transform.GetSiblingIndex();

            if (transform.parent == inventory.transform.Find("Background").transform) inventory.GetComponent<Inventory>().DiscardToInventory(transform.GetSiblingIndex());
            else inspector.GetComponent<Inspector>().DiscardToInspector(this);

            enhancer.GetComponent<Enhancer>().AddToEnhancer(this);
        }
    }

    public void PullOutEnhancer()
    {
        if (originParent && originIndex != -1)
        {
            if (originParent.transform.parent == inspector.transform) inspector.GetComponent<Inspector>().AddToInspector(this);
            else if (originParent.transform.parent == inventory.transform) inventory.GetComponent<Inventory>().AddToInventory(this);

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
        inspector.GetComponent<Inspector>().DiscardToInspector(this);
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
        transform.Find("Background").transform.Find("Item").GetComponent<Image>().sprite = item.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

        originColor[0] = GetComponent<Image>().color;
        originColor[1] = transform.Find("Background").GetComponent<Image>().color;
        originColor[2] = transform.Find("Background").transform.Find("Item").GetComponent<Image>().color;
    }

    void Start()
    {

    }

    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (!itemInform) itemInform = GameObject.Find("Canvas").transform.Find("ItemInform").gameObject;
        if (!enhancer) enhancer = GameObject.Find("Canvas").transform.Find("Enhancer").gameObject;
        if (!enhancerSelecter) enhancerSelecter = GameObject.Find("Canvas").transform.Find("EnhancerSelecter").gameObject;
        if (!selecter) selecter = GameObject.Find("Canvas").transform.Find("EnhancerSelecter").gameObject;
        if (!smithy) smithy = GameObject.Find("Canvas").transform.Find("Smithy").gameObject;

        if (itemInform.activeSelf && hovering)
        {
            if (transform.parent.parent == inventory.transform) itemInform.GetComponent<ItemInformation>().ModifyPosition(transform.GetSiblingIndex(), Input.mousePosition);
            else if (transform.parent.parent == inspector.transform) itemInform.GetComponent<ItemInformation>().ModifyPosition(36, Input.mousePosition); 
            else if (transform.parent.parent == smithy.transform) itemInform.GetComponent<ItemInformation>().ModifyPosition(36, Input.mousePosition); 
            else if (transform.parent.parent == enhancerSelecter.transform) itemInform.GetComponent<ItemInformation>().ModifyPosition(36, Input.mousePosition); 
           // else itemInform.SetActive(false);
        }

        if (slotItem != null)
        {
            if (transform.parent.parent == smithy.transform) transform.GetChild(0).Find("PriceBackground").Find("Price").GetComponent<TextMeshProUGUI>().text = slotItem.GetComponent<ItemStatus>().Price.ToString();

            if (slotItem.GetComponent<ItemStatus>().Data != null) transform.GetChild(0).Find("Cursedrate").GetComponent<Image>().fillAmount = (float)slotItem.GetComponent<ItemStatus>().CursedRate / (float)100;
        }
    }
}