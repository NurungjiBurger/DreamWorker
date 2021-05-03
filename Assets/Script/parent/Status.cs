using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public int maxHP;
    public int nowHP;
    public int power;
    public float jumpPower;
    public float moveSpeed;
    public string name;


    public float JumpPower { get { return jumpPower; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public int MaxHP { get { return maxHP; } }

    public int NowHP {  get { return nowHP; } set { nowHP = value; } }

    public string Name {  get { return name; } }

    public int Power { get { return power; } }

    // Start is called before the first frame update
    void Start()
    {
       // nowHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
