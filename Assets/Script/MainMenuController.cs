using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject newButton;
    [SerializeField]
    private GameObject loadButton;
    [SerializeField]
    private GameObject exitButton;

    public void NewButton()
    {
        Debug.Log("new");
    }

    public void LoadButton()
    {
        Debug.Log("Load");
    }

    public void ExitButton()
    {
        Debug.Log("Exit");
    }


    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) SceneManager.LoadScene("Dungeon");
    }
}
