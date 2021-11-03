using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffectSensor : EffectSensor
{
    [SerializeField]
    private GameObject prefabTimer;
    [SerializeField]
    private int durationTime;


    public Vector3 position;

    private Timer durationTimer;
    private GameObject monster;

    public int Dmg { get { return dmg; } }

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

    // Start is called before the first frame update
    private void Start()
    {
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
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (type == 1) // ��������
            {
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
