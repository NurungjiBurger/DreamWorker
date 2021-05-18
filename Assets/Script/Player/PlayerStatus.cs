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
    private int handMoney;
    [SerializeField]
    private GameObject prefabSlot;

    [SerializeField]
    private int experience;
    [SerializeField]
    private int needExperience;
    [SerializeField]
    private int level;

    [SerializeField]
    private int damage;

    private GameObject inventory;
    private int inventoryItemNumber;

    public int Level { get { return level; } }
    public int HandMoney { get { return handMoney; } }
    public string Occupation { get { return occupation; } }
    public bool Acquirable { get { return inventory.GetComponent<Inventory>().Acquirable; } }
    public GameObject Inventory { get { return inventory; } }
    public int Damage { get { return damage; } }

    public Slot CreateItemSlot(GameObject item)
    {
        GameObject tmp;
        tmp = Instantiate(prefabSlot, inventory.transform.Find("Background").transform);

        tmp.GetComponent<Slot>().InsertImage(item);
  
        return tmp.GetComponent<Slot>();
    }

    public void CalCulateExperience(int exp)
    {
        experience += exp;
        needExperience = level * 100;

        if (experience >= needExperience) level++;
    }

    public void AddHandMoney(int money)
    {
        handMoney += money;
    }

    private void CalDamage()
    {
        damage = (int)(power * 1.5);
    }

    public void CalCulateStat(GameObject item, int how)
    {
        int coefficient;

        if (item.GetComponent<ItemStatus>().Occupation == occupation) coefficient = 2;
        else coefficient = 1;

        defenseCapability += item.GetComponent<ItemStatus>().Defense * how;
        MaxHP += item.GetComponent<ItemStatus>().MaxHP * how;
        Power += item.GetComponent<ItemStatus>().Power * how * coefficient;
        if (how == 1)
        {
            if(item.GetComponent<ItemStatus>().AttackSpeed != 0 ) AttackSpeed *= item.GetComponent<ItemStatus>().AttackSpeed;
            if (item.GetComponent<ItemStatus>().JumpPower != 0) jumpPower *= item.GetComponent<ItemStatus>().JumpPower;
            if (item.GetComponent<ItemStatus>().MoveSpeed != 0) moveSpeed *= item.GetComponent<ItemStatus>().MoveSpeed;
        }
        else
        {
            if (item.GetComponent<ItemStatus>().AttackSpeed != 0)  AttackSpeed /= item.GetComponent<ItemStatus>().AttackSpeed;
            if (item.GetComponent<ItemStatus>().JumpPower != 0) jumpPower /= item.GetComponent<ItemStatus>().JumpPower;
            if (item.GetComponent<ItemStatus>().MoveSpeed != 0) moveSpeed /= item.GetComponent<ItemStatus>().MoveSpeed;
        }
        CalDamage();
    }

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        GameObject obj;
        obj = Instantiate(basicItem, new Vector3(-1,-1,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;

        CalCulateExperience(0);
    }
}
