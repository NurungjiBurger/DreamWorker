using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dropable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!GetComponent<UISensor>().DiscardAble)
        {
            if (GetComponent<UISensor>().MountAble) GetComponent<Slot>().Mounting();
            else
            {
                if (GetComponent<UISensor>().ToInventory) GetComponent<Slot>().DisMounting();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
