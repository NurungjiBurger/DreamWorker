using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_balrog : MonoBehaviour
{

    private M_info info;
    private Collider2D col2D;

    private Animator animator;
    //private CircleCollider2D col2D;

    [SerializeField]
    private int Dmg;
    [SerializeField]
    private int type;
    [SerializeField]
    private int size;

    private float dir;
    private bool disappear = false;

    public int GetDmg()
    {
        return Dmg;
    }

    void destroy()
    {
        if (type == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            if (disappear)
            {
                Destroy(gameObject);
            }
        }
    }    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (type == 1)
        {
            if (col.CompareTag("Ground") || col.CompareTag("Wall"))
            {
                disappear = true;
                animator.SetBool("disappear", true);
            }
            else if (col.CompareTag("Player"))
            {
                if(!col.GetComponent<P_info>().Gethit())
                {
                    disappear = true;
                    animator.SetBool("disappear", true);
                }
            }
        }
        else if (type != 0)
        {
            if (col.CompareTag("Player"))
            {
                disappear = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        info = GameObject.FindGameObjectWithTag("Monster").GetComponent<M_info>();
        if (type != 0 ) col2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        dir = info.Getdir();
        if (dir == 1) this.transform.localScale = new Vector3(size, size, size);
        else this.transform.localScale = new Vector3(-size, size, size);

        if (type != 1) disappear = true;
    }

    private void FixedUpdate()
    { 
        if (type == 1) // ╦чев©ю
        {
            if (col2D.isTrigger)
            {
                this.transform.Translate(-0.01f * dir, -0.01f, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
