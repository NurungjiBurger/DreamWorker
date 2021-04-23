using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbital_attack : MonoBehaviour
{
    public P_info player;
    private int dir;
    Animator animator;
    int flag;
    private int cnt;
    public bool Judgement = false;
    private int range;
    float time;

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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_info>();
        dir = player.Getdir();
        range = player.Getatkrange();
        gameObject.transform.localScale = new Vector3(2 * dir, 2, 2);
        cnt = 0;
        flag = 1;
        time = 0;
    }

    private IEnumerator TimeUpdate()
    {
        

        yield return new WaitForSeconds(0.1f);
    }

    private void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (cnt == 0) dir = player.Getdir();

        // 플레이어에게서 사출되어 멀어짐
        if (flag == 1)
        {
            if ((player.Getp_position("plax") + (range * dir)) != this.transform.position.x && cnt < (range * 50))
            {
                gameObject.transform.Translate(-0.02f * dir, 0, 0);
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
            if (player.Getp_position("plax") != this.transform.position.x && cnt < (range * 50 * 2))
            {
                gameObject.transform.Translate(0.02f * dir, 0, 0);
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
