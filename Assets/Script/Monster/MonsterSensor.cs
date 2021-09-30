using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSensor : MonoBehaviour
{
    private Collider2D lastColliderGround = null;


    private Animator animator;
    private GameObject player;

    private bool ongoing = false;

    public Collider2D LastColliderGround { get { return lastColliderGround; } }

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
                    GetComponent<MonsterMovement>().Trigger = false;
                    GetComponent<MonsterMovement>().IsGround = true;
                }
            }
            if (collision.CompareTag("Player_attack_judgement"))
            {
                //if (!GetComponent<MonsterStatus>().Boss) animator.SetTrigger("hit");
                GetComponent<Audio>().AudioPlay(0);

                GetComponent<MonsterStatus>().Data.nowHP = GetComponent<MonsterStatus>().Data.nowHP - player.GetComponent<PlayerStatus>().Damage;
            }
            if (collision.CompareTag("Wall"))
            {
                GetComponent<BoxCollider2D>().isTrigger = false;
                GetComponent<CapsuleCollider2D>().isTrigger = false;
                GetComponent<MonsterMovement>().Trigger = false;
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
                    GetComponent<MonsterMovement>().Trigger = true;
                    GetComponent<MonsterMovement>().IsGround = false;
                }
            }

            if (collision.CompareTag("Ground"))
            {
                lastColliderGround = collision;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                if (collision.CompareTag("Ground"))
                {
                    ongoing = false;
                    GetComponent<BoxCollider2D>().isTrigger = false;
                    GetComponent<CapsuleCollider2D>().isTrigger = false;
                    GetComponent<MonsterMovement>().Trigger = false;
                    GetComponent<MonsterMovement>().IsGround = false;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                GetComponent<MonsterMovement>().IsGround = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {

        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().IsPause)
        {


            if (GetComponent<MonsterMovement>().Trigger)
            {
                GetComponent<BoxCollider2D>().isTrigger = true;
                GetComponent<CapsuleCollider2D>().isTrigger = true;
                GetComponent<MonsterMovement>().IsGround = false;
            }
        }
    }
}
