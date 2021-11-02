using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerStatus : Status
{
    [SerializeField]
    private Sprite[] playerImage;
    [SerializeField]
    private string occupation;
    [SerializeField]
    private int basicItemNum;
    [SerializeField]
    private GameObject handBone;
    [SerializeField]
    private GameObject playerIcon;

    [SerializeField]
    private int damage;

    private GameData data;
    private Data dataP = null;
    public int characterPrfNumber;
    public int index = -1;

    private GameObject inventory;
    private int inventoryItemNumber;

    public Sprite PlayerImage { get { return playerImage[1]; } }
    public Data Data { get { return dataP; } }
    public int Level { get { return dataP.level; } }
    public int HandMoney { get { return dataP.handMoney; } }
    public string Occupation { get { return occupation; } }
    public bool Acquirable { get { return inventory.GetComponent<Inventory>().Acquirable; } }
    public GameObject Inventory { get { return inventory; } }
    public int Damage { get { return damage; } }
    public GameObject HandBone { get { return handBone; } }

    public void CalCulateExperience(int exp)
    {
        dataP.experience += exp;
        dataP.needExperience = dataP.level * 100;

        if (dataP.experience >= dataP.needExperience)
        {
            dataP.power += 5;
            dataP.level++;

            switch(dataP.level)
            {
                case 10:
                    dataP.firstTurn = true;
                    Turn();
                    break;
                case 30:
                    dataP.secondTurn = true;
                    Turn();
                    break;
                case 60:
                    dataP.thirdTurn = true;
                    Turn();
                    break;
                case 100:
                    dataP.forthTurn = true;
                    Turn();
                    break;
                default:
                    break;
            }
        }

    }

    public void AddHandMoney(int money)
    {
        dataP.handMoney += money;
    }

    private void Turn()
    {
        if(dataP.forthTurn)
        {
            dataP.power += 50;
        }
        else if (dataP.thirdTurn)
        {

        }
        else if (dataP.secondTurn)
        {

        }
        else if (dataP.firstTurn)
        {

        }
    }

    private void CalDamage()
    {
        damage = (int)(dataP.power * 1.5);
    }

    public void CalCulateStat(GameObject item, int how)
    {
        if (item.GetComponent<ItemStatus>().itemPrfNumber != -1)
        {
            int coefficient;

            if (item.GetComponent<ItemStatus>().Occupation == occupation) coefficient = 2;
            else coefficient = 1;

            dataP.defenseRate += item.GetComponent<ItemStatus>().Data.defenseRate * how;
            dataP.maxHP += item.GetComponent<ItemStatus>().Data.maxHP * how;
            dataP.power += item.GetComponent<ItemStatus>().Data.power * how * coefficient;
            if (how == 1)
            {
                if (item.GetComponent<ItemStatus>().Data.attackSpeed != 0) dataP.attackSpeed /= item.GetComponent<ItemStatus>().Data.attackSpeed;
                if (item.GetComponent<ItemStatus>().Data.jumpPower != 0) dataP.jumpPower *= item.GetComponent<ItemStatus>().Data.jumpPower;
                if (item.GetComponent<ItemStatus>().Data.moveSpeed != 0) dataP.moveSpeed *= item.GetComponent<ItemStatus>().Data.moveSpeed;
            }
            else
            {
                if (item.GetComponent<ItemStatus>().Data.attackSpeed != 0) dataP.attackSpeed *= item.GetComponent<ItemStatus>().Data.attackSpeed;
                if (item.GetComponent<ItemStatus>().Data.jumpPower != 0) dataP.jumpPower /= item.GetComponent<ItemStatus>().Data.jumpPower;
                if (item.GetComponent<ItemStatus>().Data.moveSpeed != 0) dataP.moveSpeed /= item.GetComponent<ItemStatus>().Data.moveSpeed;
            }
        }
        CalDamage();
    }

    private void Awake()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;        
    }

    private void Start()
    {
        GameObject tmp;

        if (index == -1)
        {
            int[] arr = new int[2];
            float[] arr2 = new float[7];
            arr[0] = maxHP;
            arr2[0] = defenseRate; arr2[1] = jumpPower; arr2[2] = moveSpeed; arr2[3] = attackSpeed; arr2[4] = bloodAbsorptionRate; arr2[5] = evasionRate; arr2[6] = power;

            index = data.datas.Count;

            data.datas.Add(new Data("Player", characterPrfNumber, index, arr, arr2, -1, -1));
            dataP = data.datas[index];

            tmp = Instantiate(GameObject.Find("GameController").GetComponent<GameController>().PrefabReturn("Item", basicItemNum), new Vector3(-1, -1, 0), Quaternion.identity);
            tmp.GetComponent<ItemStatus>().itemPrfNumber = basicItemNum;
        }
        else
        {
            dataP = data.datas[index];

        }

        tmp = Instantiate(playerIcon, transform.position, Quaternion.identity);
        tmp.transform.SetParent(GameObject.Find("Canvas").transform.Find("MiniMap").transform.Find("Background"));
        tmp.GetComponent<Icon>().obj = gameObject;

    }

    private void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (dataP == null) dataP = data.datas[index];
        else dataP.SetPosition(transform.position);

        CalCulateExperience(0);

        Debug.Log(dataP.maxHP);
        //Debug.Log(dataP.attackSpeed);
    }
}
