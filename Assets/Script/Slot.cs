using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour , IPointerClickHandler//, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private Sprite itemImage;

    public void SEND()
    {
        Debug.Log("�֤�����");
    }

    public void InsertImage(GameObject item)
    {
        itemImage = item.GetComponent<SpriteRenderer>().sprite;
        transform.Find("Item").GetComponent<Image>().sprite = itemImage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("����");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Mouse Click Button : Left");
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
            /*
            if (item != null)
            {
                if (item.itemType == Item.ItemType.Equipment)
                {
                    // ����
                    StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
                }
                else
                {
                    // �Һ�
                    Debug.Log(item.itemName + " �� ����߽��ϴ�.");
                    SetSlotCount(-1);
                }
            }
            */
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (name.Equals("Slot(Clone)"))
        {
            Debug.Log("�����Դϴ�");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

}
