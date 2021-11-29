using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffectSensor : EffectSensor
{
    [SerializeField]
    private int durationTime;
    [SerializeField]
    private GameObject prefabTimer;

    private GameObject monster;

    public Vector3 position;
    private Timer durationTimer;

    public int Dmg { get { return dmg; } }
    
    private void OnEnable()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void CalCulateDamage()
    {
        dmg = (int)monster.GetComponent<MonsterStatus>().Data.power * GameObject.Find("Data").GetComponent<DataController>().GameData.round;
    }

    public void AllocateObject(GameObject obj)
    {
        monster = obj;
        CalCulateDamage();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

    }

    private void Start()
    {
        // ����Ʈ�� �����ð����� �����ؾ� �ϴ� ��� ( ex. �������� )
        if (durationTime != 0)
        {
            durationTimer = Instantiate(prefabTimer).GetComponent<Timer>();
            durationTimer.SetCooldown(durationTime);

            durationTimer.TimerSetZero();
        }

        if (dir == 1) GetComponent<SpriteRenderer>().flipX = false;
        else GetComponent<SpriteRenderer>().flipX = true;

        if (type == 1) disappear = false;
        else disappear = true;
    }

    private void FixedUpdate()
    {
        if (!gameController.IsPause)
        {
            if (type == 1) // ��������
            {
                // �Ҵ�� �ð��� �� �Ǹ� ����
                if (durationTimer.CooldownCheck())
                {
                    disappear = true;
                    DestroyObject();
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
