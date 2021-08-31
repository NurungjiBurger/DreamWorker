using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFlip : MonoBehaviour
{
    [SerializeField]
    float x, y, z;
    [SerializeField]
    int magnification;

    public bool flipX, flipY;

    public void BeTheBossMonster()
    {
        x = x * 3;
        y = y * 3;
        z = z * 3;
    }

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
    }

    // Start is called before the first frame update
    void Start()
    {
        flipX = false;
        flipY = false;
    }

    // Update is called once per frame
    void Update()
    {
        flip('x', flipX);
    }
}
