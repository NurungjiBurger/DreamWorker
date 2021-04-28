using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    enum Direction { Left, Right, Up, Stop };

    private Direction dir = Direction.Stop;

    private bool isGround = true;
    private bool jumping = false;

    private bool isMove = false;
    private int moveRandom;

    private GameObject player;
    private Animator animator;

    [SerializeField]
    private float recognitionRange;
    [SerializeField]
    private GameObject prefabTimer;

    private Timer moveTimer;

  

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        moveTimer = Instantiate(prefabTimer).GetComponent<Timer>();
        moveTimer.SetCooldown(1.0f);
    }
    private void FixedUpdate()
    {
        if (!GetComponent<MonsterAttack>().Attack) // ���Ͱ� �������� �ƴ϶�� ���� ����
        {
            switch (dir)
            {
                case Direction.Right:
                    GetComponent<SpriteRenderer>().flipX = true;
                    animator.SetBool("move", true);
                    transform.Translate(0.005f * GetComponent<MonsterStatus>().MoveSpeed, 0, 0);
                    break;
                case Direction.Left:
                    GetComponent<SpriteRenderer>().flipX = false;
                    animator.SetBool("move", true);
                    transform.Translate(-0.005f * GetComponent<MonsterStatus>().MoveSpeed, 0, 0);
                    break;
                case Direction.Up:
                    break;
                case Direction.Stop:
                default:
                    //GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0f);
                    break;
            }

            if (jumping)
            {
                jumping = false;
                isGround = false;
                transform.Translate(0, 0.005f * GetComponent<MonsterStatus>().JumpPower, 0);
            }
        }

        // �ӵ� ����
        if (GetComponent<Rigidbody2D>().velocity.x >= 1.5f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(1.5f, GetComponent<Rigidbody2D>().velocity.y);
        }
        else if (GetComponent<Rigidbody2D>().velocity.x <= -1.5f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-1.5f, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
       Debug.Log(isMove + "     " + transform.position.x + "    " + moveRandom + "   " +  GetComponent<MonsterAttack>().Attack);

        if (moveTimer.CooldownCheck()) isMove = false;
        
        // �ν� ���� ��
        if (Vector3.Distance(player.transform.position, transform.position) <= recognitionRange)
        {
           // Debug.Log("������");
            if (player.transform.position.x < transform.position.x) moveRandom = 1;
            else if (player.transform.position.x > transform.position.x) moveRandom = 0;
            else moveRandom = 3;

            //if (player.transform.position.y > transform.position.y) moveRandom = 2;
        }
        // �ν� ���� ���϶�
        else
        {
            //Debug.Log("�����ð�");
            // �����̰� ���� ������ 1�� ���� ��� �������� ������
            if (!isMove)    
            {
                moveRandom = Random.Range(0, 4);
                moveTimer.TimerSetZero();
                isMove = true;
            }
        }

        // �� �������� ��������!
        switch (moveRandom)
        {
            case 0:
                dir = Direction.Right;
                break;
            case 1:
                dir = Direction.Left;
                break;
            case 2:
                if (isGround)
                {
                    dir = Direction.Up;
                    jumping = true;
                }
                break;
            case 3:
            default:
                dir = Direction.Stop;
                animator.SetBool("move", false);
                break;
        }
    }
}
