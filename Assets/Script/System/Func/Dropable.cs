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

    // ������ �̵��ϸ鼭 ���� �κ��� �������ų� ���â�� �������°��� �����ϱ����� ��ȣ�� ��ġ�� �ٲ�
    public void SwapInspecInven(string str)
    {
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;

        int siblingindex;
        bool value = false;

        switch(str)
        {
            case "Inventory":
                // �κ��丮�� ���� �־����
                if (inspector.transform.GetSiblingIndex() < inventory.transform.GetSiblingIndex()) value = true;
                break;
            case "Inspector":
                // ���â�� ���� �־����
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

    // �巡�� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (name.Equals("JoyStick"))
        {
            // ��� ȭ�鿡�� �νĵ��� �ʱ� ���� �Ÿ� ����
            if (Input.mousePosition.magnitude <= (transform.position + new Vector3(140,140,0)).magnitude)
            {
                var inputDir = Input.mousePosition - transform.position;
                var clampedDir = inputDir.magnitude < leverRange ? inputDir : (inputDir.normalized * leverRange);


                lever.position = clampedDir;
            }
        }
        else
        {
            // ���� ���� ��ġ ���
            if (name.Equals("Slot(Clone)"))
            {
                startPosition = transform.position;
                diff = new Vector3(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y, Input.mousePosition.z - transform.position.z);
            }
        }
    }

    // �巡�� ��
    public void OnDrag(PointerEventData eventData)
    {
        if (name.Equals("JoyStick"))
        {
            // ��� ȭ�鿡�� �νĵ��� �ʱ� ���� �Ÿ� ����
            if (Input.mousePosition.magnitude <= (transform.position + new Vector3(140, 140, 0)).magnitude)
            {
                var inputDir = Input.mousePosition - transform.position;
                var clampedDir = inputDir.magnitude < leverRange ? transform.position + inputDir : transform.position + (inputDir.normalized * leverRange);

                lever.position = clampedDir;
            }
        }
        else
        {
            // ���� �巡���� ��ġ��ȭ
            transform.position = new Vector3(Input.mousePosition.x - diff.x, Input.mousePosition.y - diff.y, Input.mousePosition.z - diff.z);
        }
    }

    // �巡�� ��
    public void OnEndDrag(PointerEventData eventData)
    {
        if (name.Equals("JoyStick"))
        {
            // ���̽�ƽ���� ���� ������ ������ ����ġ�� ���ư�
            lever.anchoredPosition = Vector2.zero;
        }

        if (name.Equals("Slot(Clone)"))
        {
            // �κ� -> ���â
            if (GetComponent<UISensor>().ToInspector && transform.parent.parent.name.Equals("Inventory"))
            {
                // ������ ������ �������� ����
                // ���â�� ���� ������ ����� �������� �ִ��� Ȯ��
                Slot tmp = inspector.GetComponent<Inspector>().FindInInspector(GetComponent<Slot>());
                
                // ���ٸ� �ٷ� ����
                if (tmp == gameObject.GetComponent<Slot>())
                {
                    inventory.GetComponent<Inventory>().DiscardToInventory(GetComponent<Slot>().transform.GetSiblingIndex());
                    inspector.GetComponent<Inspector>().AddToInspector(GetComponent<Slot>());
                }
                // �ִٸ� �ش� �������� �κ��丮�� �ű�� ����
                else
                {
                    inspector.GetComponent<Inspector>().DiscardToInspector(tmp);
                    inventory.GetComponent<Inventory>().DiscardToInventory(GetComponent<Slot>().transform.GetSiblingIndex());

                    inspector.GetComponent<Inspector>().AddToInspector(GetComponent<Slot>());
                    inventory.GetComponent<Inventory>().AddToInventory(tmp);
                }
            }
            // ���â -> �κ�
            else if (GetComponent<UISensor>().ToInventory && transform.parent.parent.name.Equals("Inspector"))
            {
                inspector.GetComponent<Inspector>().DiscardToInspector(GetComponent<Slot>());
                inventory.GetComponent<Inventory>().AddToInventory(GetComponent<Slot>());
            }
            // ���â -> ���â or �κ� -> �κ�
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
