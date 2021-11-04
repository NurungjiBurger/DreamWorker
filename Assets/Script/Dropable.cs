using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dropable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField, Range(10f, 250f)]
    private float leverRange;

    ////////////////////////////////////////////

    private Vector3 startPosition;
    private Vector3 diff;

    private bool isDrag = false;
    private GameObject tmp;
    private int index;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (name.Equals("JoyStick"))
        {
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
            isDrag = true;
            startPosition = transform.position;
            diff = new Vector3(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y, Input.mousePosition.z - transform.position.z);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (name.Equals("JoyStick"))
        {
            if (Input.mousePosition.magnitude <= (transform.position + new Vector3(140, 140, 0)).magnitude)
            {
                var inputDir = Input.mousePosition - transform.position;
                var clampedDir = inputDir.magnitude < leverRange ? transform.position + inputDir : transform.position + (inputDir.normalized * leverRange);

                lever.position = clampedDir;
            }
        }
        else
        {
            isDrag = true;
            transform.position = new Vector3(Input.mousePosition.x - diff.x, Input.mousePosition.y - diff.y, Input.mousePosition.z - diff.z);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (name.Equals("JoyStick"))
        {
            lever.anchoredPosition = Vector2.zero;
        }

        isDrag = false;
        if (name.Equals("Slot(Clone)"))
        {
            transform.SetParent(tmp.transform);
            transform.SetSiblingIndex(index);

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
