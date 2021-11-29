using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
//    [SerializeField] private float duration;

    private float time;
    private float cooldown;

    // 타이머 파괴
    public void DestroyAll()
    {
        Destroy(gameObject);
    }

    // 쿨다운 시간 설정
    public void SetCooldown(float cool)
    {
        cooldown = cool;
    }

    // 쿨다운 시간 체크
    public bool CooldownCheck()
    {
        if (time >= cooldown) return true;
        else return false;
    }

    // 타이머 초기화
    public void TimerSetZero()
    {
        time = 0;
    }

    private void Start()
    {
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;

    }
}
