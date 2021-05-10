using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour //, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private GameObject Inventory;
    private GameObject Inspector;

    private int number;
    private Sprite itemImage;

    float distance = 10;

    private bool onOff;

    public void InputNumber(int index)
    {
        number = index;
    }

    public void SelectCommand()
    {
        GameObject.Find("Canvas").transform.Find("Inventory(Clone)").transform.Find("InventoryBackground").transform.GetChild(number).GetComponent<Slot>().ConductCommand(transform.Find("Text").GetComponent<Text>().text);
        Destroy(transform.parent.gameObject);
    }

    public void InventoryActive()
    {
        onOff = !onOff;
    }

    // Start is called before the first frame update
    void Start()
    {
        onOff = false;       
    }

    // Update is called once per frame
    void Update()
    {
        if (name.Equals("InventoryButton(Clone)"))
        {
            if (!Inspector) Inspector = GameObject.Find("Canvas").transform.Find("Inspector(Clone)").gameObject;
            if (!Inventory) Inventory = GameObject.Find("Canvas").transform.Find("Inventory(Clone)").gameObject;

            Inventory.SetActive(onOff);
            Inspector.SetActive(onOff);
        }
    }
}
