using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSensor : MonoBehaviour
{

    [SerializeField]
    private GameObject prefabTimer;

    private Animator animator;
    private GameObject player;

    private bool isGround;

    public bool Ground { get { return isGround; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_attack_judgement"))
        {
            if (!GetComponent<MonsterStatus>().Boss) animator.SetTrigger("hit");
            GetComponent<MonsterStatus>().NowHP = GetComponent<MonsterStatus>().NowHP - collision.GetComponent<PlayerEffectSensor>().Damage;
        }
        if (collision.CompareTag("Ground") && GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            GetComponent<Collider2D>().isTrigger = false;
            isGround = true;
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("돌아옴");
            isGround = true;
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log("붙어있는중");
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log("떠난다");
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
