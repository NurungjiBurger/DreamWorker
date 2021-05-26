using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatus : Status
{
    enum Grade { Normal, Rare, Epic, Unique, Legendary };

    [SerializeField]
    private GameObject[] prefabAttack;
    [SerializeField]
    private string dedicatedOccupation;
    [SerializeField]
    private string mountingPart;
    [SerializeField]
    private Grade grade;
    [SerializeField]
    private int cursedRate;

    private GameObject player;
    private bool isAttack = false;
    private bool isMount = false;

    public int CursedRate { get { return cursedRate; } set { cursedRate = value; } }
    public int ItemGrade { get { return (int)grade; } }
    public string MountingPart { get { return mountingPart; } }
    public bool IsAttack { get { return isAttack; } }
    public bool IsMount { get { return isMount; } set { isMount = value; } }
    public string Occupation { get { return dedicatedOccupation; } }

    private void StatUP()
    {
        maxHP = (int)(maxHP * 1.5f);
        power = (int)(power * 1.3f);
        jumpPower = jumpPower * 1.2f;
        moveSpeed = moveSpeed * 1.2f;
        attackSpeed = attackSpeed * 1.1f;
        defenseCapability = defenseCapability * 1.2f;
    }

    private int OccupationCheck()
    {
        if (player.GetComponent<PlayerStatus>().Occupation == dedicatedOccupation) return 1;
        else return 0;
    }
    public GameObject GetAttackAnimation()
    {
        return prefabAttack[OccupationCheck()];
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<PlayerStatus>().Occupation == dedicatedOccupation) StatUP();

        cursedRate = Random.Range(0, 70); // 저주율 수치 조정 필요
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GoNext) Destroy(gameObject);
    }
}
