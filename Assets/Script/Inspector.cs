using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inspector : MonoBehaviour
{

    private GameObject player;

    public void StatusText()
    {
        transform.Find("Background").transform.Find("Infomation").transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level  " + player.GetComponent<PlayerStatus>().Level.ToString();
        transform.Find("Background").transform.Find("Infomation").transform.Find("Occupation").GetComponent<TextMeshProUGUI>().text = player.GetComponent<PlayerStatus>().Occupation;

        transform.Find("Background").transform.Find("Status").transform.Find("MaxHP").GetComponent<TextMeshProUGUI>().text = "MaxHP  " + player.GetComponent<PlayerStatus>().MaxHP.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("AttackPower").GetComponent<TextMeshProUGUI>().text = "Power  " + player.GetComponent<PlayerStatus>().Power.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("JumpPower").GetComponent<TextMeshProUGUI>().text = "JumpPower  " + player.GetComponent<PlayerStatus>().JumpPower.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("MoveSpeed").GetComponent<TextMeshProUGUI>().text = "MoveSpeed  " + player.GetComponent<PlayerStatus>().MoveSpeed.ToString();
        transform.Find("Background").transform.Find("Status").transform.Find("DefenseCapability").GetComponent<TextMeshProUGUI>().text = "Defense  " + player.GetComponent<PlayerStatus>().Defense.ToString();
        //transform.Find("Background").transform.Find("Status").transform.Find("BloodAbsorption").GetComponent<TextMeshProUGUI>().text = player.GetComponent<PlayerStatus>().BloodAbsorption.ToString();
        //transform.Find("Background").transform.Find("Status").transform.Find("Evasion").GetComponent<TextMesh>().text = player.GetComponent<PlayerStatus>().Evasion.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player");

        StatusText();
    }
}
