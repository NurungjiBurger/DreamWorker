using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inspector : MonoBehaviour
{
    private GameObject player;

    private GameData data;

    public int ItemCount { get { return data.playerEquipItemList.Count; } }
    public List<Slot> EquipItemList { get { return data.playerEquipItemList; } }

    public Slot FindInInspector(Slot slot)
    {
        int size = data.playerEquipItemList.Count;
        for (int i = 0; i < size; i++)
        {
            if (data.playerEquipItemList[i].SlotItem.GetComponent<ItemStatus>().MountingPart == slot.SlotItem.GetComponent<ItemStatus>().MountingPart)
            {
                return data.playerEquipItemList[i];
            }
        }
        return slot;
    }

    public void DiscardToInspector(Slot slot)
    {
        int size = data.playerEquipItemList.Count;
        for(int i = 0;i < size;i++)
        {
            if (data.playerEquipItemList[i].SlotItem.GetComponent<ItemStatus>().MountingPart == slot.SlotItem.GetComponent<ItemStatus>().MountingPart)
            {
                player.GetComponent<PlayerStatus>().CalCulateStat(data.playerEquipItemList[i].SlotItem, -1);
                data.playerEquipItemList.Remove(data.playerEquipItemList[i]);
                break;
            }
        }
    }

    private void ChangeItemPart(Slot slot)
    {
        int size = data.playerEquipItemList.Count;
        for (int i = 0; i < size; i++)
        {
            if (data.playerEquipItemList[i].SlotItem.GetComponent<ItemStatus>().MountingPart == slot.SlotItem.GetComponent<ItemStatus>().MountingPart)
            {
                data.playerEquipItemList[i].GetComponent<Slot>().DisMounting();
                break;
            }
        }

    }

    public void AddToInspector(Slot slot)
    {
        ChangeItemPart(slot);

        data.playerEquipItemList.Add(slot);

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

        transform.Find("Background").transform.Find("Status").transform.Find("MaxHP").GetComponent<TextMeshProUGUI>().text = "최대체력  " + player.GetComponent<PlayerStatus>().Status.maxHP.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("AttackPower").GetComponent<TextMeshProUGUI>().text = "공격력  " + player.GetComponent<PlayerStatus>().Status.power.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("JumpPower").GetComponent<TextMeshProUGUI>().text = "점프력  " + player.GetComponent<PlayerStatus>().Status.jumpPower.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("MoveSpeed").GetComponent<TextMeshProUGUI>().text = "이동속도  " + player.GetComponent<PlayerStatus>().Status.moveSpeed.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("DefenseCapability").GetComponent<TextMeshProUGUI>().text = "방어력  " + player.GetComponent<PlayerStatus>().Status.defenseRate.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("AttackSpeed").GetComponent<TextMeshProUGUI>().text = "공격속도  " + player.GetComponent<PlayerStatus>().Status.attackSpeed.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("BloodAbsorptionRate").GetComponent<TextMeshProUGUI>().text = "흡혈  " + player.GetComponent<PlayerStatus>().Status.bloodAbsorptionRate.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("EvasionRate").GetComponent<TextMeshProUGUI>().text = "회피  " + player.GetComponent<PlayerStatus>().Status.evasionRate.ToString();
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
