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
    [SerializeField]
    private Sprite[] gradeImage = new Sprite[5];

    private bool hovering = false;
    private bool isOneClick = false;

    private int originIndex = -1;
    private float doubleClickSecond = 0.25f;
    private double timer = 0;

    private GameObject itemPanel;
    private GameObject ui;
    private GameObject slotItem;
    private GameObject player;
    private GameObject inventory;
    private GameObject inspector;
    private GameObject itemInform;
//    private GameObject enhancer;
//    private GameObject enhancerSelecter;
    private GameObject selecter;
    private GameObject smithy;   
    private GameObject originParent = null;

    private Color[] originColor = new Color[3];
    private Sprite itemImage;

    public GameObject SlotItem { get { return slotItem; } }

    // 아이템 선택
    public void ItemSelect()
    {
        smithy.GetComponent<Smithy>().ItemPayment(slotItem);
        itemInform.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    // 아이템 정보 패널 활성화
    public void ActiveItemInform()
    {
        itemInform.GetComponent<ItemInformation>().InputInformation(gameObject);
        hovering = !hovering;
        itemInform.SetActive(hovering);

        // 달려있는 UI 에 따라 패널의 위치 변화
        /*
        if (transform.parent.parent == inventory.transform) itemInform.GetComponent<ItemInformation>().ModifyPosition(transform.GetSiblingIndex(), Input.mousePosition);
        else if (transform.parent.parent == inspector.transform) itemInform.GetComponent<ItemInformation>().ModifyPosition(36, Input.mousePosition);
        else if (transform.parent.parent == smithy.transform) itemInform.GetComponent<ItemInformation>().ModifyPosition(36, Input.mousePosition);
        */
        //else if (transform.parent.parent == enhancerSelecter.transform) itemInform.GetComponent<ItemInformation>().ModifyPosition(36, Input.mousePosition);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 더블클릭 구분
        if (isOneClick && ((Time.time - timer) > doubleClickSecond))
        {
            isOneClick = false;
        }

        // 원클릭
        if (!isOneClick)
        {
            timer = Time.time;
            isOneClick = true;
        }
        // 더블클릭
        else if (isOneClick && (Time.time - timer) < doubleClickSecond)
        {
            isOneClick = false;
            // 패널 온
            itemPanel.SetActive(!itemPanel.activeSelf);
            itemPanel.GetComponent<ItemPanel>().AllocateSlot(this);
        }
    }

    public void DestroyObject()
    {
        if (transform.parent.parent == inventory.transform) inventory.GetComponent<Inventory>().DiscardToInventory(transform.GetSiblingIndex());
        Destroy(gameObject);
    }

    // 선택되었음을 알리는 체크표시 활성화
    public void ShowSelect(bool value)
    {
        transform.GetChild(0).Find("Select").gameObject.SetActive(value);
    }
    
    // 선택되어 투명해진 슬롯 원래 색깔로 복구
    private void SetOriginColor()
    {
        GetComponent<Image>().color = originColor[0];
        transform.Find("Background").GetComponent<Image>().color = originColor[1];
        transform.Find("Background").transform.Find("Item").GetComponent<Image>().color = originColor[2];
    }

    // 선택된 슬롯 투명화
    private void SetTransParentColor()
    {
        GetComponent<Image>().color = new Color(originColor[0].r, originColor[0].g, originColor[0].b, 75f / 255f);
        transform.Find("Background").GetComponent<Image>().color = new Color(originColor[1].r, originColor[1].g, originColor[1].b, 75f / 255f);
        transform.Find("Background").transform.Find("Item").GetComponent<Image>().color = new Color(originColor[2].r, originColor[2].g, originColor[2].b, 75f / 255f);
    }
    
    // 장착한 아이템과 비교
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
    
    // 강화기에 올리기
    public void PutInEnhancer()
    {
        if (!originParent && originIndex == -1)
        {
            originParent = transform.parent.gameObject;
            originIndex = transform.GetSiblingIndex();

            if (transform.parent == inventory.transform.Find("Background").transform) inventory.GetComponent<Inventory>().DiscardToInventory(transform.GetSiblingIndex());
            else inspector.GetComponent<Inspector>().DiscardToInspector(this);

            //enhancer.GetComponent<Enhancer>().AddToEnhancer(this);
        }
    }

    // 강화기에서 빼오기
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

    // 장착
    public void Mounting()
    {
        inventory.GetComponent<Inventory>().DiscardToInventory(transform.GetSiblingIndex());
        inspector.GetComponent<Inspector>().AddToInspector(this);
    }

    // 해제
    public void DisMounting()
    {
        inspector.GetComponent<Inspector>().DiscardToInspector(this);
        inventory.GetComponent<Inventory>().AddToInventory(this);
    }

    // 슬롯에 이미지 및 색깔 채우기
    public void InsertImage(GameObject item)
    {
        slotItem = item;

        transform.GetChild(0).Find("Grade").GetComponent<Image>().sprite = gradeImage[slotItem.GetComponent<ItemStatus>().ItemGrade];

        // 0 ~ 100
        // 0 ~ 24 / 25 ~ 49 / 50 ~ 74 / 75 ~ 100
        // rate / 25
        // 0 / 1 / 2 / 3,4

        switch (slotItem.GetComponent<ItemStatus>().CursedRate / 25)
        {
            case 0:
                // 하양
                transform.GetChild(0).Find("Cursedrate").GetComponent<Image>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0.5f);
                break;
            case 1:
                // 노랑
                transform.GetChild(0).Find("Cursedrate").GetComponent<Image>().color = new Color(255 / 255f, 255 / 255f, 0 / 255f, 0.5f);
                break;
            case 2:
                // 빨강
                transform.GetChild(0).Find("Cursedrate").GetComponent<Image>().color = new Color(255 / 255f, 0 / 255f, 0 / 255f, 0.5f);
                break;
            case 3:
            case 4:
                // 검정
                transform.GetChild(0).Find("Cursedrate").GetComponent<Image>().color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 0.5f);
                break;
            default:
                break;
        }

        // 슬롯 크기에 맞추어 아이템 이미지를 확대 축소
        float ratio = 0;

        Sprite img = item.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

        if (img.bounds.size.x >= img.bounds.size.y)
        {
            ratio = 128 / img.bounds.size.x;
        }
        else
        {
            ratio = 128 / img.bounds.size.y;
        }


        transform.Find("Background").transform.Find("Item").GetComponent<RectTransform>().sizeDelta = new Vector2(img.bounds.size.x * ratio, img.bounds.size.y * ratio);

        transform.Find("Background").transform.Find("Item").GetComponent<Image>().sprite = img;

        originColor[0] = GetComponent<Image>().color;
        originColor[1] = transform.Find("Background").GetComponent<Image>().color;
        originColor[2] = transform.Find("Background").transform.Find("Item").GetComponent<Image>().color;
    }

    void Start()
    {

    }

    void Update()
    {
        if (!itemPanel) itemPanel = GameObject.Find("Canvas").transform.Find("ItemPanel").gameObject;
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (!itemInform) itemInform = GameObject.Find("Canvas").transform.Find("ItemInform").gameObject;
       // if (!enhancer) enhancer = GameObject.Find("Canvas").transform.Find("Enhancer").gameObject;
       // if (!enhancerSelecter) enhancerSelecter = GameObject.Find("Canvas").transform.Find("EnhancerSelecter").gameObject;
        if (!smithy) smithy = GameObject.Find("Canvas").transform.Find("Smithy").gameObject;

        if (slotItem != null)
        {
            if (transform.parent.parent == smithy.transform) transform.GetChild(0).Find("PriceBackground").Find("Price").GetComponent<TextMeshProUGUI>().text = slotItem.GetComponent<ItemStatus>().Price.ToString();

            /*
            if (transform.parent.parent != enhancerSelecter.transform)
            {
                if (slotItem.GetComponent<ItemStatus>().IsMount) SetTransParentColor();
                else SetOriginColor();

                ShowSelect(false);
            }
            */
        }
    }
}