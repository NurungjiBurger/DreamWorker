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

    private Vector3 startPosition;
    private Vector3 diff;
    private GameObject tmp;
    private RectTransform rectTransform;

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
            if (name.Equals("Slot(Clone)"))
            {
                tmp = transform.parent.gameObject;
                index = transform.GetSiblingIndex();
                transform.SetParent(GameObject.Find("Canvas").transform);
            }
            else
            {
                index = transform.GetSiblingIndex();
                transform.SetSiblingIndex(transform.parent.childCount);
            }
            startPosition = transform.position;
            diff = new Vector3(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y, Input.mousePosition.z - transform.position.z);
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
            transform.SetParent(tmp.transform);
            transform.SetSiblingIndex(index);

            // 드랍되었을때 충돌했던 콜라이더를 바탕으로 인스펙터, 인벤토리 등 위치 결정
            if (GetComponent<UISensor>().MountAble) GetComponent<Slot>().Mounting();
            else if (GetComponent<UISensor>().ToInventory) GetComponent<Slot>().DisMounting();
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

    }
}
