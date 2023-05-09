using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class GameData
{
    // 기본 게임데이터
    public int numberOfClearRoom = 0;
    public int numberOfKilledMonster = 0;
    public int numberOfObtainedItems = 0;
    public int obtainedGold = 0;
    public float playTime = 0.0f;

    public string winOrLose = "";

    public int round;
    public int stageNumber;
    public int subStageNumber;
    public bool stageEntrance;
    public bool stageClear;

    public bool eventRoomVisit;

    public List<Data> datas = new List<Data>();
}

[Serializable]
public class Data
{
    // 공통
    public string structName;
    public float[] pos = new float[3];
    public int prfNumber;
    public int index;

    //////////////////////////////////////////////// // 플레이어

    public int itemPiece;

    public int handMoney;
    public int level;
    public int experience;
    public int needExperience;

    public bool firstEvolution;
    public bool secondEvolution;
    public bool thirdEvolution;
    public bool forthEvolution;

    //////////////////////////////////////////////// // 아이템

    public int cursedRate;
    public bool isMount;
    public bool isAcquired;
    public int enhancingLevel;

    //////////////////////////////////////////////// // 스탯

    public int maxHP;
    public int nowHP;
    public float power;
    public float defenseRate;
    public float jumpPower;
    public float moveSpeed;
    public float attackSpeed;
    public float bloodAbsorptionRate;
    public float evasionRate;

    //////////////////////////////////////////////// // 몬스터

    public bool isBoss;

    //////////////////////////////////////////////// // 맵

    public int selectRoomIndex;
    public int portalDirection;

    public bool isEvent;
    public bool subStageEntrance;
    public bool isClear;
    public bool visible;
    public bool monsterCreate;

    public int eventRoomIndex;

    public Data(string name, int num, int idx, int[] arr, float[] arr2, int dir, int sel, bool isevent)
    {
        structName = name;
        prfNumber = num;
        index = idx;

        if (name == "Map")
        {
            isEvent = isevent;
            portalDirection = dir;
            selectRoomIndex = sel;

            visible = false;
            isClear = false;
            subStageEntrance = false;
            monsterCreate = false;
        }
        else if (name == "EventMap")
        {
            eventRoomIndex = idx;
        }
        else
        {
            StatData(arr, arr2);
            isAcquired = false;
            isMount = false;

            if (name == "Player")
            {

                itemPiece = 5000;
                handMoney = 0;
                level = 1;
                experience = 0;
                needExperience = 0;

                firstEvolution = false;
                secondEvolution = false;
                thirdEvolution = false;
                forthEvolution = false;
            }
        }
    }

    public void StatData(int[] arr, float[] arr2)
    {
        maxHP = arr[0];
        nowHP = arr[0];
        defenseRate = arr2[0];
        jumpPower = arr2[1];
        moveSpeed = arr2[2];
        attackSpeed = arr2[3];
        bloodAbsorptionRate = arr2[4];
        evasionRate = arr2[5];
        power = arr2[6];
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


