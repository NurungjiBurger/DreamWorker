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

    private bool ongoing = false;
    private bool isPortal = false;
    private bool isHit = false;
    private bool onOff = false;
    private Vector3 teleportPosition;

    public bool Portal { get { return isPortal; } }
    public Collider2D LastColliderGround { get { return lastColliderGround; } }
    public Vector3 TeleportPosition { get { return teleportPosition; } }

    // 트리거 충돌
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (GetComponent<Rigidbody2D>().velocity.y < 0 && !ongoing)
            {
                if (collision.CompareTag("Ground"))
                {
                    Debug.Log("걸림");
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
                if (GetComponent<PlayerStatus>().Acquirable)
                {
                    GetComponent<PlayerStatus>().Inventory.GetComponent<Inventory>().AddToInventory(GameObject.Find("GameController").GetComponent<GameController>().CreateItemSlot(collision.gameObject));
                }
            }

            if (collision.CompareTag("Wealth"))
            {
                if (collision.GetComponent<Parabola>().Arrived)
                {
                    if (collision.name.Equals("Gold(Clone)")) GetComponent<PlayerStatus>().AddHandMoney(10);
                    if (collision.name.Equals("Golds(Clone)")) GetComponent<PlayerStatus>().AddHandMoney(30);
                    //if (collision.collider.name.Equals("Mileage(Clone)")) GetComponent<PlayerStatus>().AddHandMoney(10);
                    Destroy(collision.gameObject);
                }
            }
            //
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
                    Debug.Log("여기도걸림");
                    ongoing = true;
                    GetComponent<BoxCollider2D>().isTrigger = true;
                    GetComponent<CapsuleCollider2D>().isTrigger = true;
                    GetComponent<PlayerMovement>().Trigger = true;
                    GetComponent<PlayerMovement>().IsGround = false;
                }
            }

            if (collision.CompareTag("Ground"))
            {
                lastColliderGround = collision;
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
                    isHit = true;
                    hitTimer.TimerSetZero();
                    onOff = true;
                }
            }
            if (collision.CompareTag("Monster_attack_judgement"))
            {
                if (!isHit)
                {
                    isHit = true;
                    hitTimer.TimerSetZero();
                    onOff = true;
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
                Debug.Log("탈출");
                ongoing = false;
                GetComponent<BoxCollider2D>().isTrigger = false;
                GetComponent<CapsuleCollider2D>().isTrigger = false;
                GetComponent<PlayerMovement>().Trigger = false;
                GetComponent<PlayerMovement>().IsGround = false;
            }
        }

        if (collision.CompareTag("Portal")) isPortal = false;
            if (collision.CompareTag("Monster"))
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        
    }

    // 콜리젼 충돌
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (collision.collider.CompareTag("Item"))
            {
                if (GetComponent<PlayerStatus>().Acquirable)
                {
                    GetComponent<PlayerStatus>().Inventory.GetComponent<Inventory>().AddToInventory(GameObject.Find("GameController").GetComponent<GameController>().CreateItemSlot(collision.gameObject));
                }
            }

            if (collision.collider.CompareTag("Wealth"))
            {
                if (collision.collider.name.Equals("Gold(Clone)")) GetComponent<PlayerStatus>().AddHandMoney(10);
                if (collision.collider.name.Equals("Golds(Clone)")) GetComponent<PlayerStatus>().AddHandMoney(30);
                //if (collision.collider.name.Equals("Mileage(Clone)")) GetComponent<PlayerStatus>().AddHandMoney(10);
                Destroy(collision.collider.gameObject);
            }

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (collision.collider.CompareTag("Ground")) GetComponent<PlayerMovement>().IsGround = true;
            if (collision.collider.CompareTag("Ground"))
            {
            lastCollisionGround = collision;
            }
            
            if (collision.collider.CompareTag("Monster"))
            {
                //GetComponent<Collider2D>().isTrigger = true;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                if (!isHit)
                {
                    isHit = true;
                    hitTimer.TimerSetZero();
                    onOff = true;
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
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (onOff)
            {
                isHit = !hitTimer.CooldownCheck();
                if (!isHit)
                {
                    onOff = false;
                    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
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
