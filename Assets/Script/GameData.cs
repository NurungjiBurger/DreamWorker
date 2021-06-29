using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class GameData
{
    // gamedata
    public int stageNumber;
    public bool stageEntrance;
    public int subStageNumber;
    public bool goNext;

    public List<Data> datas = new List<Data>();
}

[Serializable]
public class Data
{
    // common
    public string structName;
    public float[] pos = new float[3];
    public int prfNumber;
    public int index;

    //////////////////////////////////////////////// // player

    public int handMoney;
    public int level;
    public int experience;
    public int needExperience;

    public bool firstTurn;
    public bool secondTurn;
    public bool thirdTurn;
    public bool forthTurn;

    //////////////////////////////////////////////// // item

    public int cursedRate;
    public bool isMount;
    public bool isAcquired;

    //////////////////////////////////////////////// // stat

    public int maxHP;
    public int nowHP;
    public int power;
    public float defenseRate;
    public float jumpPower;
    public float moveSpeed;
    public float attackSpeed;
    public float bloodAbsorptionRate;
    public float evasionRate;

    //////////////////////////////////////////////// // monster data

    public bool isBoss;

    //////////////////////////////////////////////// // map data

    public int selectRoomIndex;

    public int portalDirection;

    public bool subStageEntrance;
    public bool isClear;
    public bool visible;
    public bool monsterCreate;

    public Data(string name, int num, int idx, int[] arr, float[] arr2, int dir, int sel)
    {
        structName = name;
        prfNumber = num;
        index = idx;

        if (name == "Map")
        {
            portalDirection = dir;
            selectRoomIndex = sel;

            visible = false;
            isClear = false;
            subStageEntrance = false;
            monsterCreate = false;
        }
        else
        {
            StatData(arr, arr2);
            isAcquired = false; // item

            if (name == "Player")
            {
                handMoney = 0;
                level = 1;
                experience = 0;
                needExperience = 0;

                firstTurn = false;
                secondTurn = false;
                thirdTurn = false;
                forthTurn = false;
            }
        }
    }

    public void StatData(int[] arr, float[] arr2)
    {
        maxHP = arr[0];
        nowHP = arr[0];
        power = arr[1];
        defenseRate = arr2[0];
        jumpPower = arr2[1];
        moveSpeed = arr2[2];
        attackSpeed = arr2[3];
        bloodAbsorptionRate = arr2[4];
        evasionRate = arr2[5];
    }

    public void SetPosition(Vector3 position)
    {
        pos[0] = position.x;
        pos[1] = position.y;
        pos[2] = position.z;
    }

    public Vector3 Position()
    {
        return new Vector3(pos[0], pos[1], pos[2]);
    }
}


