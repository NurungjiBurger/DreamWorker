using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour
{
    private GameObject ui;

    private bool onOff = false;

    public void SelectItem()
    {

    }

    public void CloseButton()
    {
        if (transform.parent.name.Equals("Exchanger")) GameObject.Find("Canvas").transform.Find("Inventory").transform.Find(transform.parent.name + "Button").GetComponent<ButtonUI>().UIActive();
        else GameObject.Find("Canvas").transform.Find(transform.parent.name + "Button").GetComponent<ButtonUI>().UIActive();
    }

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
        if (!ui)
        {
            switch(transform.name)
            {
                case "InventoryButton":
                    ui = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
                    break;
                case "InspectorButton":
                    ui = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
                    break;
                case "ExchangerButton":
                    ui = GameObject.Find("Canvas").transform.Find("Exchanger").gameObject;
                    break;
                default:
                    break;
            }
        }
        else ui.SetActive(onOff);
    }
}
