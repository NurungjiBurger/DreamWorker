using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbital_attack : MonoBehaviour
{
    private GameObject player;
    private bool flip;
    private Animator animator;
    private int flag;
    private int cnt;
    private Vector3 startPosition;
    private int dir;


    [SerializeField]
    private int damage;
    [SerializeField]
    private float attackRange;

    public int GetDamage()
    {
        return damage;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        flip = player.GetComponent<SpriteRenderer>().flipX;

        GetComponent<SpriteRenderer>().flipX = flip;

        attackRange = 3.0f;

        startPosition = player.transform.position;

        cnt = 0;
        flag = 1;

        if (flip) dir = -1;
        else dir = 1;

    }

    private void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        // �÷��̾�Լ� ����Ǿ� �־���
        if (flag == 1)
        {
            if ((startPosition.x + (attackRange * dir)) != this.transform.position.x && cnt < (attackRange * 50))
            {
                gameObject.transform.Translate(-0.02f * dir, 0, 0);
                cnt++;
            }
            else
            {
                // ���� ��ȯ
                flag = 2;
                gameObject.transform.localScale = new Vector3(-2 * dir, 2, 2);
            }
        }

        // ���� �÷��̾��� ��ġ�� ���ƿ�
        else if (flag == 2)
        {
            if (startPosition.x != this.transform.position.x && cnt < (attackRange * 50 * 2))
            {
                gameObject.transform.Translate(0.02f * dir, 0, 0);
                cnt++;
            }
            else
            {
                // ��
                Destroy(gameObject);
            }
        }
    }
}
