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

    protected bool disappear = false;

    public int dir;

    protected GameController gameController;

    public int Damage { get { return dmg; } }

    private void OnEnable()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void DestroyObject()
    {
        if (disappear) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameController.IsPause)
        {
            if (GetComponent<Rigidbody2D>() && GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                if (collision.CompareTag("Ground"))
                {
                    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
       
    }

    private void FixedUpdate()
    { 

    }

    // Update is called once per frame
    private void Update()
    {
    }
}
