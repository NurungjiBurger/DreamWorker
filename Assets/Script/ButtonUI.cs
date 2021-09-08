using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour
{
    private GameObject ui;

    private bool onOff = false;

    public void SaveButton()
    {
        GameObject.Find("Data").GetComponent<DataController>().SaveGameData();
    }

    public void CloseButton()
    {
        if (transform.parent.name.Equals("Exchanger"))
        {
            GameObject.Find("Canvas").transform.Find("Inventory").transform.Find(transform.parent.name + "Button").GetComponent<ButtonUI>().UIActive();
            GameObject.Find("Canvas").transform.Find("Exchanger").GetComponent<Exchanger>().DiscardToExchanger();

        }
        else GameObject.Find("Canvas").transform.Find(transform.parent.name + "Button").GetComponent<ButtonUI>().UIActive();
    }

    public void BlackSmith()
    {
        GameObject.Find("BlackSmith").transform.position = new Vector3(300, 310);
    }

    public void UIActive()
    {
        onOff = !onOff;
        if (transform.name == "ChangeableButton" && onOff) GameObject.Find("Canvas").transform.Find("ExchangeSelecter").GetComponent<ExchangeSelecter>().LoadAllItem();
    }

    void Start()
    {

    }

    void Update()
    {
        if (!ui)
        {
            switch (transform.name)
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
                    ui.SetActive(onOff);
                    break;
                case "BlackSmithButton":
                    ui = GameObject.Find("Canvas").transform.Find("Smithy").gameObject;
                    break;
                default:
                    break;
            }
        }
        else
        {
            ui.SetActive(onOff);
        }
    }
}
