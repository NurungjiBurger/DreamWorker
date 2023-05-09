using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerStatus : Status
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private int basicItemNum;
    [SerializeField]
    private string occupation;
    [SerializeField]
    private GameObject handBone;
    [SerializeField]
    private GameObject playerIcon;
    [SerializeField]
    private GameObject prefabTimer;
    [SerializeField]
    private GameObject levelUP;
    [SerializeField]
    private Sprite[] playerImage;

    public int characterPrfNumber;
    public int index = -1;
    private int inventoryItemNumber;

    private GameObject inventory;
    private Timer levelTimer;

    RectTransform hpBar;
    private Image nowHPBar;
    private TextMeshProUGUI textHp;
    private GameData data;
    private Data dataP = null;


    public bool Acquirable { get { return inventory.GetComponent<Inventory>().Acquirable; } }
    public int Level { get { return dataP.level; } }
    public int HandMoney { get { return dataP.handMoney; } }
    public int Damage { get { return damage; } }
    public string Occupation { get { return occupation; } }
    public GameObject Inventory { get { return inventory; } }
    public GameObject HandBone { get { return handBone; } }
    public Sprite PlayerImage { get { return playerImage[1]; } }
    public Data Data { get { return dataP; } }

    private void Evolution()
    {
        if (dataP.forthEvolution)
        {
            // ���ݷ�, ȸ�� ���� ����
            dataP.power += 20;
            dataP.evasionRate += 20;
        }
        else if (dataP.thirdEvolution)
        {
            // ���ݷ�, ���� ����
            dataP.power += 10;
            dataP.bloodAbsorptionRate += 10;
        }
        else if (dataP.secondEvolution)
        {
            // ���ݷ�, ������, ü�� ���� ����
            dataP.power += 5;
            dataP.jumpPower += 0.05f;
            dataP.maxHP += 10;
            dataP.nowHP += 10;
        }
        else if (dataP.firstEvolution)
        {
            // ���ݷ�, �̵��ӵ�, ���� ���� ����
            dataP.power += 5;
            dataP.moveSpeed += 0.05f;
            dataP.defenseRate += 5;
        }
    }

    // ����ġ ���
    public void CalCulateExperience(int exp)
    {
        dataP.experience += exp;
        //dataP.needExperience = dataP.level * 10;
       dataP.needExperience = dataP.level * 100;

        if (dataP.experience >= dataP.needExperience)
        {
            dataP.power += 1;
            dataP.level++;
            dataP.experience = 0;

            levelUP.SetActive(true);
            levelTimer.TimerSetZero();

            if (dataP.level >= 10 && !dataP.firstEvolution)
            {
                dataP.firstEvolution = true; Evolution();
            }
            else if (dataP.level >= 30 && !dataP.secondEvolution)
            {
                dataP.secondEvolution = true; Evolution();
            }
            else if (dataP.level >= 60 && !dataP.thirdEvolution)
            {
                dataP.thirdEvolution = true; Evolution();
            }
            else if (dataP.level >= 100 && !dataP.forthEvolution)
            {
                dataP.forthEvolution = true; Evolution();
            }
        }

    }

    // ü�� ���
    public void CalCulateHealth(int Dmg, char oper)
    {
        if (oper == '+')
        {
            dataP.nowHP += Dmg;

            if (dataP.nowHP > dataP.maxHP) dataP.nowHP = dataP.maxHP;
        }
        else if (oper == '-')
        {
            // ����� ��ŭ ������ ����
            Dmg = (int)(Dmg * ((100 - dataP.defenseRate) / 100));

            dataP.nowHP -= Dmg;
            if (dataP.nowHP < 0)
            {
                //Debug.Log("����߽��ϴ�");

                // Destroy(GameObject.Find("GameController"));
                // Destroy(GameObject.Find("Data"));
                data.winOrLose = "Defeat ...";

                GameObject.Find("Data").GetComponent<DataController>().DeleteGameData();
                GameObject.Find("GameController").GetComponent<GameController>().RevertScene("Result");
                
                //GameObject.Find("Data").GetComponent<DataController>().DeleteGameData();
                //GameObject.Find("Data").GetComponent<DataController>().ExitGame();
                
            }
        }
    }

    // ������ ���
    public bool CalCulateHandMoney(int money, char oper)
    {
        if (oper == '+') 
        { 
            dataP.handMoney += money;
            data.obtainedGold += money;

            return true; 
        }
        else if (oper == '-')
        {
            if (dataP.handMoney >= money)
            {
                dataP.handMoney -= money;
                return true;
            }
            else return false;
        }
        else return false;
    }

    // �⺻ ������ ���
    private void CalDamage()
    {
        damage = (int)(dataP.power * 1.5);
    }

    // ���� ���
    public void CalCulateStat(GameObject item, int how)
    {
        // ����� ������ �������̶�� �����ճѹ��� ���ϰ� ������
        if (item.GetComponent<ItemStatus>().itemPrfNumber != -1)
        {

            dataP.defenseRate += item.GetComponent<ItemStatus>().Data.defenseRate * how;
            dataP.maxHP += item.GetComponent<ItemStatus>().Data.maxHP * how;
            dataP.nowHP += item.GetComponent<ItemStatus>().Data.nowHP * how;
            dataP.power += item.GetComponent<ItemStatus>().Data.power * how;
            dataP.evasionRate += item.GetComponent<ItemStatus>().Data.evasionRate * how;
            dataP.bloodAbsorptionRate += item.GetComponent<ItemStatus>().Data.bloodAbsorptionRate * how;
            dataP.attackSpeed -= item.GetComponent<ItemStatus>().Data.attackSpeed * how;
            dataP.jumpPower += item.GetComponent<ItemStatus>().Data.jumpPower * how;
            dataP.moveSpeed += item.GetComponent<ItemStatus>().Data.moveSpeed * how;

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

        // ������ ����
        if (index == -1)
        {
            int[] arr = new int[2];
            float[] arr2 = new float[7];
            arr[0] = maxHP;
            arr2[0] = defenseRate; arr2[1] = jumpPower; arr2[2] = moveSpeed; arr2[3] = attackSpeed; arr2[4] = bloodAbsorptionRate; arr2[5] = evasionRate; arr2[6] = power;

            index = data.datas.Count;

            data.datas.Add(new Data("Player", characterPrfNumber, index, arr, arr2, -1, -1, false));
            dataP = data.datas[index];

            tmp = Instantiate(GameObject.Find("GameController").GetComponent<GameController>().PrefabReturn("Item", basicItemNum), new Vector3(-1, -1, 0), Quaternion.identity);
            tmp.GetComponent<ItemStatus>().itemPrfNumber = basicItemNum;
        }
        else
        {
            dataP = data.datas[index];

        }

        // Ÿ�̸� ����
        levelTimer = Instantiate(prefabTimer).GetComponent<Timer>();
        levelTimer.SetCooldown(1.0f);

        // �÷��̾� �̴ϸ� ������ ����
        tmp = Instantiate(playerIcon, transform.position, Quaternion.identity);
        tmp.transform.SetParent(GameObject.Find("Canvas").transform.Find("MiniMap").transform.Find("Background"));
        tmp.GetComponent<Icon>().obj = gameObject;

        // ü�¹� �Ҵ�
        hpBar = GameObject.Find("Canvas").transform.Find("PlayerHPBar").GetComponent<RectTransform>();
        nowHPBar = hpBar.Find("php_bar").GetComponent<Image>();
        textHp = nowHPBar.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    }

    private void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (dataP == null) dataP = data.datas[index];
        else dataP.SetPosition(transform.position);

        textHp.text = dataP.nowHP.ToString() + "    /    " + dataP.maxHP.ToString();
        nowHPBar.fillAmount = (float)dataP.nowHP / (float)dataP.maxHP;

        // ���� ����
        if (dataP.jumpPower > 10) dataP.jumpPower = 10;
        if (dataP.moveSpeed > 4.5) dataP.moveSpeed = 4.5f;
        if (dataP.defenseRate > 90) dataP.defenseRate = 90;
        if (dataP.evasionRate > 100) dataP.evasionRate = 100;
        if (dataP.bloodAbsorptionRate > 100) dataP.bloodAbsorptionRate = 100;

        // ������ ȿ�� ���
        if (!levelUP) levelUP = GameObject.Find("Canvas").transform.Find("LevelUP").gameObject;
        else
        {
            if (levelUP.activeSelf) if (levelTimer.CooldownCheck()) levelUP.SetActive(false);
        }

        CalCulateExperience(0);
        /*
        Debug.Log(dataP.defenseRate + " / " +
        dataP.maxHP + " / " +
        dataP.power + " / " +
        dataP.evasionRate + " / " +
        dataP.bloodAbsorptionRate + " / " +
        dataP.attackSpeed + " / " +
        dataP.jumpPower + " / " +
        dataP.moveSpeed);
        */
    }
}
