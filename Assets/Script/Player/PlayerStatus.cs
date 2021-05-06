using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{
    [SerializeField]
    private string occupation;

    private bool acquirable = true;

    [SerializeField]
    List<GameObject> possessItemList = new List<GameObject>();
    //private GameObject[] possessItemList;
    private int possessItemNumber;
    [SerializeField]
    private GameObject[] equipItemList;
    private int equipItemNumber;


    public void check() { Debug.Log(acquirable);  }
    public bool Acquirable { get { return acquirable; } }
    public string Occupation { get { return occupation; } }

    public void EquipItem()
    {
        // 아이템을 장착했을때 아이템 장착 리스트로 옮김
        // 해당 아이템의 옵션을 가지고와서 플레이어스탯에 더하거나 빼줌.
    }

    public void DiscardItem()
    {
        // 아이템을 버리면 소유물품리스트에서 해당 물품 삭제.
    }

    public void AcquireItem(GameObject item)
    {
        // 아이템을 먹으면 소유물품리스트에 추가.
        possessItemList.Add(item);
    }

    // Start is called before the first frame update
    void Start()
    {
        possessItemNumber = 10;
        equipItemNumber = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (possessItemList.Count == possessItemNumber) acquirable = false;
        else acquirable = true;
    }
}
