using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Map : MonoBehaviour
{
    private GameObject room;
    [SerializeField]
    private List<Vector3> monsterPosition = new List<Vector3>();
    [SerializeField]
    private List<float> portalPosition = new List<float>();
    /*
    [SerializeField]
    private List<Vector3> portalPosition = new List<Vector3>();
    */
    // 0 ~ 3 right
    // 4 ~ 7 left
    // 8 ~ 11 up
    // 12 ~ 15 down

    private GameObject tester;
    private bool done = false;

    public bool Done { get { return done; } }
    public GameObject Room { set { room = value; } }
    public List<Vector3> SafeMonsterPosition { get { return monsterPosition; } }
    public List<float> SafePortalPosition { get { return portalPosition; } }

    private void FindSafePosition(List<Vector3> position)
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

    private void Awake()
    {
        transform.SetParent(GameObject.Find("Grid").transform);

        tester = new GameObject("Tester");
        tester.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        tester.AddComponent<BoxCollider2D>();
        tester.GetComponent<BoxCollider2D>().isTrigger = true;
        tester.AddComponent<Rigidbody2D>();
        tester.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        tester.AddComponent<TesterSensor>();

        Update();
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {

    }

    private void Update()
    {
        while (!done)
        {
            if (!room) room = GameObject.Find("GameController").GetComponent<GameController>().Room[transform.GetSiblingIndex()];
            else
            {
                if (monsterPosition.Count < 20)
                {
                    FindSafePosition(monsterPosition);
                }
                else
                {
                    done = true;
                    Destroy(tester.gameObject);
                }
            }
        }
    }

}
