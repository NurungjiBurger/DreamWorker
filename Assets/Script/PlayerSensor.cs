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
               // Debug.Log("collision �浹");
                isHit = true;
                hitTimer.TimerSetZero();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Monster"))
        {
            if (!isHit) // �ǰ��� ������ ���¶��
            {
               // Debug.Log("trigger �浹");
                isHit = true;
                hitTimer.TimerSetZero();
                // ������ ����
                // �ǰ� �� ������ ���� Ÿ�� üũ ( �ǰ� �� ��ٷ� �ǰݵ����� �ʴ´�. )
            }
        }

        if (col.CompareTag("Monster_attack_judgement"))
        {
            if (!isHit) // �ǰ��� ������ ���¶��
            {
                isHit = true;
                hitTimer.TimerSetZero();
                // ������ ����
                // �ǰ� �� ������ ���� Ÿ�� üũ ( �ǰ� �� ��ٷ� �ǰݵ����� �ʴ´�. )
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
