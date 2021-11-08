using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemInformation : MonoBehaviour, IPointerClickHandler
{
    private GameObject item;
    private Slot slot;
    private GameObject inspector;

    public void ModifyPosition(int direction, Vector3 position)
    {
        // 36 슬롯은 마우스의 오른쪽 가운데에 정보창
        // 37 슬롯은 마우스의 왼쪽 가운데에 정보창

        // 0, 1, 2, 5, 6, 7 슬롯은 마우스의 오른쪽 아래에 정보창
        // 3, 4, 8, 9 슬롯은 마우스의 왼쪽 아래에 정보창
        // 10, 11, 12, 15, 16, 17 슬롯은 마우스의 오른쪽 위에 정보창
        // 13, 14, 18, 19 슬롯은 마우스의 왼쪽 위에 정보창

        switch (direction)
        {
            case 0:
            case 1:
            case 2:
            case 5:
            case 6:
            case 7:
                transform.position = new Vector3(position.x + 55, position.y - 50, position.z);
                break;
            case 3:
            case 4:
            case 8:
            case 9:
                transform.position = new Vector3(position.x - 55, position.y - 50, position.z);
                break;
            case 10:
            case 11:
            case 12:
            case 15:
            case 16:
            case 17:
                transform.position = new Vector3(position.x + 55, position.y + 50, position.z);
                break;
            case 13:
            case 14:
            case 18:
            case 19:
                transform.position = new Vector3(position.x - 55, position.y + 50, position.z);
                break;
            case 36:
                transform.position = new Vector3(position.x + 55, position.y, position.z);
                break;
            case 37:
                transform.position = new Vector3(position.x - 55, position.y, position.z);
                break;
            default:
                break;
        }
    }

    private void CompareToInspector(string str, GameObject delta)
    {
        Color blue = new Color(0 / 255f, 150 / 255f, 255 / 255f);
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
            case "Data.attackSpeed":
                if (exist)
                {
                    if (compare.Data.attackSpeed > now.Data.attackSpeed)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Data.attackSpeed < now.Data.attackSpeed)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.attackSpeed - compare.Data.attackSpeed < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.Data.attackSpeed - compare.Data.attackSpeed)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Data.attackSpeed - compare.Data.attackSpeed).ToString();
                }
                else
                {
                    if (now.Data.attackSpeed > 1)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Data.attackSpeed < 1)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.attackSpeed < 1) delta.GetComponent<TextMeshProUGUI>().text += (1 - now.Data.attackSpeed).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Data.attackSpeed - 1).ToString();
                }
                break;
            case "CursedRate":
                if (exist)
                {
                    if (compare.CursedRate > now.CursedRate)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.CursedRate < now.CursedRate)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.CursedRate - compare.CursedRate < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.CursedRate - compare.CursedRate)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.CursedRate - compare.CursedRate).ToString();
                }
                else
                {
                    if (now.CursedRate > 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.CursedRate < 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.CursedRate < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * now.CursedRate).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += now.CursedRate.ToString();
                }
                break;
            case "Data.defenseRateCapability":
                if (exist)
                {
                    if (compare.Data.defenseRate > now.Data.defenseRate)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Data.defenseRate < now.Data.defenseRate)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.defenseRate - compare.Data.defenseRate < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.Data.defenseRate - compare.Data.defenseRate)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Data.defenseRate - compare.Data.defenseRate).ToString();
                }
                else
                {
                    if (now.Data.defenseRate > 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Data.defenseRate < 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.defenseRate < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * now.Data.defenseRate).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += now.Data.defenseRate.ToString();
                }
                break;
            case "Grade":
                if (exist)
                {
                    if (compare.ItemGrade > now.ItemGrade)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.ItemGrade < now.ItemGrade)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.ItemGrade - compare.ItemGrade < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.ItemGrade - compare.ItemGrade)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.ItemGrade - compare.ItemGrade).ToString();
                }
                else
                {
                    if (now.ItemGrade > 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.ItemGrade < 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.ItemGrade < 0) delta.GetComponent<TextMeshProUGUI>().text += ( -1 * now.ItemGrade).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += now.ItemGrade.ToString();
                }
                break;
            case "HP":
                if (exist)
                {
                    if (compare.Data.maxHP > now.Data.maxHP)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Data.maxHP < now.Data.maxHP)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.maxHP - compare.Data.maxHP < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.Data.maxHP - compare.Data.maxHP)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Data.maxHP - compare.Data.maxHP).ToString();
                }
                else
                {
                    if (now.Data.maxHP > 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Data.maxHP < 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.maxHP < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * now.Data.maxHP).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += now.Data.maxHP.ToString();
                }
                break;
            case "Jump":
                if (exist)
                {
                    if (compare.Data.jumpPower > now.Data.jumpPower)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Data.jumpPower < now.Data.jumpPower)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.jumpPower - compare.Data.jumpPower < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.Data.jumpPower - compare.Data.jumpPower)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Data.jumpPower - compare.Data.jumpPower).ToString();
                }
                else
                {
                    if (now.Data.jumpPower > 1)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Data.jumpPower < 1)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.jumpPower < 1) delta.GetComponent<TextMeshProUGUI>().text += (1 - now.Data.jumpPower).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Data.jumpPower - 1 ).ToString();
                }
                break;
            case "Data.moveSpeed":
                if (exist)
                {
                    if (compare.Data.moveSpeed > now.Data.moveSpeed)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Data.moveSpeed < now.Data.moveSpeed)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.moveSpeed - compare.Data.moveSpeed < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.Data.moveSpeed - compare.Data.moveSpeed)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Data.moveSpeed - compare.Data.moveSpeed).ToString();
                }
                else
                {
                    if (now.Data.moveSpeed > 1)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Data.moveSpeed < 1)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.moveSpeed < 1) delta.GetComponent<TextMeshProUGUI>().text += (1 - now.Data.moveSpeed).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Data.moveSpeed - 1).ToString();
                }
                break;
            case "Data.power":
                if (exist)
                {
                    if (compare.Data.power > now.Data.power)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Data.power < now.Data.power)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.power - compare.Data.power < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.Data.power - compare.Data.power)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Data.power - compare.Data.power).ToString();
                }
                else
                {
                    if (now.Data.power > 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Data.power < 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.power < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * now.Data.power).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += now.Data.power.ToString();
                }
                break;
            default:
                break;
        }


    }

    public void OnPointerClick(PointerEventData eventData)
    {
        slot.GetComponent<Slot>().ActiveItemInform();
    }

    private void UpdateItemInformation()
    {
        string grade;
        // 이름 , 이미지
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Name + "  (+" + item.GetComponent<ItemStatus>().Data.enhancingLevel + ")";

        float ratio = 0;

        Sprite img = item.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

        if (img.bounds.size.x >= img.bounds.size.y)
        {
            ratio = 130 / img.bounds.size.x;
        }
        else
        {
            ratio = 130 / img.bounds.size.y;
        }

        transform.GetChild(1).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(img.bounds.size.x * ratio, img.bounds.size.y * ratio);

        transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = item.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

        // 정보 패널
        transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().MountingPart;
        transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Occupation;
        transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Description;
        // 스탯 패널
        transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "공격속도  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().Data.attackSpeed);
        CompareToInspector("Data.attackSpeed", transform.GetChild(3).GetChild(0).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = "저주율  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().CursedRate);
        CompareToInspector("CursedRate", transform.GetChild(3).GetChild(1).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = "방어력  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().Data.defenseRate);
        CompareToInspector("Data.defenseRateCapability", transform.GetChild(3).GetChild(2).Find("Delta").gameObject);
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
        transform.GetChild(3).GetChild(4).GetComponent<TextMeshProUGUI>().text = "체력  " + item.GetComponent<ItemStatus>().Data.maxHP.ToString();
        CompareToInspector("HP", transform.GetChild(3).GetChild(4).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(5).GetComponent<TextMeshProUGUI>().text = "점프력  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().Data.jumpPower);
        CompareToInspector("Jump", transform.GetChild(3).GetChild(5).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(6).GetComponent<TextMeshProUGUI>().text = "이동속도  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().Data.moveSpeed);
        CompareToInspector("Data.moveSpeed", transform.GetChild(3).GetChild(6).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(7).GetComponent<TextMeshProUGUI>().text = "공격력  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().Data.power);
        CompareToInspector("Data.power", transform.GetChild(3).GetChild(7).Find("Delta").gameObject);
    }

    public void InputInformation(GameObject obj)
    {
        slot = obj.GetComponent<Slot>();
        item = slot.SlotItem;
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