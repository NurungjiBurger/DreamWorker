using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerStatus : Status
{
    [SerializeField]
    private string occupation;
    [SerializeField]
    private GameObject basicItem;

    [SerializeField]
    private int damage;

    private GameData data;
    private StatData status;
    public int characterPrfNumber;

    private GameObject inventory;
    private int inventoryItemNumber;

    public StatData Status { get { return status; } }
    public int Level { get { return data.player.level; } }
    public int HandMoney { get { return data.player.handMoney; } }
    public string Occupation { get { return occupation; } }
    public bool Acquirable { get { return inventory.GetComponent<Inventory>().Acquirable; } }
    public GameObject Inventory { get { return inventory; } }
    public int Damage { get { return damage; } }

    public void CalCulateExperience(int exp)
    {
        data.player.experience += exp;
        data.player.needExperience = data.player.level * 100;

        if (data.player.experience >= data.player.needExperience)
        {
            status.power += 5;
            data.player.level++;

            switch(data.player.level)
            {
                case 10:
                    data.player.firstTurn = true;
                    Turn();
                    break;
                case 30:
                    data.player.secondTurn = true;
                    Turn();
                    break;
                case 60:
                    data.player.thirdTurn = true;
                    Turn();
                    break;
                case 100:
                    data.player.forthTurn = true;
                    Turn();
                    break;
                default:
                    break;
            }
        }

    }

    public void AddHandMoney(int money)
    {
        data.player.handMoney += money;
    }

    private void Turn()
    {
        if(data.player.forthTurn)
        {
            status.power += 50;
        }
        else if (data.player.thirdTurn)
        {

        }
        else if (data.player.secondTurn)
        {

        }
        else if (data.player.firstTurn)
        {

        }
    }

    private void CalDamage()
    {
        damage = (int)(status.power * 1.5);
    }

    public void CalCulateStat(GameObject item, int how)
    {
        int coefficient;

        if (item.GetComponent<ItemStatus>().Occupation == occupation) coefficient = 2;
        else coefficient = 1;

        status.defenseRate += item.GetComponent<ItemStatus>().Status.defenseRate * how;
        status.maxHP += item.GetComponent<ItemStatus>().Status.maxHP * how;
        status.power += item.GetComponent<ItemStatus>().Status.power * how * coefficient;
        if (how == 1)
        {
            if(item.GetComponent<ItemStatus>().Status.attackSpeed != 0 ) status.attackSpeed *= item.GetComponent<ItemStatus>().Status.attackSpeed;
            if (item.GetComponent<ItemStatus>().Status.jumpPower != 0) status.jumpPower *= item.GetComponent<ItemStatus>().Status.jumpPower;
            if (item.GetComponent<ItemStatus>().Status.moveSpeed != 0) status.moveSpeed *= item.GetComponent<ItemStatus>().Status.moveSpeed;
        }
        else
        {
            if (item.GetComponent<ItemStatus>().Status.attackSpeed != 0) status.attackSpeed /= item.GetComponent<ItemStatus>().Status.attackSpeed;
            if (item.GetComponent<ItemStatus>().Status.jumpPower != 0) status.jumpPower /= item.GetComponent<ItemStatus>().Status.jumpPower;
            if (item.GetComponent<ItemStatus>().Status.moveSpeed != 0) status.moveSpeed /= item.GetComponent<ItemStatus>().Status.moveSpeed;
        }
        CalDamage();
    }

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;        
    }

    private void Start()
    {
        if (data.player == null)
        {
            int[] arr = new int[2];
            float[] arr2 = new float[6];
            arr[0] = maxHP; arr[1] = power;
            arr2[0] = defenseRate; arr2[1] = jumpPower; arr2[2] = moveSpeed; arr2[3] = attackSpeed; arr2[4] = bloodAbsorptionRate; arr2[5] = evasionRate;
            data.player = new PlayerData(characterPrfNumber, arr, arr2);

            status = data.player.status;

            GameObject tmp;
            tmp = Instantiate(basicItem, new Vector3(-1, -1, 0), Quaternion.identity);
            tmp.GetComponent<ItemStatus>().itemPrfNumber = 3;
        }
        else
        {
            status = data.player.status;
        }
    }

    private void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (data == null) data = GameObject.Find("Data").GetComponent<DataController>().GameData;
        else data.player.SetPosition(transform.position);

        CalCulateExperience(0);
    }
}
