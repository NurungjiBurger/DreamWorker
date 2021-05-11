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

    private GameObject inventory;
    private int inventoryItemNumber;
    public string Occupation { get { return occupation; } }
    public bool Acquirable { get { return inventory.GetComponent<Inventory>().Acquirable; } }
    public GameObject Inventory { get { return inventory; } }

    public void CalCulateStat(GameObject item, int how)
    {
        defenseCapability += item.GetComponent<ItemStatus>().Defense * how;
        MaxHP += item.GetComponent<ItemStatus>().MaxHP * how;
        Power += item.GetComponent<ItemStatus>().Power * how;
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

    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj;
        obj = Instantiate(basicItem, new Vector3(-1,-1,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory(Clone)").transform.Find("InventoryBackground").gameObject;
        
    }
}
