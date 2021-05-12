using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dropable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private Vector3 diff;

    private bool isDrag = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        startPosition = transform.position; 
        diff = new Vector3(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y, Input.mousePosition.z - transform.position.z);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        transform.position = new Vector3(Input.mousePosition.x - diff.x, Input.mousePosition.y - diff.y, Input.mousePosition.z - diff.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        if (name.Equals("Slot(Clone)"))
        {
            if (!GetComponent<UISensor>().DiscardAble)
            {
                if (GetComponent<UISensor>().MountAble) GetComponent<Slot>().Mounting();
                else
                {
                    if (GetComponent<UISensor>().ToInventory) GetComponent<Slot>().DisMounting();
                    else transform.position = startPosition;
                }
            }
            else
            {
                Debug.Log("¿ÜºÎ");
                transform.position = startPosition;
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
