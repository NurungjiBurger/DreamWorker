using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inspector : MonoBehaviour
{
    private GameObject player;

    private List<Slot> equipItemList = new List<Slot>();

    public int ItemCount { get { return equipItemList.Count; } }
    public List<Slot> EquipItemList { get { return equipItemList; } }

    public bool DiscardToInspector(Slot slot)
    {
        // ¹ö·È´Ù¸é true
        if (transform.Find("Background").transform.Find(slot.SlotItem.GetComponent<ItemStatus>().MountingPart))
        {
            player.GetComponent<PlayerStatus>().CalCulateStat(slot.SlotItem, -1);
            int size = equipItemList.Count;
            for(int i=0;i<size;i++)
            {
                if (equipItemList[i].SlotItem.GetComponent<ItemStatus>().MountingPart == slot.SlotItem.GetComponent<ItemStatus>().MountingPart)
                {
                    equipItemList.Remove(equipItemList[i]);
                    break;
                }
            }
            return true;
        }
        // ¾È¹ö·È´Ù¸é false
        else return false;
    }

    public void AddToInspector(Slot slot)
    {
        if (equipItemList.Count < 5)
        {
            DiscardToInspector(slot);

            equipItemList.Add(slot);

            slot.transform.SetParent(transform.Find("Background").transform.Find(slot.SlotItem.GetComponent<ItemStatus>().MountingPart).transform);
            slot.transform.position = slot.transform.parent.position;
            slot.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            slot.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            player.GetComponent<PlayerStatus>().CalCulateStat(slot.SlotItem, 1);
        }
    }

    public void StatusText()
    {
        transform.Find("Background").transform.Find("Infomation").transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level  " + player.GetComponent<PlayerStatus>().Level.ToString();
        transform.Find("Background").transform.Find("Infomation").transform.Find("Occupation").GetComponent<TextMeshProUGUI>().text = player.GetComponent<PlayerStatus>().Occupation;

        transform.Find("Background").transform.Find("Status").transform.Find("MaxHP").GetComponent<TextMeshProUGUI>().text = "MaxHP  " + player.GetComponent<PlayerStatus>().MaxHP.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("AttackPower").GetComponent<TextMeshProUGUI>().text = "Power  " + player.GetComponent<PlayerStatus>().Power.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("JumpPower").GetComponent<TextMeshProUGUI>().text = "JumpPower  " + player.GetComponent<PlayerStatus>().JumpPower.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("MoveSpeed").GetComponent<TextMeshProUGUI>().text = "MoveSpeed  " + player.GetComponent<PlayerStatus>().MoveSpeed.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("DefenseCapability").GetComponent<TextMeshProUGUI>().text = "Defense  " + player.GetComponent<PlayerStatus>().Defense.ToString();
        //transform.Find("Background").transform.Find("Status").transform.Find("BloodAbsorption").GetComponent<TextMeshProUGUI>().text = player.GetComponent<PlayerStatus>().BloodAbsorption.ToString();
        //transform.Find("Background").transform.Find("Status").transform.Find("Evasion").GetComponent<TextMesh>().text = player.GetComponent<PlayerStatus>().Evasion.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        StatusText();
    }
}
