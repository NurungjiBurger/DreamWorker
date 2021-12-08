using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dropable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    [SerializeField, Range(10f, 250f)]
    private float leverRange;

    private int index;
    private GameObject inspector;
    private GameObject inventory;

    private Vector3 startPosition;
    private Vector3 diff;
    private GameObject tmp;
    private RectTransform rectTransform;

    // 슬롯이 이동하면서 각각 인벤에 가려지거나 장비창에 가려지는것을 방지하기위해 상호간 위치를 바꿈
    public void SwapInspecInven(string str)
    {
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;

        int siblingindex;
        bool value = false;

        switch(str)
        {
            case "Inventory":
                // 인벤토리가 위에 있어야함
                if (inspector.transform.GetSiblingIndex() < inventory.transform.GetSiblingIndex()) value = true;
                break;
            case "Inspector":
                // 장비창이 위에 있어야함
                if (inspector.transform.GetSiblingIndex() > inventory.transform.GetSiblingIndex()) value = true;
                break;
            default:
                break;
        }


        if (value)
        {
            siblingindex = inspector.transform.GetSiblingIndex();
            inspector.transform.SetSiblingIndex(inventory.transform.GetSiblingIndex());
            inventory.transform.SetSiblingIndex(siblingindex);
        }

    }

    // 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (name.Equals("JoyStick"))
        {
            // 모든 화면에서 인식되지 않기 위해 거리 제한
            if (Input.mousePosition.magnitude <= (transform.position + new Vector3(140,140,0)).magnitude)
            {
                var inputDir = Input.mousePosition - transform.position;
                var clampedDir = inputDir.magnitude < leverRange ? inputDir : (inputDir.normalized * leverRange);


                lever.position = clampedDir;
            }
        }
        else
        {
            // 슬롯 원래 위치 기억
            if (name.Equals("Slot(Clone)"))
            {
                startPosition = transform.position;
                diff = new Vector3(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y, Input.mousePosition.z - transform.position.z);
            }
        }
    }

    // 드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        if (name.Equals("JoyStick"))
        {
            // 모든 화면에서 인식되지 않기 위해 거리 제한
            if (Input.mousePosition.magnitude <= (transform.position + new Vector3(140, 140, 0)).magnitude)
            {
                var inputDir = Input.mousePosition - transform.position;
                var clampedDir = inputDir.magnitude < leverRange ? transform.position + inputDir : transform.position + (inputDir.normalized * leverRange);

                lever.position = clampedDir;
            }
        }
        else
        {
            // 슬롯 드래그중 위치변화
            transform.position = new Vector3(Input.mousePosition.x - diff.x, Input.mousePosition.y - diff.y, Input.mousePosition.z - diff.z);
        }
    }

    // 드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        if (name.Equals("JoyStick"))
        {
            // 조이스틱에서 손이 떼지면 레버는 원위치로 돌아감
            lever.anchoredPosition = Vector2.zero;
        }

        if (name.Equals("Slot(Clone)"))
        {
            // 인벤 -> 장비창
            if (GetComponent<UISensor>().ToInspector && transform.parent.parent.name.Equals("Inventory"))
            {
                // 선택한 슬롯의 아이템을 장착
                // 장비창에 같은 부위에 장비한 아이템이 있는지 확인
                Slot tmp = inspector.GetComponent<Inspector>().FindInInspector(GetComponent<Slot>());
                
                // 없다면 바로 장착
                if (tmp == gameObject.GetComponent<Slot>())
                {
                    inventory.GetComponent<Inventory>().DiscardToInventory(GetComponent<Slot>().transform.GetSiblingIndex());
                    inspector.GetComponent<Inspector>().AddToInspector(GetComponent<Slot>());
                }
                // 있다면 해당 아이템을 인벤토리로 옮기고 장착
                else
                {
                    inspector.GetComponent<Inspector>().DiscardToInspector(tmp);
                    inventory.GetComponent<Inventory>().DiscardToInventory(GetComponent<Slot>().transform.GetSiblingIndex());

                    inspector.GetComponent<Inspector>().AddToInspector(GetComponent<Slot>());
                    inventory.GetComponent<Inventory>().AddToInventory(tmp);
                }
            }
            // 장비창 -> 인벤
            else if (GetComponent<UISensor>().ToInventory && transform.parent.parent.name.Equals("Inspector"))
            {
                inspector.GetComponent<Inspector>().DiscardToInspector(GetComponent<Slot>());
                inventory.GetComponent<Inventory>().AddToInventory(GetComponent<Slot>());
            }
            // 장비창 -> 장비창 or 인벤 -> 인벤
            else transform.position = startPosition;
        }
        else
        {
            transform.SetSiblingIndex(index);
        }
    }

    private void Awake()
    {
        if (name.Equals("JoyStick")) rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;

        //if (name.Equals("Slot(Clone)")) Debug.Log(GetComponent<Slot>());
    }
}
