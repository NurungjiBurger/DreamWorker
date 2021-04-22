using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    // 몬스터의 스탯 & 상태
    static int size = 3;
    string enemyName;
    int maxHP;
    int nowHP;
    int atkDmg;
    int atkSpeed;
    int recognition_range;
    float jumpPower;
    float dashPower;
    public bool attacked = false;
    bool atkdone = false;
    bool isground = false;
    float dir;
    int attacktype;

    public float height = 0.7f;
    int mvrandom;
    float atktime;
    float mvtime;

    float px, py, pz;
    float mx, my, mz;
    float atkx, atky, atkz;

    // 오브젝트
    public GameObject prfHpBar;
    public GameObject canvas;
    public GameObject objmonster;
    Animator animator;
    RectTransform hpBar;
    public P_info player;

    BoxCollider2D col2D;
    Rigidbody2D rigid2D;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
