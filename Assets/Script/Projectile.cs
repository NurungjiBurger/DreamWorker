using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField]
    protected GameObject prefabEffect;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected bool isComBack;
    [SerializeField]
    protected bool isMove;
    
    protected int cnt;
    protected int flag;
    protected int dir;

    protected Vector3 startPosition;
    protected Animator animator;
    protected bool flip;

    private void Start()
    {

    }

    private void Update()
    {

    }
}
