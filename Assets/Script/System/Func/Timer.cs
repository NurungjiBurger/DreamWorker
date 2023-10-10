using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
//    [SerializeField] private float duration;

    private float time;
    private float cooldown;
    private bool active = false;

    private void ActiveFalse()
    {
        active = false;
        GameObject.Find("Canvas").transform.Find("StageNumber").gameObject.SetActive(false);
    }
    // Ÿ�̸� �ı�
    public void DestroyAll()
    {
        Destroy(gameObject);
    }

    // ��ٿ� �ð� ����
    public void SetCooldown(float cool)
    {
        cooldown = cool;
    }

    // ��ٿ� �ð� üũ
    public bool CooldownCheck()
    {
        if (time >= cooldown) return true;
        else return false;
    }

    // Ÿ�̸� �ʱ�ȭ
    public void TimerSetZero()
    {
        time = 0;
    }

    public void ActiveTrue()
    {
        active = true;
    }

    private void Start()
    {
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (active && CooldownCheck()) ActiveFalse();

    }
}
