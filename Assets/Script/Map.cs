using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Map : MonoBehaviour
{
    private GameObject room;
    [SerializeField]
    private List<Vector3> position = new List<Vector3>();

    private GameObject tester;
    private bool done = false;

    public bool Done { get { return done; } }
    public GameObject Room { set { room = value; } }
    public List<Vector3> SafePosition { get { return position; } }

    private void FindSafePosition()
    {
        if (!tester.GetComponent<TesterSensor>().PositionSave)
        {
            tester.GetComponent<TesterSensor>().PositionSave = true;
            tester.transform.position = new Vector3((float)(transform.position.x + Random.Range(-11, 12)), (float)(transform.position.y + Random.Range(-6, 8)), transform.position.z);
        }
        else
        {
            if (position.Count == 0) position.Add(tester.transform.position);
            else
            {
                for (int i = 0; i < position.Count; i++)
                {
                    if (position[i] == tester.transform.position)
                    {
                        tester.GetComponent<TesterSensor>().PositionSave = false;
                        return;
                    }
                }
                position.Add(tester.transform.position);
            }
            tester.GetComponent<TesterSensor>().PositionSave = false;
        }
    }

    private void Start()
    {
        tester = new GameObject("Tester");
        tester.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        tester.AddComponent<BoxCollider2D>();
        tester.GetComponent<BoxCollider2D>().isTrigger = true;
        tester.AddComponent<Rigidbody2D>();
        tester.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        tester.AddComponent<TesterSensor>();
    }

    private void FixedUpdate()
    {
        if (room)
        {
            if (position.Count < 20)
            {
                FindSafePosition();
            }
            else
            {
                done = true;
                Destroy(tester.gameObject);
            }
        }
    }

    private void Update()
    {
        if (!room) room = GameObject.Find("GameController").GetComponent<GameController>().Room[transform.GetSiblingIndex()];
    }
}
