using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inspector : MonoBehaviour
{
    private GameObject player;

    private GameData data;

    private List<Slot> equipItemList = new List<Slot>();

    public int ItemCount { get { return equipItemList.Count; } }
    public List<Slot> EquipItemList { get { return equipItemList; } }

    public Slot FindInInspector(Slot slot)
    {
        int size = equipItemList.Count;
        for (int i = 0; i < size; i++)
        {
            if (equipItemList[i].SlotItem.GetComponent<ItemStatus>().MountingPart == slot.SlotItem.GetComponent<ItemStatus>().MountingPart)
            {
                return equipItemList[i];
            }
        }
        return slot;
    }

    public void DiscardToInspector(Slot slot)
    {
        int size = equipItemList.Count;
        for(int i = 0;i < size;i++)
        {
            if (equipItemList[i].SlotItem.GetComponent<ItemStatus>().MountingPart == slot.SlotItem.GetComponent<ItemStatus>().MountingPart)
            {
                player.GetComponent<PlayerStatus>().CalCulateStat(equipItemList[i].SlotItem, -1);
                equipItemList.Remove(equipItemList[i]);
                break;
            }
        }
    }

    private void ChangeItemPart(Slot slot)
    {
        int size = equipItemList.Count;
        for (int i = 0; i < size; i++)
        {
            if (equipItemList[i].SlotItem.GetComponent<ItemStatus>().MountingPart == slot.SlotItem.GetComponent<ItemStatus>().MountingPart)
            {
                equipItemList[i].GetComponent<Slot>().DisMounting();
                break;
            }
        }

    }

    public void AddToInspector(Slot slot)
    {
        ChangeItemPart(slot);

        equipItemList.Add(slot);

        slot.transform.SetParent(transform.Find("Background").transform);
        slot.transform.position = transform.Find("Background").transform.Find(slot.SlotItem.GetComponent<ItemStatus>().MountingPart).position;
        slot.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        slot.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        slot.SlotItem.GetComponent<ItemStatus>().IsMount = true;
        player.GetComponent<PlayerStatus>().CalCulateStat(slot.SlotItem, 1);
    }

    public void StatusText()
    {
        transform.Find("Background").transform.Find("Infomation").transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level  " + player.GetComponent<PlayerStatus>().Level.ToString();
        transform.Find("Background").transform.Find("Infomation").transform.Find("Occupation").GetComponent<TextMeshProUGUI>().text = player.GetComponent<PlayerStatus>().Occupation;

        transform.Find("Background").transform.Find("Status").transform.Find("MaxHP").GetComponent<TextMeshProUGUI>().text = "�ִ�ü��  " + player.GetComponent<PlayerStatus>().Data.maxHP.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("AttackPower").GetComponent<TextMeshProUGUI>().text = "���ݷ�  " + player.GetComponent<PlayerStatus>().Data.power.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("JumpPower").GetComponent<TextMeshProUGUI>().text = "������  " + player.GetComponent<PlayerStatus>().Data.jumpPower.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("MoveSpeed").GetComponent<TextMeshProUGUI>().text = "�̵��ӵ�  " + player.GetComponent<PlayerStatus>().Data.moveSpeed.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("DefenseCapability").GetComponent<TextMeshProUGUI>().text = "����  " + player.GetComponent<PlayerStatus>().Data.defenseRate.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("AttackSpeed").GetComponent<TextMeshProUGUI>().text = "���ݼӵ�  " + player.GetComponent<PlayerStatus>().Data.attackSpeed.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("BloodAbsorptionRate").GetComponent<TextMeshProUGUI>().text = "����  " + player.GetComponent<PlayerStatus>().Data.bloodAbsorptionRate.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("EvasionRate").GetComponent<TextMeshProUGUI>().text = "ȸ��  " + player.GetComponent<PlayerStatus>().Data.evasionRate.ToString();
    }

    void Start()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;
    }

    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        else StatusText();
    }
}
