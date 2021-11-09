using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabTimer;

    private Timer hitTimer;

    private Collider2D lastColliderGround = null;

    private bool ongoing = false;
    private bool isPortal = false;
    private bool isHit = false;
    private bool onOff = false;
    private Vector3 teleportPosition;

    public bool Portal { get { return isPortal; } }
    public Collider2D LastColliderGround { get { return lastColliderGround; } }
    public Vector3 TeleportPosition { get { return teleportPosition; } }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (GetComponent<Rigidbody2D>().velocity.y < 0 && !ongoing)
            {
                if (collision.CompareTag("Ground"))
                {
                    GetComponent<BoxCollider2D>().isTrigger = false;
                    GetComponent<CapsuleCollider2D>().isTrigger = false;
                    GetComponent<PlayerMovement>().Trigger = false;
                    GetComponent<PlayerMovement>().IsGround = true;
                }
            }

            if (collision.CompareTag("Wall"))
            {
                GetComponent<BoxCollider2D>().isTrigger = false;
                GetComponent<CapsuleCollider2D>().isTrigger = false;
                GetComponent<PlayerMovement>().Trigger = false;
            }

            if (collision.CompareTag("Item"))
            {
                if (GetComponent<PlayerStatus>().Acquirable && !collision.GetComponent<ItemStatus>().Data.isAcquired)
                {
                    collision.GetComponent<ItemStatus>().Data.isAcquired = true;
                    GetComponent<PlayerStatus>().Inventory.GetComponent<Inventory>().AddToInventory(GameObject.Find("GameController").GetComponent<GameController>().CreateItemSlot(collision.gameObject));
                }
            }
        }

    } 

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                if (collision.CompareTag("Ground"))
                {
                    ongoing = true;
                    GetComponent<BoxCollider2D>().isTrigger = true;
                    GetComponent<CapsuleCollider2D>().isTrigger = true;
                    GetComponent<PlayerMovement>().Trigger = true;
                    GetComponent<PlayerMovement>().IsGround = false;
                }
            }

            if (collision.CompareTag("Wall"))
            {
                GetComponent<BoxCollider2D>().isTrigger = false;
                GetComponent<CapsuleCollider2D>().isTrigger = false;
                GetComponent<PlayerMovement>().Trigger = false;
            }

            if (collision.CompareTag("Portal"))
            {
                teleportPosition = collision.GetComponent<Portal>().ConnectPosition;
                isPortal = true;
            }

            if (collision.CompareTag("Monster"))
            {
                if (!isHit)
                {
                    // 회피 실패
                    if (Random.Range(0, 101) > GetComponent<PlayerStatus>().Data.evasionRate)
                    {
                        isHit = true;
                        hitTimer.TimerSetZero();
                        onOff = true;

                        GetComponent<PlayerStatus>().CalCulateHealth(collision.gameObject.GetComponent<MonsterStatus>().Dmg, '-');
                    }
                }
            }
            else if (collision.CompareTag("Monster_attack_judgement"))
            {
                if (!isHit)
                {
                    // 회피 실패
                    if (Random.Range(0, 101) > GetComponent<PlayerStatus>().Data.evasionRate)
                    {
                        isHit = true;
                        hitTimer.TimerSetZero();
                        onOff = true;

                        GetComponent<PlayerStatus>().CalCulateHealth(collision.gameObject.GetComponent<MonsterEffectSensor>().Dmg, '-');
                    }
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            if (collision.CompareTag("Ground"))
            {
                ongoing = false;
                GetComponent<BoxCollider2D>().isTrigger = false;
                GetComponent<CapsuleCollider2D>().isTrigger = false;
                GetComponent<PlayerMovement>().Trigger = false;
                GetComponent<PlayerMovement>().IsGround = false;
            }
        }

        if (collision.CompareTag("Portal")) isPortal = false;
        
    }

    // 콜리젼 충돌
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (collision.collider.CompareTag("Item"))
            {
                if (GetComponent<PlayerStatus>().Acquirable && !collision.collider.gameObject.GetComponent<ItemStatus>().Data.isAcquired)
                {
                    collision.collider.gameObject.GetComponent<ItemStatus>().Data.isAcquired = true;
                    GetComponent<PlayerStatus>().Inventory.GetComponent<Inventory>().AddToInventory(GameObject.Find("GameController").GetComponent<GameController>().CreateItemSlot(collision.gameObject));
                }
            }

            if (collision.collider.CompareTag("Wealth"))
            {
                if (collision.collider.name.Equals("골드(Clone)")) GetComponent<PlayerStatus>().CalCulateHandMoney(10, '+');
                if (collision.collider.name.Equals("골드주머니(Clone)")) GetComponent<PlayerStatus>().CalCulateHandMoney(30, '+');
                //if (collision.collider.name.Equals("스캐럽(Clone)")) GetComponent<PlayerStatus>().AddHandMoney(10);
                Destroy(collision.collider.gameObject);
            }

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                GetComponent<PlayerMovement>().IsGround = true;
            }
            
            if (collision.collider.CompareTag("Monster"))
            {
                // 회피 실패
                if (Random.Range(0, 101) > GetComponent<PlayerStatus>().Data.evasionRate) {
                    if (!isHit)
                    {
                        isHit = true;
                        hitTimer.TimerSetZero();
                        onOff = true;

                        GetComponent<PlayerStatus>().CalCulateHealth(collision.collider.gameObject.GetComponent<MonsterStatus>().Dmg, '-');
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void Start()
    {
        hitTimer = Instantiate(prefabTimer).GetComponent<Timer>();

        hitTimer.SetCooldown(2.0f);
    }

    private void Update()
    {
        //Debug.Log("Box  " + GetComponent<BoxCollider2D>().isTrigger);
        //Debug.Log("Cap  " + GetComponent<CapsuleCollider2D>().isTrigger);

        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (onOff)
            {
                isHit = !hitTimer.CooldownCheck();
                if (!isHit)
                {
                    onOff = false;
                    GetComponent<BoxCollider2D>().isTrigger = false;
                    GetComponent<CapsuleCollider2D>().isTrigger = false;
                }
            }

            if (GetComponent<PlayerMovement>().Trigger)
            {
                GetComponent<BoxCollider2D>().isTrigger = true;
                GetComponent<CapsuleCollider2D>().isTrigger = true;
                GetComponent<PlayerMovement>().IsGround = false;
            }
        }
    }
}
