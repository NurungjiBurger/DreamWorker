using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabTimer;

    private Timer hitTimer;

    private bool isGround = false;
    private bool isPortal = false;
    private bool isHit = false;
    private bool onOff = false;

    public bool Ground { get { return isGround; } }
    public bool Portal { get { return isPortal; } }

    // 트리거 충돌
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            GetComponent<Collider2D>().isTrigger = false;
            isGround = true;
        }
    } 

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal")) isPortal = true;
        if (collision.CompareTag("Monster"))
        {
            if (!isHit) // 피격이 가능한 상태라면
            {
                isHit = true;
                hitTimer.TimerSetZero();
                onOff = true;
            }
        }
        if (collision.CompareTag("Monster_attack_judgement"))
        {
            if (!isHit) // 피격이 가능한 상태라면
            {
                isHit = true;
                hitTimer.TimerSetZero();
                onOff = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal")) isPortal = false;
        if (collision.CompareTag("Monster"))
        {
            GetComponent<Collider2D>().isTrigger = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    // 콜리젼 충돌
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground")) isGround = true;

        if (collision.collider.CompareTag("Monster"))
        {
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            if (!isHit)
            {               
                isHit = true;
                hitTimer.TimerSetZero();
                onOff = true; 
            }
        }        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            GetComponent<Collider2D>().isTrigger = true;
            isGround = false;
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
        if (onOff)
        {
            isHit = !hitTimer.CooldownCheck();
            if (!isHit)
            {
                onOff = false;
                GetComponent<Collider2D>().isTrigger = false;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
}
