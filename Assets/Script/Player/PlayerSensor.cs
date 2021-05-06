using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabTimer;

    private Timer hitTimer;

    private Collision2D lastCollisionGround = null;
    private Collider2D lastColliderGround = null;

    private bool isGround = false;
    private bool isPortal = false;
    private bool isHit = false;
    private bool onOff = false;

    public bool Ground { get { return isGround; } }
    public bool Portal { get { return isPortal; } }
    public Collider2D LastColliderGround { get { return lastColliderGround; } }

    // 트리거 충돌
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            lastColliderGround = collision;
            GetComponent<Collider2D>().isTrigger = false;
            isGround = true;
        }

        if (collision.CompareTag("Wall"))
        {
            GetComponent<Collider2D>().isTrigger = false;
        }

        if (collision.CompareTag("Item"))
        {
            if(GetComponent<PlayerStatus>().Acquirable)
            {
                Debug.Log(collision.gameObject);
                // 소지품리스트에 물품을 추가한다.
                GetComponent<PlayerStatus>().AcquireItem(collision.gameObject);
            }
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
        if (collision.collider.CompareTag("Item"))
        {
            if (GetComponent<PlayerStatus>().Acquirable)
            {
                // 소지품리스트에 물품을 추가한다.
                GetComponent<PlayerStatus>().AcquireItem(collision.gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            lastCollisionGround = collision;
            isGround = true;
        }

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
    private void Start()
    {
        hitTimer = Instantiate(prefabTimer).GetComponent<Timer>();

        hitTimer.SetCooldown(2.0f);
    }

    // Update is called once per frame
    private void Update()
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
