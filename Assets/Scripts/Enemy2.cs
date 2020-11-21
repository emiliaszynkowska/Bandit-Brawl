using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Character
{
    public GameObject player;

    // Start is called before the first frame update
    protected void Start()
    {
        LoadCharacter();
    }

    // Update is called once per frame
    protected void Update()
    {
        GroundCheck();
    }

    protected void FixedUpdate()
    {
        EnemyMove();
    }

    protected void EnemyMove()
    {
        EnemyAttack();
        var playerposX = player.transform.position.x;
        var enemyposX = body.position.x; 
        var playerposY = player.transform.position.y;
        var enemyposY = body.position.y;
        if (playerposY - enemyposY > 2)
        {
            if (CanJump()) StartCoroutine(DoubleJump());
        }

        if (playerposX - enemyposX > 2)
            MoveRight();
        else if (playerposX - enemyposX < -2)
            MoveLeft();
        else
        {
            AnimateStay();
            body.velocity = new Vector2(body.velocity.x * 0.9f, body.velocity.y);
            if (playerposY - enemyposY < -3)
            {
                Attack();
            }
            else {
                StartCoroutine(EnemyBlock());
            }
        }
    }

    protected void EnemyAttack()
    {
        if (attackObject.CanAttack())
        {
            Attack();
        }
    }

    IEnumerator DoubleJump()
    {
        Jump();
        yield return new WaitForSeconds(0.31f);
        Jump();
    }

    IEnumerator EnemyBlock()
    {
        StartBlock();
        yield return new WaitForSeconds(3);
        if (isBlocking) EndBlock();
    }

    bool CanJump()
    {
        var height = 10;
        Debug.DrawRay(body.position, Vector3.up * height, Color.green, 0);
        return Physics2D.Raycast(body.position, Vector2.up, height, collisionMask);
    }
}
