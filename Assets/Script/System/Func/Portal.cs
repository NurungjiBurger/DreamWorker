using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private GameObject portalIcon;

    private GameObject miniMapPortalIcon;
    private GameObject player;

    private Portal connectPortal;


    public GameObject MiniMapPortalIcon { get { return miniMapPortalIcon; } }
    public Vector3 ConnectPosition { get { return connectPortal.transform.position; } }

    // 연결된 포탈의 위치 저장
    public void PositionSave(Portal portal)
    {
        connectPortal = portal;
    }

    private void Start()
    {
        // 미니맵에 포탈 아이콘 생성
        if (portalIcon)
        {
            miniMapPortalIcon = Instantiate(portalIcon, transform.position, Quaternion.identity);
            miniMapPortalIcon.transform.SetParent(GameObject.Find("Canvas").transform.Find("MiniMap").transform.Find("Background"));
            miniMapPortalIcon.GetComponent<Icon>().obj = gameObject;
        }
    }

    private void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player").gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
