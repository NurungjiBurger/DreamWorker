using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour //, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private GameObject ui;

    private bool onOff = false;

    public void UIActive()
    {
        onOff = !onOff;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (name.Equals("InventoryButton(Clone)")) { if (!ui) ui = GameObject.Find("Canvas").transform.Find("Inventory(Clone)").gameObject; }
        if (name.Equals("InspectorButton(Clone)")) { if (!ui) ui = GameObject.Find("Canvas").transform.Find("Inspector(Clone)").gameObject; }
        if (name.Equals("ExchangerButton(Clone)")) { if (!ui) ui = GameObject.Find("Canvas").transform.Find("Exchanger(Clone)").gameObject; }
        ui.SetActive(onOff);
    }
}
