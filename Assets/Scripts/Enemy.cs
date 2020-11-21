using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public GameObject player;
    private float attackCooldown = 3;
    private float lastAttackTime = 10;

    // Start is called before the first frame update
    protected void Start()
    {
        LoadCharacter();
    }

    // Update is called once per frame
    protected void Update()
    {
        GroundCheck();
        EnemyAttack();
    }

    protected void FixedUpdate()
    {
        EnemyMove();
    }

    protected void EnemyMove()
    {
        if (!attackObject.isAttacking)
        {
            var playerposX = player.transform.position.x;
            var enemyposX = body.position.x;
            if (playerposX - enemyposX > 2)
                MoveRight();
            else if (playerposX - enemyposX < -2)
                MoveLeft();
            else
            {
                AnimateStay();
                StartBlock();
            }
        }
    }
    
    protected void EnemyAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown ) return;
        if (!attackObject.isAttacking && (player.transform.position - transform.position).magnitude <= 2)
        {
            EndBlock();
            Attack();
            lastAttackTime = Time.time;
        }
    }
}