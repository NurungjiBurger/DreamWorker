using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatus : Status
{
    enum Grade { Normal, Rare, Epic, Unique, Legendary };

    [SerializeField]
    private GameObject[] prefabAttack;
    [SerializeField]
    private string dedicatedOccupation;
    [SerializeField]
    private string mountingPart;
    [SerializeField]
    private Grade grade;
    [SerializeField]
    private int cursedRate;

    private GameObject player;
    private bool isAttack = false;
    private bool isMount = false;

    public int CursedRate { get { return cursedRate; } set { cursedRate = value; } }
    public int ItemGrade { get { return (int)grade; } }
    public string MountingPart { get { return mountingPart; } }
    public bool IsAttack { get { return isAttack; } }
    public bool IsMount { get { return isMount; } set { isMount = value; } }
    public string Occupation { get { return dedicatedOccupation; } }

    private int OccupationCheck()
    {
        // 무기마다 정해진 전용 직업군이 무기를 장착했을 경우 특별한 효과가 생기도록하기위한 체크
        if (player.GetComponent<PlayerStatus>().Occupation == dedicatedOccupation) return 1;
        else return 0;
    }
    public GameObject GetAttackAnimation()
    {
        return prefabAttack[OccupationCheck()];
    }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        cursedRate = Random.Range(0, 70); // 저주율 수치 조정 필요
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GoNext) Destroy(gameObject);
    }
}
