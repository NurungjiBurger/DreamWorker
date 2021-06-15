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

    public bool isFirst = true;
    public int itemPrfNumber;
    public int index;

    private GameData data;
    private GameObject player;

    public int CursedRate { get { return data.items[index].cursedRate; } set { data.items[index].cursedRate = value; } }
    public int ItemGrade { get { return (int)grade; } }
    public string MountingPart { get { return mountingPart; } }
    public bool IsMount { get { return data.items[index].isMount; } set { data.items[index].isMount = value; } }
    public string Occupation { get { return dedicatedOccupation; } }

    private void CurseApply()
    {
        switch(MountingPart)
        {
            case "Head":
                Debug.Log("체력 감소" + (1 - ((float)data.items[index].cursedRate / 100)).ToString());
                maxHP = (int)((1.0f - ((float)data.items[index].cursedRate / 100)) * (float)maxHP);
                break;
            case "Hand":
                Debug.Log("공격속도 감소" + (1 - ((float)data.items[index].cursedRate / 100)).ToString());
                attackSpeed = (1.0f - ((float)data.items[index].cursedRate / 100)) * attackSpeed;
                break;
            case "Foot":
                Debug.Log("이동속도 감소" + (1 - ((float)data.items[index].cursedRate / 100)).ToString());
                moveSpeed = (1.0f - ((float)data.items[index].cursedRate / 100)) * moveSpeed;
                break;
            case "Body":
                Debug.Log("점프력 감소" + (1 - ((float)data.items[index].cursedRate / 100)).ToString());
                jumpPower = (1.0f - ((float)data.items[index].cursedRate / 100)) * jumpPower;
                break;
            case "Weapon":
                Debug.Log("공격력 감소" + (1 - ((float)data.items[index].cursedRate / 100)).ToString());
                power = (int)((1.0f - ((float)data.items[index].cursedRate / 100)) * (float)power);
                break;
            default:
                break;
        }
    }

    private void StatUP()
    {
        maxHP = (int)(maxHP * 1.5f);
        power = (int)(power * 1.3f);
        jumpPower = jumpPower * 1.2f;
        moveSpeed = moveSpeed * 1.2f;
        attackSpeed = attackSpeed * 1.1f;
        defenseCapability = defenseCapability * 1.2f;
    }

    private int OccupationCheck()
    {
        if (player.GetComponent<PlayerStatus>().Occupation == dedicatedOccupation) return 1;
        else return 0;
    }
    public GameObject GetAttackAnimation()
    {
        return prefabAttack[OccupationCheck()];
    }

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;
        if (isFirst)
        {
            data.items[index].cursedRate = Random.Range(0, 50); // 저주율 수치 조정 필요
            data.items[index].isMount = false;
            data.items[index].SetPosition(transform.position);
        }
        else
        {
            CursedRate = data.items[index].cursedRate;
            
            // 아이템 슬롯화 후 인벤, 장비창에 넣어야함.
        }

        CurseApply();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<PlayerStatus>().Occupation == dedicatedOccupation) StatUP();
    }

    private void Update()
    {
        if (GameObject.Find("GameController").GetComponent<GameController>().GoNext) Destroy(gameObject);
    }
}
