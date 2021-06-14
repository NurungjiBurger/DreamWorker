using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class GameData
{

    // player
    public PlayerData player = new PlayerData();


    // player UI
    public List<Slot> playerEquipItemList = new List<Slot>();
    public List<Slot> playerPossessItemList = new List<Slot>();

    public GameObject inspector;
    public GameObject inventory;

    // item
    public List<GameObject> item = new List<GameObject>();

    // monsters

    public void diff()
    {
        Debug.Log(JsonUtility.ToJson(new Serialization<GameObject>(item)));
    }    


    public void Test()
    {
        // 소지금 , 레벨 , 경험치, 필요경험치
        if (player != null) Debug.Log(player.handMoney + "  " + player.level + "  " + player.experience + "  " + player.needExperience);

    }

    public void DataRestore()
    {

    }
}

[Serializable]
public class Serialization<T>
{
    [SerializeField]
    List<T> target;
    public List<T> ToList() { return target; }

    public Serialization(List<T> target)
    {
        this.target = target;
    }
}

[Serializable]
public class PlayerData
{
    public float[] pos = new float[3];
    
    public int handMoney;
    public int level;
    public int experience;
    public int needExperience;
    
}

[Serializable]
public class SlotData
{

}


