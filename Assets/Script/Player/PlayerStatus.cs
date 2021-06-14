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
    private GameObject prefabSlot;

    [SerializeField]
    private int damage;

    private GameData data;

    private GameObject inventory;
    private int inventoryItemNumber;

    public int Level { get { return data.player.level; } }
    public int HandMoney { get { return data.player.handMoney; } }
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
        data.player.experience += exp;
        data.player.needExperience = data.player.level * 100;

        if (data.player.experience >= data.player.needExperience) data.player.level++;

    }

    public void AddHandMoney(int money)
    {
        data.player.handMoney += money;
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

    private void Start()
    {
        data = GameObject.Find("Data").GetComponent<DataController>().GameData;

        if (data.player.level == 0) data.player.level = 1;
        GameObject obj;
        obj = Instantiate(basicItem, new Vector3(-1,-1,0), Quaternion.identity);
    }

    private void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (data == null) data = GameObject.Find("Data").GetComponent<DataController>().GameData;

        CalCulateExperience(0);
    }
}
