using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Icon : MonoBehaviour
{
    private GameObject room;
    public GameObject obj;

    private GameController gameController;

    // Ȱ��ȭ�� �� ã��
    private void RoomFinder()
    {
        int idx;

        for (idx = 0; idx < gameController.Room.Count; idx++)
        {
            if (gameController.Room[idx].GetComponent<Room>().isPlayer)
            {
                room = gameController.Room[idx];
                break;
            }
        }
    }

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (!gameController) gameController = GameObject.Find("GameController").GetComponent<GameController>();
        else
        {
            if (gameController.Room.Count != 0) RoomFinder();      
        }

        Vector3 position;

        // Ȱ��ȭ�� ��� ������Ʈ�� ã�Ƴ����ٸ�
        if (obj && room)
        {
            // ������Ʈ ��ġ - Ȱ��ȭ�� ���� ��ǥ�� �̴ϸʿ����� ��ǥ�� ġȯ
            position = obj.transform.position - room.transform.position;

            position.x *= 20.8f;
            position.y *= 16.25f;

            position.x %= 250;
            position.y %= 130;

            GetComponent<RectTransform>().localPosition = position;
        }
    }
}
