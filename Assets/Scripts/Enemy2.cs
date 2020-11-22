using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Character
{
    public GameObject player;
    protected bool doingAction = false;

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
        if (player != null) EnemyMove();
    }

    protected void EnemyMove()
    {
        if (attackObject.CanAttack())
        {
            Attack();
            return;
        }
        var playerposX = player.transform.position.x;
        var enemyposX = body.position.x; 
        var playerposY = player.transform.position.y;
        var enemyposY = body.position.y;
        if (playerposY - enemyposY > 2)
        {
            if (!doingAction && CanJump())
            {
                doingAction = true;
                StartCoroutine(DoubleJump());
            }
        }

        if (playerposX - enemyposX > 2)
        {
            MoveRight();
        }
        else if (playerposX - enemyposX < -2)
        {
            MoveLeft();
        }
        else
        {
            AnimateStay();
            body.velocity = new Vector2(body.velocity.x * 0.9f, body.velocity.y);
            if (playerposY - enemyposY > 3)
            {
                if (body.velocity.y < 0)
                {
                    Attack();
                }
                 else
                {
                    if (CanJump()) Jump();
                }
            }
            else {
                if (body.velocity.y < 0)
                {
                    Attack();
                }
                if (!doingAction)
                {
                    doingAction = true;
                    StartCoroutine(EnemyBlock());
                }
            }
        }
    }

    IEnumerator DoubleJump()
    {
        Jump();
        yield return new WaitForSeconds(0.31f);
        Jump();
        doingAction = false;
    }

    IEnumerator EnemyBlock()
    {
        StartBlock();
        yield return new WaitForSeconds(3);
        if (isBlocking) EndBlock();
        doingAction = false;
    }

    bool CanJump()
    {
        var height = 4;
        var position = body.position + Vector2.up*3.5f;
        Debug.DrawRay(position, Vector3.up * height, Color.green, 0);
        bool val = !Physics2D.Raycast(position, Vector2.up, height, collisionMask);
        Debug.Log(val);
        return val;
    }
}
