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

    // 오브젝트 사이즈 변경
    public void ChangeSize(float size)
    {
        x = x * size;
        y = y * size;
        z = z * size;
    }

    // 오브젝트 좌우 / 상하 반전
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

    void Start()
    {
        flipX = false;
        flipY = false;
    }

    void Update()
    {
        flip('x', flipX);
    }
}
