using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField]
    protected int maxHP;
    [SerializeField]
    protected int power;
    [SerializeField]
    protected float jumpPower;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float attackSpeed;
    [SerializeField]
    protected string name;
    [SerializeField]
    protected float defenseCapability;

    protected int nowHP;

    // 현상유지 할거면 1을 넣어주고
    // 너프 할거면 1보다 작게
    // 버프 할거면 1보다 크게

    public float Defense { get { return defenseCapability; } set { defenseCapability = value; } }
    public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
    public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public int MaxHP { get { return maxHP; } set { maxHP = value; } }
    public int NowHP {  get { return nowHP; } set { nowHP = value; } }
    public string Name {  get { return name; } }
    public int Power { get { return power; } set { power = value; } }

    // Start is called before the first frame update
    private void Start()
    {
       // nowHP = maxHP;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
