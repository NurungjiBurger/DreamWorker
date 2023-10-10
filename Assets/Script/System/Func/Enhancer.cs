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

    // 강화 성공 확률 계산
    private void CalculateSuccess()
    {
        success = 0;

        // 강화레벨이 높을수록 성공확률은 감소해야함
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

        // 성공확률 =  기본확률 + ( 투자한 조각수 * 0.5 )
        success = (int)((float)success + (devoteItemPiece * 0.5f));
    }

    // 강화 성공확률 표시
    private void EnhancerSuccessRatePrint()
    {
        CalculateSuccess();

        transform.Find("SuccessRate").GetComponent<TextMeshProUGUI>().text = "강화 성공 확률은 " + success.ToString() + "% 입니다.\n 제물로 바칠 아이템 조각의 수를 결정해주세요.";
    }

    // 강화 성공
    private void Success()
    {
        timer.TimerSetZero();
        transform.Find("Success").gameObject.SetActive(true);
    }

    // 강화 실패
    private void Fail()
    {
        timer.TimerSetZero();
        transform.Find("Fail").gameObject.SetActive(true);
    }

    // 강화할 아이템 등록
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

        // 강화 성공시 아이템의 스탯 증가
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

            // 플레이어가 소지한 아이템조각이 최대가능 갯수
            // 0 / 50 ~ 50 / 50

            possessItemPiece = player.GetComponent<PlayerStatus>().Data.itemPiece;

            if (possessItemPiece > 50) possessItemPiece = 50;

            transform.Find("SelectNumberBackground").GetChild(0).GetComponent<TextMeshProUGUI>().text = ((int)(transform.Find("Background").GetChild(0).GetComponent<Slider>().value * possessItemPiece)).ToString() + " / " + possessItemPiece.ToString();
            devoteItemPiece = (int)(transform.Find("Background").GetChild(0).GetComponent<Slider>().value * possessItemPiece);
            EnhancerSuccessRatePrint();

            // 강화 아이템이 등록되어있고 제물로 바칠 아이템도 있는 경우 성공 확률 표시
            CalculateSuccess();

            transform.Find("SuccessRate").gameObject.SetActive(true);
        }
        else transform.Find("SuccessRate").gameObject.SetActive(false);

        // 성공 실패 글씨가 시간이 지나면 자동으로 꺼짐
        if (timer.CooldownCheck())
        {
            transform.Find("Success").gameObject.SetActive(false);
            transform.Find("Fail").gameObject.SetActive(false);
            if (enhanceItem == null) gameObject.SetActive(false);
        }
    }
}
