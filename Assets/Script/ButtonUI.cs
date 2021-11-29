using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour
{
    private GameObject[] ui = new GameObject [2];

    private bool onOff = false;

    public bool OnOff { get { return onOff; } }

    // 저장
    public void SaveButton()
    {
        GameObject.Find("Data").GetComponent<DataController>().SaveGameData();
    }

    // 메인메뉴로
    public void MainButton()
    {
        GameObject.Find("GameController").GetComponent<GameController>().ReturntoMain();
    }

    // 게임종료
    public void ExitGame()
    {
        GameObject.Find("GameController").GetComponent<GameController>().ExitGame();
    }

    public void RefreshPosition()
    {
        GameObject.Find("GameController").GetComponent<GameController>().RefreshPlayerPosition();
    }

    // UI 닫기
    public void CloseButton()
    {
        if (transform.parent.name.Equals("Enhancer"))
        {
            GameObject.Find("Canvas").transform.Find("Inventory").transform.Find(transform.parent.name + "Button").GetComponent<ButtonUI>().UIActive();
            GameObject.Find("Canvas").transform.Find("Enhancer").GetComponent<Enhancer>().DiscardToEnhancer(true);

        }
        else if (transform.parent.name.Equals("Inventory") || transform.parent.name.Equals("Inspector"))
        {
            GameObject.Find("Canvas").transform.Find("Inventory&InspectorButton").GetComponent<ButtonUI>().UIActive();
        }
        else if (transform.parent.name.Equals("Pause"))
        {
            GameObject.Find("Canvas").transform.Find("MenuButton").GetComponent<ButtonUI>().UIActive();
        }
        else if (transform.parent.name.Equals("Background")) GameObject.Find("Canvas").transform.Find(transform.parent.parent.name + "Button").GetComponent<ButtonUI>().UIActive();
        else GameObject.Find("Canvas").transform.Find(transform.parent.name + "Button").GetComponent<ButtonUI>().UIActive();
    }

    public void BlackSmith()
    {
        GameObject.Find("BlackSmith").transform.position = new Vector3(300, 310);
    }

    // UI on / off
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
        // UI 할당
        if (!ui[0])
        {
            switch (transform.name)
            {
                case "Inventory&InspectorButton":
                    ui[0] = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
                    ui[1] = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
                    break;
                case "EnhancerButton":
                    ui[0] = GameObject.Find("Canvas").transform.Find("Enhancer").gameObject;
                    break;
                case "LoadButton":
                    ui[0] = GameObject.Find("Canvas").transform.Find("EnhancerSelecter").gameObject;
                    ui[0].SetActive(onOff);
                    break;
                case "BlackSmithButton":
                    ui[0] = GameObject.Find("Canvas").transform.Find("Smithy").gameObject;
                    break;
                case "SaveButton":
                    ui[0] = GameObject.Find("Canvas").transform.Find("Save").gameObject;
                    break;
                case "DescriptionButton":
                    ui[0] = GameObject.Find("Canvas").transform.Find("Description").gameObject;
                    break;
                case "MenuButton":
                    ui[0] = GameObject.Find("Canvas").transform.Find("Pause").gameObject;
                    break;
                default:
                    break;
            }
        }
        else
        {
            ui[0].SetActive(onOff);
            if (ui[1]) ui[1].SetActive(onOff);
        }
    }
}
