using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayThrow : Projectile
{
    protected void OnGoingOneWay()
    {
        GetComponent<SpriteRenderer>().flipX = flip;

        if (flip) dir = -1;
        else dir = 1;

        if (cnt < (attackRange * 60))
        {
            if ((startPosition.x + (attackRange * dir)) != this.transform.position.x)
            {
                gameObject.transform.Translate(-0.02f * dir, 0, 0);
                cnt++;
            }
        }
        else Destroy(gameObject);
    }

    protected void SetPosition()
    {
        startPosition = transform.position;
        cnt = 0;
    }

    // Start is called before the first frame update
    private void Start()
    {
    
    }

    // Update is called once per frame
    private void Update()
    {
     
    }
}
