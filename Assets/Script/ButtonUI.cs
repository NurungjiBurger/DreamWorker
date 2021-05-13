using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour //, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private GameObject[] ui = new GameObject[2];

    private bool onOff = false;

    public void UIActive()
    {
        onOff = !onOff;
        ui[0].SetActive(onOff);
        ui[1].SetActive(onOff);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (name.Equals("InventoryButton(Clone)"))
        {
            if (!ui[0]) { ui[0] = GameObject.Find("Canvas").transform.Find("Inspector(Clone)").gameObject; ui[0].SetActive(onOff); }
            if (!ui[1]) { ui[1] = GameObject.Find("Canvas").transform.Find("Inventory(Clone)").gameObject; ui[1].SetActive(onOff); }

        }
        else
        {
            if (!ui[0]) { ui[0] = GameObject.Find("Canvas").transform.Find("Synthesizer(Clone)").gameObject; ui[0].SetActive(onOff); }
                if (!ui[1]) { ui[1] = GameObject.Find("Canvas").transform.Find("Inventory(Clone)").gameObject; ui[1].SetActive(onOff); }
        }
    }
}
