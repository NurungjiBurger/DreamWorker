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
        GameObject.Find("Canvas").transform.Find("ExchangeSelecter").GetComponent<ExchangeSelecter>().SelectItem(transform.GetSiblingIndex());
    }

    public void CloseButton()
    {
        if (transform.parent.name.Equals("Exchanger"))
        {
            GameObject.Find("Canvas").transform.Find("Inventory").transform.Find(transform.parent.name + "Button").GetComponent<ButtonUI>().UIActive();
            GameObject.Find("Canvas").transform.Find("Exchanger").GetComponent<Exchanger>().DiscardToExchanger();

        }
        else if (transform.parent.name.Equals("ExchangeSelecter"))
        {
            GameObject.Find("Canvas").transform.Find("Exchanger").transform.Find("ButtonBackground").transform.Find("ChangeableButton").GetComponent<ButtonUI>().UIActive();
            GameObject.Find("Canvas").transform.Find("ExchangeSelecter").GetComponent<ExchangeSelecter>().DeleteAllList();
        }
        else GameObject.Find("Canvas").transform.Find(transform.parent.name + "Button").GetComponent<ButtonUI>().UIActive();
    }

    public void UIActive()
    {
        onOff = !onOff;
        if (transform.name == "ChangeableButton" && onOff) GameObject.Find("Canvas").transform.Find("ExchangeSelecter").GetComponent<ExchangeSelecter>().LoadAllItem();
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
                case "ChangeableButton":
                    ui = GameObject.Find("Canvas").transform.Find("ExchangeSelecter").gameObject;
                    break;
                default:
                    break;
            }
        }
        else ui.SetActive(onOff);
    }
}
