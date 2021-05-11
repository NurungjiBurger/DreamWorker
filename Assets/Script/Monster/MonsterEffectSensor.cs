using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffectSensor : EffectSensor
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (type == 1)
        {
            if (col.CompareTag("Ground") || col.CompareTag("Wall"))
            {
                disappear = true;
                GetComponent<Animator>().SetBool("disappear", true);
            }
        }

        if (col.CompareTag("Player"))
        {
            disappear = true;
            if (type == 1) GetComponent<Animator>().SetBool("disappear", true);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        dir = GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterAttack>().Direction;

        if (dir == 1) GetComponent<SpriteRenderer>().flipX = false;
        else GetComponent<SpriteRenderer>().flipX = true;

        if (type == 1) disappear = false;
        else disappear = true;
    }

    private void FixedUpdate()
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (type == 1) // ╦чев©ю
            {
                transform.Translate(-0.01f * dir, -0.01f, 0);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
