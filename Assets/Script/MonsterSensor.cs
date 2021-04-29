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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }

        if (col.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player_attack_judgement"))
        {
            if (!GetComponent<MonsterStatus>().Boss) animator.SetTrigger("hit");
            GetComponent<MonsterStatus>().NowHP = GetComponent<MonsterStatus>().NowHP - col.GetComponent<orbital_attack>().GetDamage();
            Debug.Log(GetComponent<MonsterStatus>().NowHP);
        }

        if (col.CompareTag("Player"))
        {
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }

        if (col.CompareTag("Ground"))
        {
            Debug.Log("∂•¿Ã¥Ÿ");
            isGround = true;
            GetComponent<Collider2D>().isTrigger = false;
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
