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
        // 1���� ��޾� 1��
        // 2�� 20��
        // 3�� 40��
        // 4�� 60��
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

        // ���� ������� �������� ��ް� ������ ���
        int averageGrade = 0;
        int averageCursedRate = 0;

        for (int i = 0; i < ExchangeItemList.Count; i++) { averageGrade += ExchangeItemList[i].SlotItem.GetComponent<ItemStatus>().ItemGrade; averageCursedRate += ExchangeItemList[i].SlotItem.GetComponent<ItemStatus>().CursedRate; }

        averageGrade /= ExchangeItemList.Count;
        averageCursedRate /= ExchangeItemList.Count;

        Debug.Log(averageCursedRate + "   " + averageGrade);

        Slot tmp;

        // �Ҹ�� �����۵� ����
        for (int i = 0; i < ExchangeItemList.Count; i++)
        {
            tmp = ExchangeItemList[i];
            ExchangeItemList.Remove(ExchangeItemList[i]);
            Destroy(tmp.SlotItem.gameObject);
            Destroy(tmp.gameObject);
        }

        if (Random.Range(0,101) <= success)
        {
            // ��޾� ����
            // ���� ��� ������ ���� ����
            Debug.Log("��޾�!");
            // Instantiate(prefabItem[((averageGrade+1) * 5) + Random.Range(0, 5)]);
            Instantiate(prefabItem[(averageGrade * 5) + Random.Range(0, 5)], new Vector3(player.transform.position.x + 1, player.transform.position.y + 1, 0), Quaternion.identity);
        }
        else
        {
            // ��޾� ����
            // ���� ������ ���� ����
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
