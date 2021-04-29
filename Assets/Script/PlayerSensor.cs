using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabTimer;

    private Timer hitTimer;

    private bool isHit = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Monster"))
        {
            if (!isHit)
            {
               // Debug.Log("collision 충돌");
                isHit = true;
                hitTimer.TimerSetZero();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Monster"))
        {
            if (!isHit) // 피격이 가능한 상태라면
            {
               // Debug.Log("trigger 충돌");
                isHit = true;
                hitTimer.TimerSetZero();
                // 데미지 들어옴
                // 피격 후 무적을 위한 타임 체크 ( 피격 후 곧바로 피격되지는 않는다. )
            }
        }

        if (col.CompareTag("Monster_attack_judgement"))
        {
            if (!isHit) // 피격이 가능한 상태라면
            {
                isHit = true;
                hitTimer.TimerSetZero();
                // 데미지 들어옴
                // 피격 후 무적을 위한 타임 체크 ( 피격 후 곧바로 피격되지는 않는다. )
            }
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        hitTimer = Instantiate(prefabTimer).GetComponent<Timer>();

        hitTimer.SetCooldown(2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit) isHit = !hitTimer.CooldownCheck();
        //Debug.Log(isHit);
    }
}
