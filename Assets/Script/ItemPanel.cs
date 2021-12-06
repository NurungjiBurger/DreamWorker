using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject inventory;
    [SerializeField]
    private GameObject inspector;
    [SerializeField]
    private GameObject enhancer;

    private GameObject player;
    private Slot selectSlot;
    private Slot devoteSlot;

    public void AllocateSlot(Slot slot)
    {
        selectSlot = slot;
    }

    public void Enhance()
    {
        transform.GetChild(0).GetComponent<ButtonUI>().UIActive();
        
        enhancer.GetComponent<Enhancer>().Enroll(selectSlot, true);
        enhancer.GetComponent<Enhancer>().Enroll(devoteSlot, false);
    }

    public void ItemInform()
    {
        selectSlot.GetComponent<Slot>().ActiveItemInform();
        gameObject.SetActive(false);
    }

    public void ItemDisassembly()
    {
        selectSlot.GetComponent<Slot>().DestroyObject();
        player.GetComponent<PlayerStatus>().Data.itemPiece++;
        gameObject.SetActive(false);
    }

    void Start()
    {

    }

    private void OnDisable()
    {
        selectSlot = null;
        devoteSlot = null;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        if (!inventory) inventory = GameObject.Find("Canvas").transform.Find("Inventory").gameObject;
        if (!inspector) inspector = GameObject.Find("Canvas").transform.Find("Inspector").gameObject;
        if (!enhancer) enhancer = GameObject.Find("Canvas").transform.Find("Enhancer").gameObject;
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        if (selectSlot.transform.parent.parent == inspector.transform && selectSlot.GetComponent<Slot>().SlotItem.GetComponent<ItemStatus>().Data.enhancingLevel < 6)
        {
            if (selectSlot && devoteSlot == null)
            {
                int size = 0;
                size = inventory.GetComponent<Inventory>().ItemCount;

                for (int i = 0; i < size; i++)
                {
                    if (inventory.transform.GetChild(0).GetChild(i).GetComponent<Slot>().SlotItem.name == selectSlot.GetComponent<Slot>().SlotItem.name)
                    {
                        devoteSlot = inventory.transform.GetChild(0).GetChild(i).gameObject.GetComponent<Slot>();
                        transform.GetChild(0).gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }

    }
}
