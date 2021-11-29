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
    private Sprite[] playerImage;

    public int characterPrfNumber;
    public int index = -1;
    private int inventoryItemNumber;

    private GameObject inventory;

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
            // 공격력, 이동속도, 방어력 소폭 증가
        }
        else if (dataP.thirdEvolution)
        {
            // 공격력, 점프력, 체력 소폭 증가
        }
        else if (dataP.secondEvolution)
        {
            // 공격력, 흡혈 증가
        }
        else if (dataP.firstEvolution)
        {
            // 공격력, 회피 대폭 증가
        }
    }

    // 경험치 계산
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
                    dataP.firstEvolution = true;
                    Evolution();
                    break;
                case 30:
                    dataP.secondEvolution = true;
                    Evolution();
                    break;
                case 60:
                    dataP.thirdEvolution = true;
                    Evolution();
                    break;
                case 100:
                    dataP.forthEvolution = true;
                    Evolution();
                    break;
                default:
                    break;
            }
        }

    }

    // 체력 계산
    public void CalCulateHealth(int Dmg, char oper)
    {
        if (oper == '+')
        {
            dataP.nowHP += Dmg;

            if (dataP.nowHP > dataP.maxHP) dataP.nowHP = dataP.maxHP;
        }
        else if (oper == '-')
        {
            // 방어율 만큼 데미지 감소
            Dmg = (int)(Dmg * ((100 - dataP.defenseRate) / 100));

            dataP.nowHP -= Dmg;
            if (dataP.nowHP < 0)
            {
                Debug.Log("사망했습니다");
                // 저장된 파일 삭제후 메인메뉴로 돌아가기 or 게임 종료
                /*
                Destroy(GameObject.Find("GameController"));
                Destroy(GameObject.Find("Data"));
                GameObject.Find("Data").GetComponent<DataController>().DeleteGameData();
                GameObject.Find("GameController").GetComponent<GameController>().RevertScene("MainMenu");
                /*
                GameObject.Find("Data").GetComponent<DataController>().DeleteGameData();
                GameObject.Find("Data").GetComponent<DataController>().ExitGame();
                */
            }
        }
    }

    // 소지금 계산
    public bool CalCulateHandMoney(int money, char oper)
    {
        if (oper == '+') { dataP.handMoney += money; return true; }
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

    // 기본 데미지 계산
    private void CalDamage()
    {
        damage = (int)(dataP.power * 1.5);
    }

    // 스탯 계산
    public void CalCulateStat(GameObject item, int how)
    {
        // 제대로 생성된 아이템이라면
        if (item.GetComponent<ItemStatus>().itemPrfNumber != -1)
        {
            int coefficient;

            // 전용 아이템의 경우 능력치가 배로 증가함
            if (item.GetComponent<ItemStatus>().Occupation == occupation) coefficient = 2;
            else coefficient = 1;

            dataP.defenseRate += item.GetComponent<ItemStatus>().Data.defenseRate * how;
            dataP.maxHP += item.GetComponent<ItemStatus>().Data.maxHP * how;
            dataP.nowHP += item.GetComponent<ItemStatus>().Data.nowHP * how;
            dataP.power += item.GetComponent<ItemStatus>().Data.power * how * coefficient;
            if (how == 1)
            {
                if (item.GetComponent<ItemStatus>().Data.attackSpeed != 0) dataP.attackSpeed /= item.GetComponent<ItemStatus>().Data.attackSpeed;
                if (item.GetComponent<ItemStatus>().Data.jumpPower != 0) dataP.jumpPower *= item.GetComponent<ItemStatus>().Data.jumpPower;
                if (item.GetComponent<ItemStatus>().Data.moveSpeed != 0) dataP.moveSpeed *= item.GetComponent<ItemStatus>().Data.moveSpeed;
                if (item.GetComponent<ItemStatus>().Data.defenseRate != 0) dataP.defenseRate *= item.GetComponent<ItemStatus>().Data.defenseRate;
                if (item.GetComponent<ItemStatus>().Data.bloodAbsorptionRate != 0) dataP.bloodAbsorptionRate *= item.GetComponent<ItemStatus>().Data.bloodAbsorptionRate;
                if (item.GetComponent<ItemStatus>().Data.evasionRate != 0) dataP.evasionRate *= item.GetComponent<ItemStatus>().Data.evasionRate;
            }
            else
            {
                if (item.GetComponent<ItemStatus>().Data.attackSpeed != 0) dataP.attackSpeed *= item.GetComponent<ItemStatus>().Data.attackSpeed;
                if (item.GetComponent<ItemStatus>().Data.jumpPower != 0) dataP.jumpPower /= item.GetComponent<ItemStatus>().Data.jumpPower;
                if (item.GetComponent<ItemStatus>().Data.moveSpeed != 0) dataP.moveSpeed /= item.GetComponent<ItemStatus>().Data.moveSpeed;
                if (item.GetComponent<ItemStatus>().Data.defenseRate != 0) dataP.defenseRate /= item.GetComponent<ItemStatus>().Data.defenseRate;
                if (item.GetComponent<ItemStatus>().Data.bloodAbsorptionRate != 0) dataP.bloodAbsorptionRate /= item.GetComponent<ItemStatus>().Data.bloodAbsorptionRate;
                if (item.GetComponent<ItemStatus>().Data.evasionRate != 0) dataP.evasionRate /= item.GetComponent<ItemStatus>().Data.evasionRate;
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

        // 데이터 생성
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

        // 플레이어 미니맵 아이콘 생성
        tmp = Instantiate(playerIcon, transform.position, Quaternion.identity);
        tmp.transform.SetParent(GameObject.Find("Canvas").transform.Find("MiniMap").transform.Find("Background"));
        tmp.GetComponent<Icon>().obj = gameObject;

        // 체력바 할당
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

        if (dataP.jumpPower > 10) dataP.jumpPower = 10;
        if (dataP.moveSpeed > 3) dataP.moveSpeed = 3;
    }
}
