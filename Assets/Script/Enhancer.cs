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

    private void CalculateSuccess()
    {
        success = 0;

        // 강화레벨이 높을수록 성공확률은 감소해야한다.
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

    private void EnhancerSuccessRatePrint()
    {
        CalculateSuccess();

        transform.Find("SuccessRate").GetComponent<TextMeshProUGUI>().text = "강화 성공 확률은 " + success.ToString() + "% 입니다.";
    }

    private void Success()
    {
        timer.TimerSetZero();
        transform.Find("Success").gameObject.SetActive(true);
    }

    private void Fail()
    {
        timer.TimerSetZero();
        transform.Find("Fail").gameObject.SetActive(true);
    }

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

        for (int i = 0; i < size; i++)
        {
            tmp = devoteItemList[0];
            devoteItemList.Remove(devoteItemList[0]);
            tmp.SlotItem.GetComponent<ItemStatus>().DestoryAll();
            Destroy(tmp.gameObject);
        }

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

        if (devoteItemList.Count == 0)
        {
            transform.Find("ButtonBackground").transform.Find("EnrollButton").gameObject.SetActive(false);
            transform.Find("ButtonBackground").transform.Find("DevoteButton").gameObject.SetActive(false);
        }
        else
        {
            if (!enhanceItem && devoteItemList.Count == 1) transform.Find("ButtonBackground").transform.Find("EnrollButton").gameObject.SetActive(true);
            else transform.Find("ButtonBackground").transform.Find("EnrollButton").gameObject.SetActive(false);

            if (enhanceItem) transform.Find("ButtonBackground").transform.Find("DevoteButton").gameObject.SetActive(true);
        }

        if (enhanceItem != null && devoteItemList.Count != 0)
        {
            transform.Find("SuccessRate").gameObject.SetActive(true);
            EnhancerSuccessRatePrint();
        }
        else transform.Find("SuccessRate").gameObject.SetActive(false);

        if (timer.CooldownCheck())
        {
            transform.Find("Success").gameObject.SetActive(false);
            transform.Find("Fail").gameObject.SetActive(false);
        }
    }
}
