using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyaerUtility : MonoBehaviour
{
    [SerializeField]
    private GameObject Inventory;
    [SerializeField]
    private GameObject Inspector;

    float distance = 10;

    private bool onOff;

    public void OnMouseDrag()
    {
        Debug.Log("½ÇÇà");
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objPosition;
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
