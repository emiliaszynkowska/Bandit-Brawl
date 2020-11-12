using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // Start is called before the first frame update
    void Start()
    {
        LoadCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        EnemyMove();
    }

    void EnemyMove()
    {
        //enemy does stuff here idk
        body.velocity = new Vector2(body.velocity.x * 0.9f, body.velocity.y);
        AnimateStay();
        StartBlock();
    }
}
