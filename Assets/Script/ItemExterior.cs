using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExterior : MonoBehaviour
{
    [SerializeField]
    private GameObject handBone;

    private GameObject hand;

    void Start()
    {
        hand = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>().HandBone;
    }

    void Update()
    {
        // ������ ���� �������� ���
        if (GetComponent<ItemStatus>().Data.isMount)
        {
            // �÷��̾��� �տ� �ش��ϴ� ���� ��ġ, ȸ�� ���� �״�� ����
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<Collider2D>().isTrigger = true;
            transform.position = hand.transform.position;
            GetComponent<ObjectFlip>().flip('x', GameObject.FindGameObjectWithTag("Player").GetComponent<ObjectFlip>().flipX);
            if (GetComponent<ItemStatus>().AttackType != "Sting")
            {
                transform.rotation = hand.transform.rotation;
                GetComponent<ObjectFlip>().flip('x', !GameObject.FindGameObjectWithTag("Player").GetComponent<ObjectFlip>().flipX);
            }

        }
    }
}
