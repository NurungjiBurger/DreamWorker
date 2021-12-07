using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject inventory;
    [SerializeField]
    private GameObject inspector;
    [SerializeField]
    private GameObject enhancer;

    private GameObject player;
    private Slot selectSlot;
    private Slot devoteSlot;

    public void AllocateSlot(Slot slot)
    {
        selectSlot = slot;
    }

    public void Equip()
    {
        // 인벤 -> 장비창
        if (transform.Find("EquipButton").Find("Background").Find("Equip").GetComponent<TextMeshProUGUI>().text == "장착")
        {
            // 선택한 슬롯의 아이템을 장착
            // 장비창에 같은 부위에 장비한 아이템이 있는지 확인
            Slot tmp = inspector.GetComponent<Inspector>().FindInInspector(selectSlot);

            // 없다면 바로 장착
            if (tmp == selectSlot)
            {
                inventory.GetComponent<Inventory>().DiscardToInventory(selectSlot.transform.GetSiblingIndex());
                inspector.GetComponent<Inspector>().AddToInspector(selectSlot);
            }
            // 있다면 해당 아이템을 인벤토리로 옮기고 장착
            else
            {
                inspector.GetComponent<Inspector>().DiscardToInspector(tmp);
                inventory.GetComponent<Inventory>().DiscardToInventory(selectSlot.transform.GetSiblingIndex());

                inspector.GetComponent<Inspector>().AddToInspector(selectSlot);
                inventory.GetComponent<Inventory>().AddToInventory(tmp);
            }
        }
        // 장비창 -> 인벤
        else if (transform.Find("EquipButton").Find("Background").Find("Equip").GetComponent<TextMeshProUGUI>().text == "해제")
        {
            inspector.GetComponent<Inspector>().DiscardToInspector(selectSlot);
            inventory.GetComponent<Inventory>().AddToInventory(selectSlot);
        }

        // 아이템패널 비활
        gameObject.SetActive(false);
    }

    public void Enhance()
    {
        // 강화창 활성화
        transform.GetChild(0).GetComponent<ButtonUI>().UIActive();
        
        // 강화할 아이템, 제물 아이템 자동 등록
        enhancer.GetComponent<Enhancer>().Enroll(selectSlot, true);
        enhancer.GetComponent<Enhancer>().Enroll(devoteSlot, false);
    }

    public void ItemInform()
    {
        // 아이템 정보창 활성화
        selectSlot.GetComponent<Slot>().ActiveItemInform();
        // 아이템 패널 비활
        gameObject.SetActive(false);
    }

    public void ItemDisassembly()
    {
        // 슬롯 오브젝트 제거
        selectSlot.GetComponent<Slot>().DestroyObject();
        // 아이템 조각 획득
        player.GetComponent<PlayerStatus>().Data.itemPiece++;
        // 아이템 패널 비활
        gameObject.SetActive(false);
    }

    void Start()
    {

    }

    private void OnDisable()
    {
        selectSlot = null;
        devoteSlot = null;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (!enhancer) enhancer = GameObject.Find("Canvas").transform.Find("Enhancer").gameObject;
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        // 선택 슬롯이 장비창에 있는 슬롯인 경우
        if (selectSlot.transform.parent == inspector.transform.Find("Background").transform && selectSlot.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel < 6)
        {
            int size = 0;
            size = inventory.GetComponent<Inventory>().ItemCount;

            // 장비창에 있는 아이템을 선택하는 경우에는 장착할수없고 해제만 가능
            transform.Find("EquipButton").Find("Background").Find("Equip").GetComponent<TextMeshProUGUI>().text = "해제";
            // 하지만 인벤에 공간이 없으면 해제할수 없음.
            if (size == 20) transform.Find("EquipButton").gameObject.SetActive(false);
            // 실수로 장착한 아이템 분해 방지
            transform.Find("DisAssemblyButton").gameObject.SetActive(false);

            Debug.Log("size 는 " + size);

            if (selectSlot && devoteSlot == null)
            {

                for (int i = 0; i < size; i++)
                {
                    if (inventory.transform.GetChild(0).GetChild(i).GetComponent<Slot>().SlotItem.name == selectSlot.GetComponent<Slot>().SlotItem.name)
                    {
                        devoteSlot = inventory.transform.GetChild(0).GetChild(i).gameObject.GetComponent<Slot>();
                        transform.Find("EnhanceButton").gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
        // 선택 슬롯이 인벤토리인 경우
        else
        {
            // 인벤토리에서는 해제는 불가능하고 장착만 가능함.
            transform.Find("EquipButton").gameObject.SetActive(true);
            transform.Find("EquipButton").Find("Background").Find("Equip").GetComponent<TextMeshProUGUI>().text = "장착";
            transform.Find("EnhanceButton").gameObject.SetActive(false);
            transform.Find("DisAssemblyButton").gameObject.SetActive(true);
        }

    }
}
