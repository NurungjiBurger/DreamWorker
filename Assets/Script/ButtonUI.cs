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
        if (transform.parent.name.Equals("Enhancer"))
        {
            GameObject.Find("Canvas").transform.Find("Inventory").transform.Find(transform.parent.name + "Button").GetComponent<ButtonUI>().UIActive();
            GameObject.Find("Canvas").transform.Find("Enhancer").GetComponent<Enhancer>().DiscardToEnhancer(true);

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
        if (transform.name == "LoadButton" && onOff) GameObject.Find("Canvas").transform.Find("EnhancerSelecter").GetComponent<EnhancerSelecter>().LoadAllItem();
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
                case "EnhancerButton":
                    ui = GameObject.Find("Canvas").transform.Find("Enhancer").gameObject;
                    break;
                case "LoadButton":
                    ui = GameObject.Find("Canvas").transform.Find("EnhancerSelecter").gameObject;
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
