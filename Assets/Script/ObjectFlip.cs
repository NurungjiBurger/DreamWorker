using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFlip : MonoBehaviour
{
    [SerializeField]
    private float x, y, z;
    [SerializeField]
    private int magnification;

    public bool flipX, flipY;

    public void ChangeSize(float size)
    {
        x = x * size;
        y = y * size;
        z = z * size;
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
