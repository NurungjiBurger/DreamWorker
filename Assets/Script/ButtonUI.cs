using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour //, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private GameObject Inventory;
    [SerializeField]
    private GameObject Inspector;

    private int number;
    private Sprite itemImage;

    float distance = 10;

    private bool onOff;

    public void InsertImage(GameObject item)
    {
        itemImage = item.GetComponent<SpriteRenderer>().sprite;
        GetComponent<Image>().sprite = itemImage;
    }

    /*
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("실행합니다");
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Debug.Log("실행중");
        transform.position = Input.mousePosition;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("끝났습니다");
    }
    */
    public void Test()
    {
        Debug.Log("실행");
    }

    public void InventoryActive()
    {
        onOff = !onOff;
        Inventory.SetActive(onOff);
        Inspector.SetActive(onOff);
    }

    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<Button>().interactable = true;
        onOff = false;
        if (name.Equals("Item"))
        {
            Debug.Log("슬롯입니다");
            GetComponent<Button>().onClick.AddListener(Test);
            Debug.Log("아이템입니다");
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
