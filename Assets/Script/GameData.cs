using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class GameData
{
    // gmaedata
    public int stageNumber;
    public bool stageEntrance;
    public int subStageNumber;
    public bool goNext;

    // player
    public PlayerData player;

    // player UI
    public List<Slot> playerEquipItemList = new List<Slot>();
    public List<Slot> playerPossessItemList = new List<Slot>();

    public GameObject inspector;
    public GameObject inventory;

    // item
    public List<ItemData> items = new List<ItemData>();

    // monsters
    public List<MonsterData> monsters = new List<MonsterData>();

    // map
    public List<MapData> maps = new List<MapData>();

    public void Test()
    {
        // 소지금 , 레벨 , 경험치, 필요경험치
        //if (player != null) Debug.Log(player.handMoney + "  " + player.level + "  " + player.experience + "  " + player.needExperience);

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
    public int characterPrfNumber;

    public float[] pos = new float[3];
    
    // inven
    public int handMoney;
    public int level;
    public int experience;
    public int needExperience;

    // stat
    

    public PlayerData(int num)
    {
        characterPrfNumber = num;

        handMoney = 0;
        level = 0;
        experience = 0;
        needExperience = 0;
    }

    public void SetPosition(Vector3 position)
    {
        pos[0] = position.x;
        pos[1] = position.y;
        pos[2] = position.z;
    }

    public Vector3 Position()
    {
        Vector3 tmp = new Vector3(pos[0], pos[1], pos[2]);
        return tmp;
    }

}

[Serializable]
public class ItemData
{
    public int itemPrfNumber;

    public float[] pos = new float[3];

    public int index;
    public int cursedRate;
    public bool isMount;

    public ItemData(int num, int idx)
    {
        itemPrfNumber = num;
        index = idx;
    }

    public void SetPosition(Vector3 position)
    {
        pos[0] = position.x;
        pos[1] = position.y;
        pos[2] = position.z;
    }

    public Vector3 Position()
    {
        Vector3 tmp = new Vector3(pos[0], pos[1], pos[2]);
        return tmp;
    }
}

[Serializable]
public class MonsterData
{
    public int monsterPrfNumber;

    public float[] pos = new float[3];

    public bool isBoss;
    public int index;


    public MonsterData(int num, int idx)
    {
        monsterPrfNumber = num;
        index = idx;
    }

    public void SetPosition(Vector3 position)
    {
        pos[0] = position.x;
        pos[1] = position.y;
        pos[2] = position.z;
    }

    public Vector3 Position()
    {
        Vector3 tmp = new Vector3(pos[0], pos[1], pos[2]);
        return tmp;
    }
}

[Serializable]
public class MapData
{
    public int mapPrfNumber;

    public float[] pos = new float[3];

    public int selectRoomIndex;
    public int index;
    public int portalDirection;
    public bool isClear;

    public MapData(int num, int idx, int dir, int sel)
    {
        mapPrfNumber = num;
        index = idx;
        portalDirection = dir;
        selectRoomIndex = sel;
    }

    public void SetPosition(Vector3 position)
    {
        pos[0] = position.x;
        pos[1] = position.y;
        pos[2] = position.z;
    }

    public Vector3 Position()
    {
        Vector3 tmp = new Vector3(pos[0], pos[1], pos[2]);
        return tmp;
    }
}


