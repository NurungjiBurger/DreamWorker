using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeSelecter : MonoBehaviour
{
    private GameObject inventory;
    private GameObject inspector;

    [SerializeField]
    private GameObject prefabSlotButton;

    public void LoadAllItem()
    {
        // 장비창과 인벤에서 모든 아이템을 불러와 나열.
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").transform.Find("Background").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").transform.Find("Background").gameObject;
        //if (!exchanger) exchanger = GameObject.Find("Canvas").transform.Find("Exchanger").transform.Find("Background").gameObject;
    }
}
