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
        // �������� ���������� ������ ���� ����Ʈ�� �ű�
        // �ش� �������� �ɼ��� ������ͼ� �÷��̾�ȿ� ���ϰų� ����.
    }

    public void DiscardItem()
    {
        // �������� ������ ������ǰ����Ʈ���� �ش� ��ǰ ����.
    }

    public void AcquireItem(GameObject item)
    {
        // �������� ������ ������ǰ����Ʈ�� �߰�.
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
