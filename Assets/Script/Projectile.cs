using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    protected GameObject prefabEffect;
    [SerializeField]
    protected float attackRange;

    protected int cnt;
    protected int flag;
    protected int dir;

    protected Vector3 startPosition;
    protected Animator animator;
    protected bool flip;

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }
}
