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

    private void ButtonOnoff()
    {
        if (page == 0) button[0].SetActive(false);
        else if (page == 5) button[1].SetActive(false);
        else
        {
            button[0].SetActive(true);
            button[1].SetActive(true);
        }
    }

    private void PrintPage()
    {
        // page = 0  ���θ޴�����
        // page = 1  ĳ���� ����â ����? �ʿ��Ѱ�
        // page = 2  �⺻ȭ�鼳��
        // page = 3  �κ��丮ȭ�鼳��
        // page = 4  ��ȭâȭ�鼳��
        // page = 5  ��ȭ����âȭ�鼳��

        GetComponent<Image>().sprite = backGroundImage[page];

        for(int idx = 0; idx < 6; idx++)
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
