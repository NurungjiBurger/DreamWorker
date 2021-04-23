using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    public P_info player;
    private void checkdelete()
    {

        if(player.Getportal() && player.Getinputup())
        {
            player.Setgonext(true);
            Destroy(gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<P_info>();
    }

    // Update is called once per frame
    void Update()
    {
        checkdelete();
    }
}
