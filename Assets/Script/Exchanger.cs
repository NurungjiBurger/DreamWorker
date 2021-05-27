using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exchanger : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabItem;

    private List<Slot> exchangeItemList = new List<Slot>();
    private GameObject player;

    public List<Slot> ExchangeItemList { get { return exchangeItemList; } }
    public int ItemCount { get { return exchangeItemList.Count; } }

    public void Exchange()
    {
        // 1개면 등급업 1퍼
        // 2개 20퍼
        // 3개 40퍼
        // 4개 60퍼
        int success;

        switch (exchangeItemList.Count)
        {
            case 1:
                success = 1;
                break;
            case 2:
                success = 20;
                break;
            case 3:
                success = 40;
                break;
            case 4:
                success = 60;
                break;
            default:
                success = 0;
                break;
        }

        int averageGrade = 0;
        int averageCursedRate = 0;

        for (int i = 0; i < exchangeItemList.Count; i++) { averageGrade += exchangeItemList[i].SlotItem.GetComponent<ItemStatus>().ItemGrade; averageCursedRate += exchangeItemList[i].SlotItem.GetComponent<ItemStatus>().CursedRate; }

        averageGrade /= exchangeItemList.Count;
        averageCursedRate /= exchangeItemList.Count;

        Debug.Log(averageCursedRate + "   " + averageGrade);

        Slot tmp;
        int size = ExchangeItemList.Count;

        for (int i = 0; i < size; i++)
        {
            tmp = exchangeItemList[0];
            exchangeItemList.Remove(exchangeItemList[0]);
            Destroy(tmp.SlotItem.gameObject);
            Destroy(tmp.gameObject);
        }

        if (Random.Range(0,101) <= success)
        {
            Debug.Log("등급업!");
            Instantiate(prefabItem[((averageGrade+1) * 5) + Random.Range(0, 5)], new Vector3(player.transform.position.x + 1, player.transform.position.y + 1, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(prefabItem[(averageGrade * 5) + Random.Range(0, 5)], new Vector3(player.transform.position.x + 1, player.transform.position.y + 1, 0), Quaternion.identity);
        }


        GameObject.Find("Canvas").transform.Find("Inventory").transform.Find("ExchangerButton").GetComponent<ButtonUI>().UIActive();
        transform.gameObject.SetActive(false);

        GameObject.Find("Canvas").transform.Find("InspectorButton").GetComponent<ButtonUI>().UIActive();
        GameObject.Find("Canvas").transform.Find("InventoryButton").GetComponent<ButtonUI>().UIActive();

    }
    
    public void DiscardToExchanger()
    {
        if (exchangeItemList.Count != 0)
        {
            Slot tmp;
            int size = exchangeItemList.Count;
            for (int i = size - 1; i >= 0; i--)
            {
                tmp = exchangeItemList[i];
                exchangeItemList.Remove(exchangeItemList[i]);
                tmp.GetComponent<Slot>().PullOutExchanger();
            }
        }
    }

    public void AddToExchanger(Slot slot)
    {
        exchangeItemList.Add(slot);

        slot.transform.SetParent(transform.Find("Background").transform);
        slot.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        slot.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        if (gameObject.activeInHierarchy == false)
        {
            for (int i = 0; i < exchangeItemList.Count; i++) exchangeItemList.Remove(exchangeItemList[0]);
        }

        if (exchangeItemList.Count == 0) transform.Find("ButtonBackground").transform.Find("ExchangeButton").gameObject.SetActive(false);
        else transform.Find("ButtonBackground").transform.Find("ExchangeButton").gameObject.SetActive(true);
    }
}
