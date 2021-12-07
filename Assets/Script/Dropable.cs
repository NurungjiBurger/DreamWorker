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
