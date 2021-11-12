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

    public void InspectorStatRerange(int oper)
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        for (int idx = 0; idx < EquipItemList.Count; idx++)
        {
            player.GetComponent<PlayerStatus>().CalCulateStat(equipItemList[idx].GetComponent<Slot>().SlotItem, oper);
        }
    }

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

        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0;i < size;i++)
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
        slot.SlotItem.GetComponent<ItemStatus>().IsMount = true;
        if(slot.SlotItem.GetComponent<ItemStatus>().MountingPart == "Weapon") slot.SlotItem.SetActive(true);

        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerStatus>().CalCulateStat(slot.SlotItem, 1);
    }

    public void StatusText()
    {
        transform.Find("Background").transform.Find("Infomation").transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level  " + player.GetComponent<PlayerStatus>().Level.ToString();
        transform.Find("Background").transform.Find("Infomation").transform.Find("Occupation").GetComponent<TextMeshProUGUI>().text = player.GetComponent<PlayerStatus>().Occupation;

        transform.Find("Background").transform.Find("Status").transform.Find("MaxHP").GetComponent<TextMeshProUGUI>().text = "최대체력  " + player.GetComponent<PlayerStatus>().Data.maxHP.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("AttackPower").GetComponent<TextMeshProUGUI>().text = "공격력  " + string.Format("{0:N2}", player.GetComponent<PlayerStatus>().Data.power);
        transform.Find("Background").transform.Find("Status").transform.Find("JumpPower").GetComponent<TextMeshProUGUI>().text = "점프력  " + string.Format("{0:N2}", player.GetComponent<PlayerStatus>().Data.jumpPower);
        transform.Find("Background").transform.Find("Status").transform.Find("MoveSpeed").GetComponent<TextMeshProUGUI>().text = "이동속도  " + string.Format("{0:N2}", player.GetComponent<PlayerStatus>().Data.moveSpeed);
        transform.Find("Background").transform.Find("Status").transform.Find("DefenseCapability").GetComponent<TextMeshProUGUI>().text = "방어력  " + string.Format("{0:N2}", player.GetComponent<PlayerStatus>().Data.defenseRate);
        transform.Find("Background").transform.Find("Status").transform.Find("AttackSpeed").GetComponent<TextMeshProUGUI>().text = "공격속도  " + string.Format("{0:N2}", player.GetComponent<PlayerStatus>().Data.attackSpeed);
        transform.Find("Background").transform.Find("Status").transform.Find("BloodAbsorptionRate").GetComponent<TextMeshProUGUI>().text = "피흡  " + string.Format("{0:N2}", player.GetComponent<PlayerStatus>().Data.bloodAbsorptionRate);
        transform.Find("Background").transform.Find("Status").transform.Find("EvasionRate").GetComponent<TextMeshProUGUI>().text = "회피  " + string.Format("{0:N2}", player.GetComponent<PlayerStatus>().Data.evasionRate);
    }

    void Start()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;
    }

    void Update()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            transform.Find("Background").transform.Find("Character").transform.GetComponent<Image>().sprite = player.GetComponent<PlayerStatus>().PlayerImage;
        }
        else StatusText();


    }
}
