using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_attack : MonoBehaviour
{
    
    private M_info info;
    private BoxCollider2D col2D;
    private Rigidbody2D rigid2D;

    private float mx, my, mz;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            col2D.isTrigger = false;
            info.Setisground(false);
        }
    }
    public void Attackmotion()
    {
        if (info.Getatktype() == 0) // 점프공격
        {
            if (!info.Getatkdone())
            {
                if (my < info.Getatkposition("atky")) this.transform.Translate(0, info.Getpower(), 0);
                else info.Setatkdone(true);
            }
            else
            {
                col2D.isTrigger = false;
                rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (info.Gettime("atktime") >= info.GetatkSpd())
                {
                    info.Setattacked(false);
                }
            }
        }
        else if (info.Getatktype() == 1) // 돌진공격
        {
            if (info.Getdir() < 0)
            {
                if (mx > info.Getatkposition("atkx"))
                {
                    info.animator.SetBool("move", true);
                    this.transform.Translate(-info.Getpower(), 0, 0);
                }
                else
                {
                    col2D.isTrigger = false;
                    rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                    if (info.Gettime("atktime") >= info.GetatkSpd())
                    {
                        info.animator.SetBool("move", false);
                        info.Setattacked(false);
                    }
                }
            }
            else if (info.Getdir() > 0)
            {
                if (mx < info.Getatkposition("atkx"))
                {
                    info.animator.SetBool("move", true);
                    this.transform.Translate(info.Getpower(), 0, 0);
                }
                else
                {
                    col2D.isTrigger = false;
                    rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                    if (info.Gettime("atktime") >= info.GetatkSpd())
                    {
                        info.animator.SetBool("move", false);
                        info.Setattacked(false);
                    }
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<M_info>();
        rigid2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mx = this.transform.position.x;
        my = this.transform.position.y;
        mz = this.transform.position.z;
    }
}
