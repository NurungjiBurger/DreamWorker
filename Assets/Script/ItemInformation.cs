using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemInformation : MonoBehaviour
{
    private GameObject item;
    private GameObject inspector;

    public void ModifyPosition(int direction, Vector3 position)
    {
        // 16 슬롯은 마우스의 오른쪽 가운데에 정보창

        // 0, 1, 4, 5 슬롯은 마우스의 오른쪽 아래에 정보창
        // 2, 3, 6, 7 슬롯은 마우스의 왼쪽 아래에 정보창
        // 8, 9, 12, 13 슬롯은 마우스의 오른쪽 위에 정보창
        // 10, 11, 14, 15 슬롯은 마우스의 왼쪽 위에 정보창

        switch (direction)
        {
            case 0:
            case 1:
            case 4:
            case 5:
                transform.position = new Vector3(position.x + 55, position.y - 50, position.z);
                break;
            case 2:
            case 3:
            case 6:
            case 7:
                transform.position = new Vector3(position.x - 55, position.y - 50, position.z);
                break;
            case 8:
            case 9:
            case 12:
            case 13:
                transform.position = new Vector3(position.x + 55, position.y + 50, position.z);
                break;
            case 10:
            case 11:
            case 14:
            case 15:
                transform.position = new Vector3(position.x - 55, position.y + 50, position.z);
                break;
            case 16:
                transform.position = new Vector3(position.x + 55, position.y, position.z);
                break;
            default:
                break;
        }
    }

    private void CompareToInspector(string str, GameObject delta)
    {
        Color blue = new Color(0 / 255f, 0 / 255f, 255 / 255f);
        Color red = new Color(255 / 255f, 0 / 255f, 0 / 255f);
        Color green = new Color(0 / 255f, 255 / 255f, 0 / 255f);

        Quaternion up = new Quaternion(0, 0, 0, 0);
        Quaternion down = new Quaternion(0, 0, 180, 0);

        bool exist = false;

        ItemStatus now = item.GetComponent<ItemStatus>();
        ItemStatus compare = null;

        for (int i = 0; i < inspector.GetComponent<Inspector>().EquipItemList.Count; i++)
        {
            if (inspector.GetComponent<Inspector>().EquipItemList[i].GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().MountingPart == now.MountingPart)
            {
                compare = inspector.GetComponent<Inspector>().EquipItemList[i].GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>();
                exist = true;
            }
        }


        switch (str)
        {
            case "AttackSpeed":
                if (exist)
                {
                    if (compare.AttackSpeed > now.AttackSpeed) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else if (compare.AttackSpeed < now.AttackSpeed) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.AttackSpeed - compare.AttackSpeed < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * (now.AttackSpeed - compare.AttackSpeed)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = (now.AttackSpeed - compare.AttackSpeed).ToString();
                }
                else
                {
                    if (now.AttackSpeed > 1) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else if (now.AttackSpeed < 1) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.AttackSpeed < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * now.AttackSpeed).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = now.AttackSpeed.ToString();
                }
                break;
            case "CursedRate":
                if (exist)
                {
                    if (compare.CursedRate > now.CursedRate) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else if (compare.CursedRate < now.CursedRate) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.CursedRate - compare.CursedRate < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * (now.CursedRate - compare.CursedRate)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = (now.CursedRate - compare.CursedRate).ToString();
                }
                else
                {
                    if (now.CursedRate > 0) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else if (now.CursedRate < 0) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.CursedRate < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * now.CursedRate).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = now.CursedRate.ToString();
                }
                break;
            case "DefenseCapability":
                if (exist)
                {
                    if (compare.Defense > now.Defense) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else if (compare.Defense < now.Defense) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.Defense - compare.Defense < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * (now.Defense - compare.Defense)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = (now.Defense - compare.Defense).ToString();
                }
                else
                {
                    if (now.Defense > 0) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else if (now.Defense < 0) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.Defense < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * now.Defense).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = now.Defense.ToString();
                }
                break;
            case "Grade":
                if (exist)
                {
                    if (compare.ItemGrade > now.ItemGrade) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else if (compare.ItemGrade < now.ItemGrade) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.ItemGrade - compare.ItemGrade < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * (now.ItemGrade - compare.ItemGrade)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = (now.ItemGrade - compare.ItemGrade).ToString();
                }
                else
                {
                    if (now.ItemGrade > 0) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else if (now.ItemGrade < 0) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.ItemGrade < 0) delta.GetComponent<TextMeshProUGUI>().text = now.ItemGrade.ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = now.ItemGrade.ToString();
                }
                break;
            case "HP":
                if (exist)
                {
                    if (compare.MaxHP > now.MaxHP) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else if (compare.MaxHP < now.MaxHP) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.MaxHP - compare.MaxHP < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * (now.MaxHP - compare.MaxHP)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = (now.MaxHP - compare.MaxHP).ToString();
                }
                else
                {
                    if (now.MaxHP > 0) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else if (now.MaxHP < 0) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.MaxHP < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * now.MaxHP).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = now.MaxHP.ToString();
                }
                break;
            case "Jump":
                if (exist)
                {
                    if (compare.JumpPower > now.JumpPower) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else if (compare.JumpPower < now.JumpPower) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.JumpPower - compare.JumpPower < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * (now.JumpPower - compare.JumpPower)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = (now.JumpPower - compare.JumpPower).ToString();
                }
                else
                {
                    if (now.JumpPower > 1) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else if (now.JumpPower < 1) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.JumpPower < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * now.JumpPower).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = now.JumpPower.ToString();
                }
                break;
            case "MoveSpeed":
                if (exist)
                {
                    if (compare.MoveSpeed > now.MoveSpeed) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else if (compare.MoveSpeed < now.MoveSpeed) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.MoveSpeed - compare.MoveSpeed < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * (now.MoveSpeed - compare.MoveSpeed)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = (now.MoveSpeed - compare.MoveSpeed).ToString();
                }
                else
                {
                    if (now.MoveSpeed > 1) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else if (now.MoveSpeed < 1) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.MoveSpeed < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * now.MoveSpeed).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = now.MoveSpeed.ToString();
                }
                break;
            case "Power":
                if (exist)
                {
                    if (compare.Power > now.Power) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else if (compare.Power < now.Power) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.Power - compare.Power < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * (now.Power - compare.Power)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = (now.Power - compare.Power).ToString();
                }
                else
                {
                    if (now.Power > 0) delta.GetComponent<TextMeshProUGUI>().color = red;
                    else if (now.Power < 0) delta.GetComponent<TextMeshProUGUI>().color = blue;
                    else delta.GetComponent<TextMeshProUGUI>().color = green;

                    if (now.Power < 0) delta.GetComponent<TextMeshProUGUI>().text = (-1 * now.Power).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text = now.Power.ToString();
                }
                break;
            default:
                break;
        }


    }

    private void UpdateItemInformation()
    {
        string grade;
        // 이름 , 이미지
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Name;
        transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
        // 정보 패널
        transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().MountingPart;
        transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Occupation;
        // 스탯 패널
        transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "공격속도  " + item.GetComponent<ItemStatus>().AttackSpeed.ToString();
        CompareToInspector("AttackSpeed", transform.GetChild(3).GetChild(0).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = "저주율  " + item.GetComponent<ItemStatus>().CursedRate.ToString();
        CompareToInspector("CursedRate", transform.GetChild(3).GetChild(1).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = "방어력  " + item.GetComponent<ItemStatus>().Defense.ToString();
        CompareToInspector("DefenseCapability", transform.GetChild(3).GetChild(2).Find("Delta").gameObject);
        switch (item.GetComponent<ItemStatus>().ItemGrade)
        {
            case 0:
                grade = "Normal";
                break;
            case 1:
                grade = "Rare";
                break;
            case 2:
                grade = "Epic";
                break;
            case 3:
                grade = "Unique";
                break;
            case 4:
                grade = "Legendary";
                break;
            default:
                grade = "None";
                break;
        }
        transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>().text = "등급  " + grade;
        CompareToInspector("Grade", transform.GetChild(3).GetChild(3).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(4).GetComponent<TextMeshProUGUI>().text = "체력  " + item.GetComponent<ItemStatus>().MaxHP.ToString();
        CompareToInspector("HP", transform.GetChild(3).GetChild(4).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(5).GetComponent<TextMeshProUGUI>().text = "점프력  " + item.GetComponent<ItemStatus>().JumpPower.ToString();
        CompareToInspector("Jump", transform.GetChild(3).GetChild(5).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(6).GetComponent<TextMeshProUGUI>().text = "이동속도  " + item.GetComponent<ItemStatus>().MoveSpeed.ToString();
        CompareToInspector("MoveSpeed", transform.GetChild(3).GetChild(6).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(7).GetComponent<TextMeshProUGUI>().text = "공격력  " + item.GetComponent<ItemStatus>().Power.ToString();
        CompareToInspector("Power", transform.GetChild(3).GetChild(7).Find("Delta").gameObject);
    }

    public void InputInformation(GameObject obj)
    {
        item = obj;
    }

    private void Start()
    {
        inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
    }

    private void Update()
    {
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (item) UpdateItemInformation();
    }
}