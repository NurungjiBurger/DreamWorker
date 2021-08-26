using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFlip : MonoBehaviour
{
    float x, y, z;
    public bool flipX, flipY;

    public void flip(char axis, bool value)
    {
        int dir;

        if (value)
        {
            flipX = true;
            dir = -1;
        }
        else
        {
            flipX = false;
            dir = 1;
        }

        if (axis == 'x')
        {
            transform.localScale = new Vector3(x * dir, y, z);
        }
        else if (axis == 'y')
        {
            transform.localScale = new Vector3(x, y * dir, z);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        x = transform.localScale.x;
        y = transform.localScale.y;
        z = transform.localScale.z;

        flipX = false;
        flipY = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
