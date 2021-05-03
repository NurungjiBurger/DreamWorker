using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSensor : MonoBehaviour
{

    [SerializeField]
    private GameObject prefabTimer;

    private Collider2D lastColliderGround = null;
    private Collision2D lastCollisionGround = null;

    private Animator animator;
    private GameObject player;

    private bool isGround;

    public bool Ground { get { return isGround; } }
    public Collider2D LastColliderGround { get { return lastColliderGround; } }
    public Collision2D LastCollisionGround { get { return lastCollisionGround; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_attack_judgement"))
        {
            if (!GetComponent<MonsterStatus>().Boss) animator.SetTrigger("hit");
            GetComponent<MonsterStatus>().NowHP = GetComponent<MonsterStatus>().NowHP - collision.GetComponent<PlayerEffectSensor>().Damage;
        }
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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            lastCollisionGround = collision;
            isGround = true;
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
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
