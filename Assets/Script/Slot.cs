using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour , IPointerClickHandler//, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private GameObject prefabUI;

    private GameObject UI;

    private Sprite itemImage;

    private GameObject player;

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void InsertImage(GameObject item)
    {
        itemImage = item.GetComponent<SpriteRenderer>().sprite;
        transform.Find("Item").GetComponent<Image>().sprite = itemImage;
    }

    public void ConductCommand(string command)
    {
        // ÀåÂø
        if (command == "Equip")
        {
            Debug.Log("equip");
            player.GetComponent<PlayerStatus>().EquipItem(transform.GetSiblingIndex());
        }
        // ¹ö¸®±â
        else if (command == "Discard")
        {
            Debug.Log("discard");
            player.GetComponent<PlayerStatus>().DiscardItem(transform.GetSiblingIndex());
            DestroyObject();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // UI ¶ç¿ì±â
           // Debug.Log(transform.GetSiblingIndex());
            UI = Instantiate(prefabUI, GameObject.Find("Canvas").transform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");
    }

}
