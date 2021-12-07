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

    private float doubleClickSecond = 0.25f;
    private double timer = 0;

    private GameObject itemPanel;
    private GameObject ui;
    private GameObject slotItem;
    private GameObject player;
    private GameObject inventory;
    private GameObject inspector;
    private GameObject itemInform;
    private GameObject selecter;
    private GameObject smithy;   

    private Sprite itemImage;

    public GameObject SlotItem { get { return slotItem; } }

    // ������ ����
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

    // ������ ���� �г� Ȱ��ȭ
    public void ActiveItemInform()
    {
        itemInform.GetComponent<ItemInformation>().InputInformation(gameObject);
        hovering = !hovering;
        itemInform.SetActive(hovering);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ����Ŭ�� ����
        if (isOneClick && ((Time.time - timer) > doubleClickSecond))
        {
            isOneClick = false;
        }

        // ��Ŭ��
        if (!isOneClick)
        {
            timer = Time.time;
            isOneClick = true;
        }
        // ����Ŭ��
        else if (isOneClick && (Time.time - timer) < doubleClickSecond)
        {
            isOneClick = false;
            // �г� ��
            itemPanel.SetActive(!itemPanel.activeSelf);
            itemPanel.GetComponent<ItemPanel>().AllocateSlot(this);
        }
    }

    public void DestroyObject()
    {
        if (transform.parent.parent == inventory.transform)
        {
            // �κ����� ����
            inventory.GetComponent<Inventory>().DiscardToInventory(transform.GetSiblingIndex());
            // ������ ������ ����
            slotItem.GetComponent<ItemStatus>().DestoryAll();
        }
        Destroy(gameObject);
    }

    
    // ������ �����۰� ��
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

    // ���Կ� �̹��� �� ���� ä���
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
                // �Ͼ�
                transform.GetChild(0).Find("Cursedrate").GetComponent<Image>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0.5f);
                break;
            case 1:
                // ���
                transform.GetChild(0).Find("Cursedrate").GetComponent<Image>().color = new Color(255 / 255f, 255 / 255f, 0 / 255f, 0.5f);
                break;
            case 2:
                // ����
                transform.GetChild(0).Find("Cursedrate").GetComponent<Image>().color = new Color(255 / 255f, 0 / 255f, 0 / 255f, 0.5f);
                break;
            case 3:
            case 4:
                // ����
                transform.GetChild(0).Find("Cursedrate").GetComponent<Image>().color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 0.5f);
                break;
            default:
                break;
        }

        // ���� ũ�⿡ ���߾� ������ �̹����� Ȯ�� ���
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
        if (!smithy) smithy = GameObject.Find("Canvas").transform.Find("Smithy").gameObject;

        if (slotItem != null)
        {
            if (transform.parent == smithy.transform.GetChild(0).transform)
            {
                Debug.Log("�� �����°ž�?");
                transform.GetChild(0).Find("PriceBackground").Find("Price").GetComponent<TextMeshProUGUI>().text = slotItem.GetComponent<ItemStatus>().Price.ToString();
            }
        }
    }
}