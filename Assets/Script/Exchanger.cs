using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exchanger : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabSlot;

    [SerializeField]
    private GameObject[] prefabItem;

    private List<Slot> ExchangeItemList = new List<Slot>();
    private GameObject player;


    public void Exchange()
    {
        // 1개면 등급업 1퍼
        // 2개 20퍼
        // 3개 40퍼
        // 4개 60퍼
        int success;

        switch (ExchangeItemList.Count)
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

        // 새로 만들어질 아이템의 등급과 저주율 고려
        int averageGrade = 0;
        int averageCursedRate = 0;

        for (int i = 0; i < ExchangeItemList.Count; i++) { averageGrade += ExchangeItemList[i].SlotItem.GetComponent<ItemStatus>().ItemGrade; averageCursedRate += ExchangeItemList[i].SlotItem.GetComponent<ItemStatus>().CursedRate; }

        averageGrade /= ExchangeItemList.Count;
        averageCursedRate /= ExchangeItemList.Count;

        Debug.Log(averageCursedRate + "   " + averageGrade);

        Slot tmp;

        // 소모된 아이템들 삭제
        for (int i = 0; i < ExchangeItemList.Count; i++)
        {
            tmp = ExchangeItemList[i];
            ExchangeItemList.Remove(ExchangeItemList[i]);
            Destroy(tmp.SlotItem.gameObject);
            Destroy(tmp.gameObject);
        }

        if (Random.Range(0,101) <= success)
        {
            // 등급업 성공
            // 상위 등급 아이템 랜덤 생성
            Debug.Log("등급업!");
            // Instantiate(prefabItem[((averageGrade+1) * 5) + Random.Range(0, 5)]);
            Instantiate(prefabItem[(averageGrade * 5) + Random.Range(0, 5)], new Vector3(player.transform.position.x + 1, player.transform.position.y + 1, 0), Quaternion.identity);
        }
        else
        {
            // 등급업 실패
            // 동급 아이템 랜덤 생성
            Instantiate(prefabItem[(averageGrade * 5) + Random.Range(0, 5)], new Vector3(player.transform.position.x + 1, player.transform.position.y + 1, 0), Quaternion.identity);
        }

    }

    public void DiscardItem(int index)
    {
        if (ExchangeItemList.Count != 0) ExchangeItemList.Remove(ExchangeItemList[index]);
    }

    public void AddItem(Slot slot)
    {
        if (ExchangeItemList.Count < 4 ) ExchangeItemList.Add(slot);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        if (ExchangeItemList.Count == 0) transform.parent.transform.Find("ButtonBackground").gameObject.SetActive(false);
        else transform.parent.transform.Find("ButtonBackground").gameObject.SetActive(true);
    }
}
