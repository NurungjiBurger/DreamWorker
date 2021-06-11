using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class GameData : MonoBehaviour
{
    public GameObject player;

    public List<Slot> equipItem = new List<Slot>();
    public List<Slot> possessItem = new List<Slot>();

    public GameObject inspector;
    public GameObject inventory;

    public int num;

    bool save;

    public void ComLoad()
    {

    }

    private void AutoSave()
    {

    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
