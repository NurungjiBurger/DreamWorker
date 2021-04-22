using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbital_attack : MonoBehaviour
{
    public P_info character;
    private int dir;
    Animator animator;
    int flag;
    private int cnt;
    public bool Judgement = false;
    private int range;

    void JudgemnetTrue()
    {
        Judgement = true;
    }
    void JudgementFalse()
    {
        Judgement = false;
    }

    public bool state()
    {
        return Judgement;
    }
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<P_info>();
        dir = character.dir;
        gameObject.transform.localScale = new Vector3(2 * dir, 2, 2);
        cnt = 0;
        range = character.attack_range;
        flag = 1;
    }
    private void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

        if (cnt == 0) dir = character.dir;

        // 플레이어에게서 사출되어 멀어짐
        if (flag == 1)
        {
            if ((character.plax + (range * dir)) != character.shotx && cnt < (range * 10))
            {
                gameObject.transform.Translate(-0.1f * dir, 0, 0);
                character.shotx = character.shotx - (dir * 0.1f);
                cnt++;
            }
            else
            {
                // 방향 전환
                flag = 2;
                gameObject.transform.localScale = new Vector3(-2 * dir, 2, 2);
            }
        }

        // 원래 플레이어의 위치로 돌아옴
        else if (flag == 2)
        {
            if (character.plax != character.shotx && cnt < (range * 10 * 2))
            {
                gameObject.transform.Translate(0.1f * dir, 0, 0);
                character.shotx = character.shotx + (dir * 0.1f);
                cnt++;
            }
            else
            {
                // 끝
                Destroy(gameObject);
            }
        }


    }
}
