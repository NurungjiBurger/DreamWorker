using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inspector : MonoBehaviour
{
    private List<Slot> equipItemList = new List<Slot>();
    private GameObject player;

    public void DiscardToInspector(string part)
    {
        for(int i=0;i<equipItemList.Count;i++)
        {
            if (equipItemList[i].SlotItem.GetComponent<ItemStatus>().MountingPart == part)
            {
                equipItemList.Remove(equipItemList[i]);
                player.GetComponent<PlayerStatus>().CalCulateStat(equipItemList[i].SlotItem, -1);
                break;
            }
        }
    }

    public void AddToInspector(Slot slot)
    {
        if (equipItemList.Count < 5)
        {
            Debug.Log("오");
            if (transform.Find(slot.SlotItem.GetComponent<ItemStatus>().MountingPart).childCount == 1) DiscardToInspector(slot.SlotItem.GetComponent<ItemStatus>().MountingPart);
            Debug.Log("여");
            equipItemList.Add(slot);

            slot.transform.SetParent(transform.Find(slot.SlotItem.GetComponent<ItemStatus>().MountingPart).transform);
            slot.transform.position = slot.transform.parent.position;
            slot.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            slot.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            player.GetComponent<PlayerStatus>().CalCulateStat(slot.SlotItem, 1);
        }
        else Debug.Log("장착할수없습니다");
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
