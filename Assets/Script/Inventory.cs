using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabSlot;

    private List<GameObject> possessItemList = new List<GameObject>();
    private GameObject player;
    
    public void SEND()
    {
        Debug.Log("°í");
    }

    public void AddToInventory()
    {
        Instantiate(prefabSlot, transform).GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        possessItemList = player.GetComponent<PlayerStatus>().PossessItemList;
    }
}
