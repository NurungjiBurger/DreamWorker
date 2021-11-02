using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhancer : MonoBehaviour
{
    private GameObject prefabItem;

    private List<Slot> devoteItemList = new List<Slot>();
    private Slot enhanceItem;
    private GameObject player;

    public List<Slot> DevoteItemList { get { return devoteItemList; } }
    public int ItemCount { get { return devoteItemList.Count; } }
    public Slot EnhanceItem { get { return enhanceItem; } }

    public void Enroll()
    {
        enhanceItem = devoteItemList[0];

        DiscardToEnhancer(false);
    }

    public void Enhance()
    {
        int success = 0;

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

        Debug.Log("Success " + (100 - success));

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
            //Debug.Log("강화성공");
            enhanceItem.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().StatUP(true);
        }
        else
        {
            //Debug.Log("강화실패");
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
        slot.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        slot.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
    }

    void Start()
    {
        
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
    }
}
