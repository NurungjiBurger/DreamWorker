using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enhancer : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabTimer;

    private int success;

    private GameObject prefabItem;
    private GameObject player;
    private Timer timer;

    private List<Slot> devoteItemList = new List<Slot>();
    private Slot enhanceItem;

    public int ItemCount { get { return devoteItemList.Count; } }
    public List<Slot> DevoteItemList { get { return devoteItemList; } }
    public Slot EnhanceItem { get { return enhanceItem; } }

    // 강화 성공 확률 계산
    private void CalculateSuccess()
    {
        success = 0;

        // 강화레벨이 높을수록 성공확률은 감소해야함
        if (enhanceItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel <= 10)
        {
            success = 100;

        }
        else if (enhanceItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel > 10 && enhanceItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel <= 30)
        {
            success = 75;
        }
        else if (enhanceItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel > 30 && enhanceItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel <= 60)
        {
            success = 50;
        }
        else if (enhanceItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel > 60 && enhanceItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel <= 100)
        {
            success = 25;
        }
        else
        {
            success = 15;
        }

        // 아이템의 enhancinglevel * 투자한 아이템 수 ( 1 , 1.125 , 1.25 , 1.375 , 1.5 )
        switch (devoteItemList.Count)
        {
            case 1:
                success *= 1;
                break;
            case 2:
                success = (int)(success * 1.25f);
                break;
            case 3:
                success = (int)(success * 1.5f);
                break;
            case 4:
                success = (int)(success * 1.75f);
                break;
            default:
                success *= 1;
                break;
        }
    }

    // 강화 성공확률 표시
    private void EnhancerSuccessRatePrint()
    {
        CalculateSuccess();

        transform.Find("SuccessRate").GetComponent<TextMeshProUGUI>().text = "강화 성공 확률은 " + success.ToString() + "% 입니다.";
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
    public void Enroll()
    {
        enhanceItem = devoteItemList[0];

        DiscardToEnhancer(false);
    }

    public void Enhance()
    {
        
        CalculateSuccess();

        Slot tmp;
        int size = devoteItemList.Count;

        // 제물로 사용된 아이템들은 강화 성공 여부에 관계없이 삭제
        for (int i = 0; i < size; i++)
        {
            tmp = devoteItemList[0];
            devoteItemList.Remove(devoteItemList[0]);
            tmp.SlotItem.GetComponent<ItemStatus>().DestoryAll();
            Destroy(tmp.gameObject);
        }

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
    }
    
    // 강화창에 올려진 아이템들 제거 ( 인스펙터, 인벤토리로 되돌아감 )
    public void DiscardToEnhancer(bool value)
    {

        if (devoteItemList.Count != 0)
        {
            Slot tmp;
            int size = devoteItemList.Count;
            for (int i = size - 1; i >= 0; i--)
            {
                tmp = devoteItemList[i];
                devoteItemList.Remove(devoteItemList[i]);
                tmp.GetComponent<Slot>().PullOutEnhancer();
            }
        }

        if (value) enhanceItem = null;
    }

    // 강화창에 아이템을 올림
    public void AddToEnhancer(Slot slot)
    {
        devoteItemList.Add(slot);

        slot.transform.SetParent(transform.Find("Background").transform);
    }

    void Start()
    {
        timer = Instantiate(prefabTimer).GetComponent<Timer>();

        timer.SetCooldown(0.5f);
    }

    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        if (gameObject.activeInHierarchy == false)
        {
            for (int i = 0; i < devoteItemList.Count; i++) devoteItemList.Remove(devoteItemList[0]);
        }

        // 아무런 아이템이 선택되지 않은 경우
        if (devoteItemList.Count == 0)
        {
            transform.Find("ButtonBackground").transform.Find("EnrollButton").gameObject.SetActive(false);
            transform.Find("ButtonBackground").transform.Find("DevoteButton").gameObject.SetActive(false);
        }
        else
        {
            // 강화 아이템이 등록되지 않고 선택된 아이템이 한개인 경우
            if (!enhanceItem && devoteItemList.Count == 1) transform.Find("ButtonBackground").transform.Find("EnrollButton").gameObject.SetActive(true);
            else transform.Find("ButtonBackground").transform.Find("EnrollButton").gameObject.SetActive(false);

            // 강화아이템이 등록된 경우
            if (enhanceItem) transform.Find("ButtonBackground").transform.Find("DevoteButton").gameObject.SetActive(true);
        }

        if (enhanceItem != null && devoteItemList.Count != 0)
        {
            // 강화 아이템이 등록되어있고 제물로 바칠 아이템도 선택된 경우 성공 확률 표시
            transform.Find("SuccessRate").gameObject.SetActive(true);
            EnhancerSuccessRatePrint();
        }
        else transform.Find("SuccessRate").gameObject.SetActive(false);

        // 성공 실패 글씨가 시간이 지나면 자동으로 꺼짐
        if (timer.CooldownCheck())
        {
            transform.Find("Success").gameObject.SetActive(false);
            transform.Find("Fail").gameObject.SetActive(false);
        }
    }
}
