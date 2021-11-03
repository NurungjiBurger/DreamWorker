using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Icon : MonoBehaviour
{
    private GameController gameController;
    private GameObject room;

    public GameObject obj;

    private void RoomFinder()
    {
        int idx;

        for (idx = 0; idx < gameController.EventRoom.Count; idx++)
        {
            if (gameController.EventRoom[idx].transform.GetChild(0).GetComponent<Room>().isPlayer)
            {
                room = gameController.EventRoom[idx];
                break;
            }
        }
        for (idx = 0; idx < gameController.Room.Count; idx++)
        {
            if (gameController.Room[idx].GetComponent<Room>().isPlayer)
            {
                room = gameController.Room[idx];
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController) gameController = GameObject.Find("GameController").GetComponent<GameController>();
        else
        {
            if (gameController.Room.Count != 0) RoomFinder();      
        }

        Vector3 position;

        if (obj && room)
        {
            position = obj.transform.position - room.transform.position;

            position.x *= 20.8f;
            position.y *= 16.25f;

            position.x %= 250;
            position.y %= 130;

            GetComponent<RectTransform>().localPosition = position;
        }
    }
}
