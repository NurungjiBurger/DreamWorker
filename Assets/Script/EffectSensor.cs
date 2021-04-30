using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSensor : MonoBehaviour
{
    [SerializeField]
    protected int dmg;
    [SerializeField]
    protected int type;
    [SerializeField]
    protected int size;

    protected float dir;
    protected bool disappear = false;

    public int Damage { get { return dmg; } }

    void DestroyObject()
    {
        if (disappear) Destroy(gameObject);
    }    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void FixedUpdate()
    { 

    }

    // Update is called once per frame
    void Update()
    {
    }
}
