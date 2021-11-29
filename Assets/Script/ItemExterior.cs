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
        // 장착된 무기 아이템의 경우
        if (GetComponent<ItemStatus>().Data.isMount)
        {
            // 플레이어의 손에 해당하는 뼈의 위치, 회전 등을 그대로 따라감
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
