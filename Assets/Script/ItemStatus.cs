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
        // 무기마다 정해진 전용 직업군이 무기를 장착했을 경우 특별한 효과가 생기도록하기위한 체크
        if (player.GetComponent<PlayerStatus>().Occupation == dedicatedOccupation) return 1;
        else return 0;
    }
    public GameObject AttackAnimation()
    {
        return prefabAttack[OccupationCheck()];
        /*
        Debug.Log("공격");
        player.GetComponent<PlayerAttack>().AttackTimer.TimerSetZero();
        Instantiate(prefabAttack[OccupationCheck()], player.transform.position, Quaternion.identity);
        */
    }

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("생성");
        player = GameObject.FindGameObjectWithTag("Player");
        number = Random.Range(0, 1000);
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GoNext) Destroy(gameObject);

        // 플레이어가 공격을 하면 어택애니메이션스타트 실행
    }
}
