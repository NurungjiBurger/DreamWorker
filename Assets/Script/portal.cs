using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    public character player;
    private void checkdelete()
    {

        if(player.portal && player.inputUp)
        {
            player.gonext = true;
            Destroy(gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<character>();
    }

    // Update is called once per frame
    void Update()
    {
        checkdelete();
    }
}
