using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatus : Status
{
    [SerializeField]
    private GameObject[] prefabAttack;
    [SerializeField]
    private string dedicatedOccupation;
    [SerializeField]
    private string mountingPart;

    private List<GameObject> equipList = new List<GameObject>();


    private int number;

    private float cursedRate;
    private GameObject player;

    private bool isMount = false;
    private bool isAttack = false;

    public string MountingPart { get { return mountingPart; } }
    public int Number { get { return number; } }
    public bool IsAttack { get { return isAttack; } }
    public float CurseRate { get { return cursedRate; } set { cursedRate = value; } }
    public string Occupation { get { return dedicatedOccupation; } }

    public void EquipCheck(List<GameObject> list)
    {
        equipList = list;
        for(int i=equipList.Count-1;i>=0;i--)
        {
            if(equipList[i].GetComponent<ItemStatus>().Name == Name && equipList[i].GetComponent<ItemStatus>().Number == number)
            {
                isMount = true;
            }
        }
    }

    private int OccupationCheck()
    {
        // ���⸶�� ������ ���� �������� ���⸦ �������� ��� Ư���� ȿ���� ���⵵���ϱ����� üũ
        if (player.GetComponent<PlayerStatus>().Occupation == dedicatedOccupation) return 1;
        else return 0;
    }
    public GameObject AttackAnimation()
    {
        return prefabAttack[OccupationCheck()];
        /*
        Debug.Log("����");
        player.GetComponent<PlayerAttack>().AttackTimer.TimerSetZero();
        Instantiate(prefabAttack[OccupationCheck()], player.transform.position, Quaternion.identity);
        */
    }

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("����");
        player = GameObject.FindGameObjectWithTag("Player");
        number = Random.Range(0, 1000);
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GoNext) Destroy(gameObject);

        // �÷��̾ ������ �ϸ� ���þִϸ��̼ǽ�ŸƮ ����
    }
}
