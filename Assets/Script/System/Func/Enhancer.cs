using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enhancer : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabTimer;
    [SerializeField]
    private GameObject itemPanel;
    [SerializeField]
    private GameObject inspector;
    [SerializeField]
    private GameObject inventory;

    private int possessItemPiece;
    private int success;

    private GameObject prefabItem;
    private GameObject player;
    private Timer timer;

    private GameObject itemPiece;

    private Slot devoteItem;
    private Slot enhanceItem;

    private int devoteItemPiece;

    public Slot EnhanceItem { get { return enhanceItem; } }

    // ��ȭ ���� Ȯ�� ���
    private void CalculateSuccess()
    {
        success = 0;

        // ��ȭ������ �������� ����Ȯ���� �����ؾ���
        if (enhanceItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel < 2)
        {
            success = 75;

        }
        else if (enhanceItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel >= 2 && enhanceItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel < 4)
        {
            success = 50;
        }
        else
        {
            success = 25;
        }

        // ����Ȯ�� =  �⺻Ȯ�� + ( ������ ������ * 0.5 )
        success = (int)((float)success + (devoteItemPiece * 0.5f));
    }

    // ��ȭ ����Ȯ�� ǥ��
    private void EnhancerSuccessRatePrint()
    {
        CalculateSuccess();

        transform.Find("SuccessRate").GetComponent<TextMeshProUGUI>().text = "��ȭ ���� Ȯ���� " + success.ToString() + "% �Դϴ�.\n ������ ��ĥ ������ ������ ���� �������ּ���.";
    }

    // ��ȭ ����
    private void Success()
    {
        timer.TimerSetZero();
        transform.Find("Success").gameObject.SetActive(true);
    }

    // ��ȭ ����
    private void Fail()
    {
        timer.TimerSetZero();
        transform.Find("Fail").gameObject.SetActive(true);
    }

    // ��ȭ�� ������ ���
    public void Enroll(Slot item, bool enhance)
    {
        if (enhance) enhanceItem = item;
        else devoteItem = item;
    }

    public void Enhance()
    {
        CalculateSuccess();

        devoteItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().DestoryAll();
        devoteItem.GetComponent<Slot>().DestroyObject();

        // ��ȭ ������ �������� ���� ����
        if (Random.Range(0,101) <= success)
        {
            Success();
            enhanceItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().StatUP(true);
        }
        else
        {
            Fail();
        }

        GameObject.Find("Canvas").transform.Find("ItemPanel").transform.Find("EnhanceButton").GetComponent<ButtonUI>().UIActive();
        player.GetComponent<PlayerStatus>().Data.itemPiece -= devoteItemPiece;
        enhanceItem = null;
        devoteItem = null;
    }

    private void OnDisable()
    {
        enhanceItem = null;
        devoteItem = null;
    }

    void Start()
    {
        timer = Instantiate(prefabTimer).GetComponent<Timer>();

        timer.SetCooldown(0.5f);
    }

    void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (!player) player = GameObject.FindGameObjectWithTag("Player");


        if (enhanceItem != null && devoteItem != null && player)
        {
            itemPanel.SetActive(false);

            // �÷��̾ ������ ������������ �ִ밡�� ����
            // 0 / 50 ~ 50 / 50

            possessItemPiece = player.GetComponent<PlayerStatus>().Data.itemPiece;

            if (possessItemPiece > 50) possessItemPiece = 50;

            transform.Find("SelectNumberBackground").GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)(transform.Find("Background").GetChild(0).GetComponent<Slider>().value * possessItemPiece)).ToString() + " / " + possessItemPiece.ToString();
            devoteItemPiece = (int)(transform.Find("Background").GetChild(0).GetComponent<Slider>().value * possessItemPiece);
            EnhancerSuccessRatePrint();

            // ��ȭ �������� ��ϵǾ��ְ� ������ ��ĥ �����۵� �ִ� ��� ���� Ȯ�� ǥ��
            CalculateSuccess();

            transform.Find("SuccessRate").gameObject.SetActive(true);
        }
        else transform.Find("SuccessRate").gameObject.SetActive(false);

        // ���� ���� �۾��� �ð��� ������ �ڵ����� ����
        if (timer.CooldownCheck())
        {
            transform.Find("Success").gameObject.SetActive(false);
            transform.Find("Fail").gameObject.SetActive(false);
            if (enhanceItem == null) gameObject.SetActive(false);
        }
    }
}
