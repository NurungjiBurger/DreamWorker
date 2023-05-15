using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Description : MonoBehaviour
{
    [SerializeField]
    private GameObject[] button = new GameObject[2];
    [SerializeField]
    private Sprite[] backGroundImage = new Sprite[6];

    private int page;

    // 좌, 우 로 가기위한 버튼 조정
    private void ButtonOnoff()
    {
        if (page == 0) button[0].SetActive(false);
        else if (page == 3) button[1].SetActive(false);
        else
        {
            button[0].SetActive(true);
            button[1].SetActive(true);
        }
    }

    private void PrintPage()
    {
        // page = 0  메인메뉴
        // page = 1  캐릭터 선택창
        // page = 2  기본화면
        // page = 3  인벤토리화면
        // xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        // page = 4  강화창화면
        // page = 5  강화선택창화면

        GetComponent<Image>().sprite = backGroundImage[page];

        for(int idx = 0; idx < 4; idx++)
        {
            if (idx == page) transform.GetChild(0).GetChild(idx).gameObject.SetActive(true);
            else transform.GetChild(0).GetChild(idx).gameObject.SetActive(false);
        }
    }

    public void PageCount(int increase)
    {
        page += increase;

        PrintPage();
    }

    void Start()
    {
        page = 0;
        PrintPage();
    }

    void Update()
    {
        ButtonOnoff();
    }
}
