using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabUI;

    private GameObject slotItem;

    private Sprite itemImage;

    private GameObject player;
    private GameObject inventory;
    private GameObject inspector;

    public GameObject SlotItem { get { return slotItem; } }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void Discard()
    {
        Debug.Log("버린다");
    }

    public void Mounting() // toinspector가 트루
    {
        //Debug.Log("mount");
        transform.SetParent(inspector.transform.Find(slotItem.GetComponent<ItemStatus>().MountingPart).transform);
        transform.position = transform.parent.position;
        player.GetComponent<PlayerStatus>().CalCulateStat(slotItem, 1);
        inventory.GetComponent<Inventory>().DiscardItem(transform.GetSiblingIndex());

        if (inspector.transform.Find(slotItem.GetComponent<ItemStatus>().MountingPart).childCount == 2)
        {
            //Debug.Log("교환");
            GameObject tmp;
            tmp = inspector.transform.Find(slotItem.GetComponent<ItemStatus>().MountingPart).GetChild(0).gameObject;
            tmp.transform.SetParent(inventory.transform);
            player.GetComponent<PlayerStatus>().CalCulateStat(tmp.GetComponent<Slot>().SlotItem, -1);
            inventory.GetComponent<Inventory>().MoveItem(tmp.GetComponent<Slot>().SlotItem);
        }
    }

    public void DisMounting() // toinventory가 트루
    {
        Debug.Log("dismount");
        transform.SetParent(inventory.transform);
        player.GetComponent<PlayerStatus>().CalCulateStat(slotItem, -1);
        inventory.GetComponent<Inventory>().MoveItem(slotItem);
    }


    public void InsertImage(GameObject item)
    {
        slotItem = item;
        transform.Find("Item").GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory(Clone)").transform.Find("InventoryBackground").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector(Clone)").transform.Find("Background").gameObject;

        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 왼쪽 버튼을 눌렀을 때의 처리
        }

        if (Input.GetMouseButton(0))
        {
            // 마우스 왼쪽 버튼을 누르고 있는 도중의 처리
        }

        if (Input.GetMouseButtonUp(0))
        {
           // Debug.Log(GetObject());
            // 마우스 왼쪽 버튼을 뗄 때의 처리
        }
    }
}
