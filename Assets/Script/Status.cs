using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField]
    protected new string name;    
    [SerializeField]
    protected int maxHP;
    [SerializeField]
    protected float power;
    [SerializeField]
    protected float jumpPower;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float attackSpeed;
    [SerializeField]
    protected float defenseRate;
    [SerializeField]
    protected float bloodAbsorptionRate;
    [SerializeField]
    protected float evasionRate;

    // 현상유지 할거면 1을 넣어주고
    // 너프 할거면 1보다 작게
    // 버프 할거면 1보다 크게

    public string Name { get { return name; } }

    private void Start()
    {
       
    }

    private void Update()
    {
        
    }
}
