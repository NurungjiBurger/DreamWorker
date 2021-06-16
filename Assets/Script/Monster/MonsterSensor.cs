using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSensor : MonoBehaviour
{
    private Collider2D lastColliderGround = null;

    private bool isPlayer;

    private Animator animator;
    private GameObject player;

    private bool isGround;

    public bool Player { get { return isPlayer; } }
    public bool Ground { get { return isGround; } }
    public Collider2D LastColliderGround { get { return lastColliderGround; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (collision.CompareTag("Player_attack_judgement"))
            {
                if (!GetComponent<MonsterStatus>().Boss) animator.SetTrigger("hit");
                GetComponent<MonsterStatus>().Status.nowHP = GetComponent<MonsterStatus>().Status.nowHP - player.GetComponent<PlayerStatus>().Damage;
            }
            if (collision.CompareTag("Wall"))
            {
                GetComponent<Collider2D>().isTrigger = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (collision.CompareTag("Player")) isPlayer = true;
            if (collision.CompareTag("Ground") && GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                lastColliderGround = collision;
                GetComponent<Collider2D>().isTrigger = false;
                isGround = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (collision.CompareTag("Player")) isPlayer = false;
            if (collision.CompareTag("Ground")) isGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                isGround = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                GetComponent<Collider2D>().isTrigger = true;
                isGround = false;
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
