using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{
    [SerializeField]
    private string occupation;


    private GameObject[] possessItemList;
    private GameObject[] equipItemList;


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

    public void AcquireItem()
    {
        // �������� ������ ������ǰ����Ʈ�� �߰�.
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
