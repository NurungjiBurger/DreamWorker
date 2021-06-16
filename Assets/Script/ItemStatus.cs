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

    private GameObject inventory;
    private GameObject inspector;

    public int itemPrfNumber = -1;
    public int index = -1;
    public bool done = false;

    private GameData data;
    private StatData status;
    private GameObject player;

    public StatData Status { get { return status; } }
    public int CursedRate { get { return data.items[index].cursedRate; } set { data.items[index].cursedRate = value; } }
    public int ItemGrade { get { return (int)grade; } }
    public string MountingPart { get { return mountingPart; } }
    public bool IsMount { get { return data.items[index].isMount; } set { data.items[index].isMount = value; } }
    public string Occupation { get { return dedicatedOccupation; } }

    public void DestoryAll()
    {
        data.items.Remove(data.items[index]);
        Destroy(gameObject);
    }

    private void CurseApply()
    {
        switch(MountingPart)
        {
            case "Head":
                Debug.Log("체력 감소" + (1 - ((float)data.items[index].cursedRate / 100)).ToString());
                status.maxHP = (int)((1.0f - ((float)data.items[index].cursedRate / 100)) * (float)status.maxHP);
                break;
            case "Hand":
                Debug.Log("공격속도 감소" + (1 - ((float)data.items[index].cursedRate / 100)).ToString());
                status.attackSpeed = (1.0f - ((float)data.items[index].cursedRate / 100)) * status.attackSpeed;
                break;
            case "Foot":
                Debug.Log("이동속도 감소" + (1 - ((float)data.items[index].cursedRate / 100)).ToString());
                status.moveSpeed = (1.0f - ((float)data.items[index].cursedRate / 100)) * status.moveSpeed;
                break;
            case "Body":
                Debug.Log("점프력 감소" + (1 - ((float)data.items[index].cursedRate / 100)).ToString());
                status.jumpPower = (1.0f - ((float)data.items[index].cursedRate / 100)) * status.jumpPower;
                break;
            case "Weapon":
                Debug.Log("공격력 감소" + (1 - ((float)data.items[index].cursedRate / 100)).ToString());
                status.power = (int)((1.0f - ((float)data.items[index].cursedRate / 100)) * (float)status.power);
                break;
            default:
                break;
        }
    }

    private void StatUP()
    {
        status.maxHP = (int)(status.maxHP * 1.5f);
        status.power = (int)(status.power * 1.3f);
        status.jumpPower = status.jumpPower * 1.2f;
        status.moveSpeed = status.moveSpeed * 1.2f;
        status.attackSpeed = status.attackSpeed * 1.1f;
        status.defenseRate = status.defenseRate * 1.2f;
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
    }

    private void Start()
    {
        inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;

        player = GameObject.FindGameObjectWithTag("Player");
        if (index == -1 && itemPrfNumber != -1)
        {
            int[] arr = new int[2];
            float[] arr2 = new float[6];
            arr[0] = maxHP; arr[1] = power;
            arr2[0] = defenseRate; arr2[1] = jumpPower; arr2[2] = moveSpeed; arr2[3] = attackSpeed; arr2[4] = bloodAbsorptionRate; arr2[5] = evasionRate;
            index = data.items.Count;

            data.items.Add(new ItemData(itemPrfNumber, index, arr, arr2));

            data.items[index].cursedRate = Random.Range(0, 50); // 저주율 수치 조정 필요
            data.items[index].isMount = false;
            data.items[index].SetPosition(transform.position);

            status = data.items[index].status;

            CurseApply();

            if (player.GetComponent<PlayerStatus>().Occupation == dedicatedOccupation) StatUP();
        }
        else
        {
            itemPrfNumber = data.items[index].itemPrfNumber;
            status = data.items[index].status;

            CursedRate = data.items[index].cursedRate;

            if (data.items[index].isMount) inspector.GetComponent<Inspector>().AddToInspector(GameObject.Find("GameController").GetComponent<GameController>().CreateItemSlot(gameObject));
            else inventory.GetComponent<Inventory>().AddToInventory(GameObject.Find("GameController").GetComponent<GameController>().CreateItemSlot(gameObject));
        }
        done = true;
    }

    private void Update()
    {
        if (GameObject.Find("GameController").GetComponent<GameController>().GoNext) Destroy(gameObject);
    }
}
