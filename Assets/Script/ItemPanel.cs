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
        // �κ� -> ���â
        if (transform.Find("EquipButton").Find("Background").Find("Equip").GetComponent<TextMeshProUGUI>().text == "����")
        {
            // ������ ������ �������� ����
            // ���â�� ���� ������ ����� �������� �ִ��� Ȯ��
            Slot tmp = inspector.GetComponent<Inspector>().FindInInspector(selectSlot);

            // ���ٸ� �ٷ� ����
            if (tmp == selectSlot)
            {
                inventory.GetComponent<Inventory>().DiscardToInventory(selectSlot.transform.GetSiblingIndex());
                inspector.GetComponent<Inspector>().AddToInspector(selectSlot);
            }
            // �ִٸ� �ش� �������� �κ��丮�� �ű�� ����
            else
            {
                inspector.GetComponent<Inspector>().DiscardToInspector(tmp);
                inventory.GetComponent<Inventory>().DiscardToInventory(selectSlot.transform.GetSiblingIndex());

                inspector.GetComponent<Inspector>().AddToInspector(selectSlot);
                inventory.GetComponent<Inventory>().AddToInventory(tmp);
            }
        }
        // ���â -> �κ�
        else if (transform.Find("EquipButton").Find("Background").Find("Equip").GetComponent<TextMeshProUGUI>().text == "����")
        {
            inspector.GetComponent<Inspector>().DiscardToInspector(selectSlot);
            inventory.GetComponent<Inventory>().AddToInventory(selectSlot);
        }

        // �������г� ��Ȱ
        gameObject.SetActive(false);
    }

    public void Enhance()
    {
        // ��ȭâ Ȱ��ȭ
        transform.GetChild(0).GetComponent<ButtonUI>().UIActive();
        
        // ��ȭ�� ������, ���� ������ �ڵ� ���
        enhancer.GetComponent<Enhancer>().Enroll(selectSlot, true);
        enhancer.GetComponent<Enhancer>().Enroll(devoteSlot, false);
    }

    public void ItemInform()
    {
        // ������ ����â Ȱ��ȭ
        selectSlot.GetComponent<Slot>().ActiveItemInform();
        // ������ �г� ��Ȱ
        gameObject.SetActive(false);
    }

    public void ItemDisassembly()
    {
        // ���� ������Ʈ ����
        selectSlot.GetComponent<Slot>().DestroyObject();
        // ������ ���� ȹ��
        player.GetComponent<PlayerStatus>().Data.itemPiece++;
        // ������ �г� ��Ȱ
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

        // ���� ������ ���â�� �ִ� ������ ���
        if (selectSlot.transform.parent == inspector.transform.Find("Background").transform && selectSlot.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel < 6)
        {
            int size = 0;
            size = inventory.GetComponent<Inventory>().ItemCount;

            // ���â�� �ִ� �������� �����ϴ� ��쿡�� �����Ҽ����� ������ ����
            transform.Find("EquipButton").Find("Background").Find("Equip").GetComponent<TextMeshProUGUI>().text = "����";
            // ������ �κ��� ������ ������ �����Ҽ� ����.
            if (size == 20) transform.Find("EquipButton").gameObject.SetActive(false);
            // �Ǽ��� ������ ������ ���� ����
            transform.Find("DisAssemblyButton").gameObject.SetActive(false);

            Debug.Log("size �� " + size);

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
        // ���� ������ �κ��丮�� ���
        else
        {
            // �κ��丮������ ������ �Ұ����ϰ� ������ ������.
            transform.Find("EquipButton").gameObject.SetActive(true);
            transform.Find("EquipButton").Find("Background").Find("Equip").GetComponent<TextMeshProUGUI>().text = "����";
            transform.Find("EnhanceButton").gameObject.SetActive(false);
            transform.Find("DisAssemblyButton").gameObject.SetActive(true);
        }

    }
}
