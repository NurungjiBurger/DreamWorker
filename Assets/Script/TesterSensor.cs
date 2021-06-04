using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterSensor : MonoBehaviour
{

    private bool positionSave;

    public bool PositionSave { get { return positionSave; } set { positionSave = value; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Item"))
        {

        }
        else
        {
            positionSave = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Item"))
        {

        }
        else
        {
            positionSave = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
