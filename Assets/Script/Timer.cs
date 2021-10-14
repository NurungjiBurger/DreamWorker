using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
//    [SerializeField] private float duration;

    private float time;
    private float cooldown;

    public void DestroyAll()
    {
        Destroy(gameObject);
    }

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
    private void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;

        /*
            if (duration != 0)
            {
                Debug.Log(time + " " + duration);
                if (time >= duration)
                {
                    Debug.Log("¿Ö ½ÇÇà?");
                    DestroyAll();
                }
            }
            */
    }
}
