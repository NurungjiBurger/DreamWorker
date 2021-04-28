using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float time;
    private float cooldown;

    public void SetCooldown(float cool)
    {
        cooldown = cool;
    }

    public bool CooldownCheck()
    {
        if (time >= cooldown) return true;
        else return false;
    }

    public void TimerSetZero()
    {
        time = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }
}
