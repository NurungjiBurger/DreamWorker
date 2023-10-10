using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
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
    [SerializeField]
    protected new string name;

    // �������� �ҰŸ� 1�� �־��ְ�
    // ���� �ҰŸ� 1���� �۰�
    // ���� �ҰŸ� 1���� ũ��

    public string Name { get { return name; } }

    private void Start()
    {
       
    }

    private void Update()
    {
        
    }
}
