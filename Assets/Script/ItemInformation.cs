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
        // 16 ������ ���콺�� ������ ����� ����â

        // 0, 1, 4, 5 ������ ���콺�� ������ �Ʒ��� ����â
        // 2, 3, 6, 7 ������ ���콺�� ���� �Ʒ��� ����â
        // 8, 9, 12, 13 ������ ���콺�� ������ ���� ����â
        // 10, 11, 14, 15 ������ ���콺�� ���� ���� ����â

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
            case "Status.attackSpeed":
                if (exist)
                {
                    if (compare.Status.attackSpeed > now.Status.attackSpeed)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Status.attackSpeed < now.Status.attackSpeed)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Status.attackSpeed - compare.Status.attackSpeed < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.Status.attackSpeed - compare.Status.attackSpeed)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Status.attackSpeed - compare.Status.attackSpeed).ToString();
                }
                else
                {
                    if (now.Status.attackSpeed > 1)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Status.attackSpeed < 1)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Status.attackSpeed < 1) delta.GetComponent<TextMeshProUGUI>().text += (1 - now.Status.attackSpeed).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Status.attackSpeed - 1).ToString();
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
            case "Status.defenseRateCapability":
                if (exist)
                {
                    if (compare.Status.defenseRate > now.Status.defenseRate)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Status.defenseRate < now.Status.defenseRate)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Status.defenseRate - compare.Status.defenseRate < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.Status.defenseRate - compare.Status.defenseRate)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Status.defenseRate - compare.Status.defenseRate).ToString();
                }
                else
                {
                    if (now.Status.defenseRate > 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Status.defenseRate < 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Status.defenseRate < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * now.Status.defenseRate).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += now.Status.defenseRate.ToString();
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
                    if (compare.Status.maxHP > now.Status.maxHP)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Status.maxHP < now.Status.maxHP)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Status.maxHP - compare.Status.maxHP < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.Status.maxHP - compare.Status.maxHP)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Status.maxHP - compare.Status.maxHP).ToString();
                }
                else
                {
                    if (now.Status.maxHP > 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Status.maxHP < 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Status.maxHP < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * now.Status.maxHP).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += now.Status.maxHP.ToString();
                }
                break;
            case "Jump":
                if (exist)
                {
                    if (compare.Status.jumpPower > now.Status.jumpPower)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Status.jumpPower < now.Status.jumpPower)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Status.jumpPower - compare.Status.jumpPower < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.Status.jumpPower - compare.Status.jumpPower)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Status.jumpPower - compare.Status.jumpPower).ToString();
                }
                else
                {
                    if (now.Status.jumpPower > 1)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Status.jumpPower < 1)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Status.jumpPower < 1) delta.GetComponent<TextMeshProUGUI>().text += (1 - now.Status.jumpPower).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Status.jumpPower - 1 ).ToString();
                }
                break;
            case "Status.moveSpeed":
                if (exist)
                {
                    if (compare.Status.moveSpeed > now.Status.moveSpeed)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Status.moveSpeed < now.Status.moveSpeed)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Status.moveSpeed - compare.Status.moveSpeed < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.Status.moveSpeed - compare.Status.moveSpeed)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Status.moveSpeed - compare.Status.moveSpeed).ToString();
                }
                else
                {
                    if (now.Status.moveSpeed > 1)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Status.moveSpeed < 1)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Status.moveSpeed < 1) delta.GetComponent<TextMeshProUGUI>().text += (1 - now.Status.moveSpeed).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Status.moveSpeed - 1).ToString();
                }
                break;
            case "Status.power":
                if (exist)
                {
                    if (compare.Status.power > now.Status.power)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Status.power < now.Status.power)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Status.power - compare.Status.power < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * (now.Status.power - compare.Status.power)).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += (now.Status.power - compare.Status.power).ToString();
                }
                else
                {
                    if (now.Status.power > 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Status.power < 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Status.power < 0) delta.GetComponent<TextMeshProUGUI>().text += (-1 * now.Status.power).ToString();
                    else delta.GetComponent<TextMeshProUGUI>().text += now.Status.power.ToString();
                }
                break;
            default:
                break;
        }


    }

    private void UpdateItemInformation()
    {
        string grade;
        // �̸� , �̹���
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Name;
        transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
        // ���� �г�
        transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().MountingPart;
        transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Occupation;
        // ���� �г�
        transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "���ݼӵ�  " + string.Format("{0:0.#}", item.GetComponent<ItemStatus>().Status.attackSpeed.ToString());
        CompareToInspector("Status.attackSpeed", transform.GetChild(3).GetChild(0).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = "������  " + item.GetComponent<ItemStatus>().CursedRate.ToString();
        CompareToInspector("CursedRate", transform.GetChild(3).GetChild(1).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = "����  " + item.GetComponent<ItemStatus>().Status.defenseRate.ToString();
        CompareToInspector("Status.defenseRateCapability", transform.GetChild(3).GetChild(2).Find("Delta").gameObject);
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
        transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>().text = "���  " + grade;
        CompareToInspector("Grade", transform.GetChild(3).GetChild(3).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(4).GetComponent<TextMeshProUGUI>().text = "ü��  " + item.GetComponent<ItemStatus>().Status.maxHP.ToString();
        CompareToInspector("HP", transform.GetChild(3).GetChild(4).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(5).GetComponent<TextMeshProUGUI>().text = "������  " + item.GetComponent<ItemStatus>().Status.jumpPower.ToString();
        CompareToInspector("Jump", transform.GetChild(3).GetChild(5).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(6).GetComponent<TextMeshProUGUI>().text = "�̵��ӵ�  " + item.GetComponent<ItemStatus>().Status.moveSpeed.ToString();
        CompareToInspector("Status.moveSpeed", transform.GetChild(3).GetChild(6).Find("Delta").gameObject);
        transform.GetChild(3).GetChild(7).GetComponent<TextMeshProUGUI>().text = "���ݷ�  " + item.GetComponent<ItemStatus>().Status.power.ToString();
        CompareToInspector("Status.power", transform.GetChild(3).GetChild(7).Find("Delta").gameObject);
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