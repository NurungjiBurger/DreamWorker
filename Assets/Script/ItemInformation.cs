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

    private void CompareToInspector(string str, GameObject arrow)
    {
        Color blue = new Color(0 / 255f, 0 / 255f, 255 / 255f);
        Color red = new Color(255 / 255f, 0 / 255f, 0 / 255f);

        Quaternion up = new Quaternion(0, 0, 0, 0);
        Quaternion down = new Quaternion(0, 0, 180, 0);

        bool exist = false;

        if (inspector.GetComponent<Inspector>().EquipItemList.Count != 0)
        {
            ItemStatus now = item.GetComponent<ItemStatus>();
            ItemStatus compare = null;

            for (int i=0;i<inspector.GetComponent<Inspector>().EquipItemList.Count;i++)
            {
                if (inspector.GetComponent<Inspector>().EquipItemList[i].GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().MountingPart == now.MountingPart)
                {
                    compare = inspector.GetComponent<Inspector>().EquipItemList[i].GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>();
                    exist = true;
                }
            }

            if (exist)
            {
                switch (str)
                {
                    case "AttackSpeed":
                        if (compare.AttackSpeed > now.AttackSpeed)
                        {
                            arrow.GetComponent<Image>().color = blue;
                            arrow.GetComponent<RectTransform>().rotation = down;
                            arrow.SetActive(true);
                        }
                        else if (compare.AttackSpeed < now.AttackSpeed)
                        {
                            arrow.GetComponent<Image>().color = red;
                            arrow.GetComponent<RectTransform>().rotation = up;
                            arrow.SetActive(true);
                        }
                        else arrow.SetActive(false);
                        break;
                    case "CursedRate":
                        if (compare.CursedRate > now.CursedRate)
                        {
                            arrow.GetComponent<Image>().color = blue;
                            arrow.GetComponent<RectTransform>().rotation = down;
                            arrow.SetActive(true);
                        }
                        else if (compare.CursedRate < now.CursedRate)
                        {
                            arrow.GetComponent<Image>().color = red;
                            arrow.GetComponent<RectTransform>().rotation = up;
                            arrow.SetActive(true);
                        }
                        else arrow.SetActive(false);
                        break;
                    case "DefenseCapability":
                        if (compare.Defense > now.Defense)
                        {
                            arrow.GetComponent<Image>().color = blue;
                            arrow.GetComponent<RectTransform>().rotation = down;
                            arrow.SetActive(true);
                        }
                        else if (compare.Defense < now.Defense)
                        {
                            arrow.GetComponent<Image>().color = red;
                            arrow.GetComponent<RectTransform>().rotation = up;
                            arrow.SetActive(true);
                        }
                        else arrow.SetActive(false);
                        break;
                    case "Grade":
                        if (compare.ItemGrade > now.ItemGrade)
                        {
                            arrow.GetComponent<Image>().color = blue;
                            arrow.GetComponent<RectTransform>().rotation = down;
                            arrow.SetActive(true);
                        }
                        else if (compare.ItemGrade < now.ItemGrade)
                        {
                            arrow.GetComponent<Image>().color = red;
                            arrow.GetComponent<RectTransform>().rotation = up;
                            arrow.SetActive(true);
                        }
                        else arrow.SetActive(false);
                        break;
                    case "HP":
                        if (compare.MaxHP > now.MaxHP)
                        {
                            arrow.GetComponent<Image>().color = blue;
                            arrow.GetComponent<RectTransform>().rotation = down;
                            arrow.SetActive(true);
                        }
                        else if (compare.MaxHP < now.MaxHP)
                        {
                            arrow.GetComponent<Image>().color = red;
                            arrow.GetComponent<RectTransform>().rotation = up;
                            arrow.SetActive(true);
                        }
                        else arrow.SetActive(false);
                        break;
                    case "Jump":
                        if (compare.JumpPower > now.JumpPower)
                        {
                            arrow.GetComponent<Image>().color = blue;
                            arrow.GetComponent<RectTransform>().rotation = down;
                            arrow.SetActive(true);
                        }
                        else if (compare.JumpPower < now.JumpPower)
                        {
                            arrow.GetComponent<Image>().color = red;
                            arrow.GetComponent<RectTransform>().rotation = up;
                            arrow.SetActive(true);
                        }
                        else arrow.SetActive(false);
                        break;
                    case "MoveSpeed":
                        if (compare.MoveSpeed > now.MoveSpeed)
                        {
                            arrow.GetComponent<Image>().color = blue;
                            arrow.GetComponent<RectTransform>().rotation = down;
                            arrow.SetActive(true);
                        }
                        else if (compare.MoveSpeed < now.MoveSpeed)
                        {
                            arrow.GetComponent<Image>().color = red;
                            arrow.GetComponent<RectTransform>().rotation = up;
                            arrow.SetActive(true);
                        }
                        else arrow.SetActive(false);
                        break;
                    case "Power":
                        if (compare.Power > now.Power)
                        {
                            arrow.GetComponent<Image>().color = blue;
                            arrow.GetComponent<RectTransform>().rotation = down;
                            arrow.SetActive(true);
                        }
                        else if (compare.Power < now.Power)
                        {
                            arrow.GetComponent<Image>().color = red;
                            arrow.GetComponent<RectTransform>().rotation = up;
                            arrow.SetActive(true);
                        }
                        else arrow.SetActive(false);
                        break;
                    default:
                        break;
                }
            }
        }
        else 
        
        if (!exist)
        {
            arrow.GetComponent<Image>().color = red;
            arrow.GetComponent<RectTransform>().rotation = up;
            arrow.SetActive(true);
        }
    }

    private void UpdateItemInformation()
    {
        // 이름 , 이미지
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Name;
        transform.GetChild(1).GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
        // 정보 패널
        transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().MountingPart;
        transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Occupation;
        // 스탯 패널
        transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().AttackSpeed.ToString();
        CompareToInspector("AttackSpeed", transform.GetChild(3).GetChild(0).GetChild(0).gameObject);
        transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().CursedRate.ToString();
        CompareToInspector("CursedRate", transform.GetChild(3).GetChild(1).GetChild(0).gameObject);
        transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Defense.ToString();
        CompareToInspector("DefenseCapability", transform.GetChild(3).GetChild(2).GetChild(0).gameObject);
        transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().ItemGrade.ToString();
        CompareToInspector("Grade", transform.GetChild(3).GetChild(3).GetChild(0).gameObject);
        transform.GetChild(3).GetChild(4).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().MaxHP.ToString();
        CompareToInspector("HP", transform.GetChild(3).GetChild(4).GetChild(0).gameObject);
        transform.GetChild(3).GetChild(5).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().JumpPower.ToString();
        CompareToInspector("Jump", transform.GetChild(3).GetChild(5).GetChild(0).gameObject);
        transform.GetChild(3).GetChild(6).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().MoveSpeed.ToString();
        CompareToInspector("MoveSpeed", transform.GetChild(3).GetChild(6).GetChild(0).gameObject);
        transform.GetChild(3).GetChild(7).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Power.ToString();
        CompareToInspector("Power", transform.GetChild(3).GetChild(7).GetChild(0).gameObject);
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
