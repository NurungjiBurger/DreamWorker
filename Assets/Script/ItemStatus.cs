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
        // ���⸶�� ������ ���� �������� ���⸦ �������� ��� Ư���� ȿ���� ���⵵����.
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
