using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatus : Status
{
    [SerializeField]
    private GameObject[] prefabAttack;
    [SerializeField]
    private string dedicatedOccupation;
    [SerializeField]
    private string mountingPart;

    private float cursedRate;
    private GameObject player;

    public float CurseRate { get { return cursedRate; } set { cursedRate = value; } }
    public string Occupation { get { return dedicatedOccupation; } }

    private int OccupationCheck()
    {
        // 무기마다 정해진 전용 직업군이 무기를 장착했을 경우 특별한 효과가 생기도록함.
        if (player.GetComponent<PlayerStatus>().Occupation == dedicatedOccupation) return 1;
        else return 0;
    }
    public void AttackAnimatestart()
    {
        
        Instantiate(prefabAttack[OccupationCheck()], player.transform.position, Quaternion.identity);
    }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
