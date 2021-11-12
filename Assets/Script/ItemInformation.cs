using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemInformation : MonoBehaviour, IPointerClickHandler
{
    private GameObject item;
    private GameObject inspector;
    private Slot slot;

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
                transform.position = new Vector3(position.x + 220, position.y - 200, position.z);
                break;
            case 3:
            case 4:
            case 8:
            case 9:
                transform.position = new Vector3(position.x - 220, position.y - 200, position.z);
                break;
            case 10:
            case 11:
            case 12:
            case 15:
            case 16:
            case 17:
                transform.position = new Vector3(position.x + 220, position.y + 200, position.z);
                break;
            case 13:
            case 14:
            case 18:
            case 19:
                transform.position = new Vector3(position.x - 220, position.y + 200, position.z);
                break;
            case 36:
                transform.position = new Vector3(position.x + 220, position.y, position.z);
                break;
            case 37:
                transform.position = new Vector3(position.x - 220, position.y, position.z);
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

                    if (now.Data.attackSpeed - compare.Data.attackSpeed < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (-1 * (now.Data.attackSpeed - compare.Data.attackSpeed)));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}" , (now.Data.attackSpeed - compare.Data.attackSpeed));
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

                    if (now.Data.attackSpeed < 1) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (1 - now.Data.attackSpeed));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (now.Data.attackSpeed - 1));
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

                    if (now.CursedRate - compare.CursedRate < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (-1 * (now.CursedRate - compare.CursedRate)));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (now.CursedRate - compare.CursedRate));
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

                    if (now.CursedRate < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (-1 * now.CursedRate));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", now.CursedRate);
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

                    if (now.Data.defenseRate - compare.Data.defenseRate < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (-1 * (now.Data.defenseRate - compare.Data.defenseRate)));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (now.Data.defenseRate - compare.Data.defenseRate));
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

                    if (now.Data.defenseRate < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (-1 * now.Data.defenseRate));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", now.Data.defenseRate);
                }
                break;
            case "Data.evasionRate":                
                if (exist)
                {
                    if (compare.Data.evasionRate > now.Data.evasionRate)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else if (compare.Data.evasionRate < now.Data.evasionRate)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.evasionRate - compare.Data.evasionRate < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (-1 * (now.Data.evasionRate - compare.Data.evasionRate)));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (now.Data.evasionRate - compare.Data.evasionRate));
                }
                else
                {
                    if (now.Data.evasionRate > 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = red;
                        delta.GetComponent<TextMeshProUGUI>().text = "+ ";
                    }
                    else if (now.Data.evasionRate < 0)
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = blue;
                        delta.GetComponent<TextMeshProUGUI>().text = "- ";
                    }
                    else
                    {
                        delta.GetComponent<TextMeshProUGUI>().color = green;
                        delta.GetComponent<TextMeshProUGUI>().text = "";
                    }

                    if (now.Data.evasionRate < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", ( -1 * now.Data.evasionRate));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", now.Data.evasionRate);
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

                    if (now.Data.maxHP - compare.Data.maxHP < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (-1 * (now.Data.maxHP - compare.Data.maxHP)));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (now.Data.maxHP - compare.Data.maxHP));
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

                    if (now.Data.maxHP < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (-1 * now.Data.maxHP));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", now.Data.maxHP);
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

                    if (now.Data.jumpPower - compare.Data.jumpPower < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (-1 * (now.Data.jumpPower - compare.Data.jumpPower)));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (now.Data.jumpPower - compare.Data.jumpPower));
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

                    if (now.Data.jumpPower < 1) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (1 - now.Data.jumpPower));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (now.Data.jumpPower - 1 ));
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

                    if (now.Data.moveSpeed - compare.Data.moveSpeed < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (-1 * (now.Data.moveSpeed - compare.Data.moveSpeed)));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (now.Data.moveSpeed - compare.Data.moveSpeed));
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

                    if (now.Data.moveSpeed < 1) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (1 - now.Data.moveSpeed));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (now.Data.moveSpeed - 1));
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

                    if (now.Data.power - compare.Data.power < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (-1 * (now.Data.power - compare.Data.power)));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (now.Data.power - compare.Data.power));
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

                    if (now.Data.power < 0) delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", (-1 * now.Data.power));
                    else delta.GetComponent<TextMeshProUGUI>().text += string.Format("{0:N2}", now.Data.power);
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
        // 이름 , 이미지
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Name + "  (+" + item.GetComponent<ItemStatus>().Data.enhancingLevel + ")";

        switch (item.GetComponent<ItemStatus>().ItemGrade)
        {
            case 0:
                // 노멀 하양
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 1f);
                break;
            case 1:
                // 레어 하늘
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(23 / 255f, 224 / 255f, 224 / 255f, 1f);
                break;
            case 2:
                // 에픽 보라
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(159 / 255f, 14 / 255f, 224 / 255f, 1f);
                break;
            case 3:
                // 유니크 노랑
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(233 / 255f, 241 / 255f, 35 / 255f, 1f);
                break;
            case 4:
                // 레전더리 초록
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(96 / 255f, 236 / 255f, 39 / 255f, 1f);
                break;
            default:
                break;
        }
        /*
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
        */
        // 정보 패널
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().MountingPart;
        transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Occupation;
        transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = item.GetComponent<ItemStatus>().Description;
        // 스탯 패널
        transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "공격속도  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().Data.attackSpeed);
        CompareToInspector("Data.attackSpeed", transform.GetChild(2).GetChild(0).Find("Delta").gameObject);
        transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "저주율  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().CursedRate);
        CompareToInspector("CursedRate", transform.GetChild(2).GetChild(1).Find("Delta").gameObject);
        transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = "방어력  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().Data.defenseRate);
        CompareToInspector("Data.defenseRateCapability", transform.GetChild(2).GetChild(2).Find("Delta").gameObject);
        transform.GetChild(2).GetChild(3).GetComponent<TextMeshProUGUI>().text = "회피  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().Data.evasionRate); 
        CompareToInspector("Data.evasionRate", transform.GetChild(2).GetChild(3).Find("Delta").gameObject);
        transform.GetChild(2).GetChild(4).GetComponent<TextMeshProUGUI>().text = "체력  " + item.GetComponent<ItemStatus>().Data.maxHP.ToString();
        CompareToInspector("HP", transform.GetChild(2).GetChild(4).Find("Delta").gameObject);
        transform.GetChild(2).GetChild(5).GetComponent<TextMeshProUGUI>().text = "점프력  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().Data.jumpPower);
        CompareToInspector("Jump", transform.GetChild(2).GetChild(5).Find("Delta").gameObject);
        transform.GetChild(2).GetChild(6).GetComponent<TextMeshProUGUI>().text = "이동속도  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().Data.moveSpeed);
        CompareToInspector("Data.moveSpeed", transform.GetChild(2).GetChild(6).Find("Delta").gameObject);
        transform.GetChild(2).GetChild(7).GetComponent<TextMeshProUGUI>().text = "공격력  " + string.Format("{0:N2}", item.GetComponent<ItemStatus>().Data.power);
        CompareToInspector("Data.power", transform.GetChild(2).GetChild(7).Find("Delta").gameObject);
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