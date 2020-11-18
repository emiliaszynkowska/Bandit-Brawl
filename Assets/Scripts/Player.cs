using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Inherits from Character class
public class Player : Character
{
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        LoadCharacter();
        sound.PlayMusic();
    }


    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        PlayerJump();
        PlayerAttack();
        PlayerBlock();
        LowHealthCheck();
    }

    // FixedUpdate is called for physics
    private void FixedUpdate()
    {
        CameraFollow();
        PlayerMove();
    }

    void PlayerMove()
    {
        float horizontalmove = Input.GetAxis("Horizontal");
        float verticalmove = Input.GetAxis("Vertical");
        if (horizontalmove > 0)
        {
            MoveRight();
        }

        if (horizontalmove < 0)
        {
            MoveLeft();
        }
        else if (horizontalmove == 0)
        {
            AnimateStay();
            body.velocity = new Vector2(body.velocity.x * 0.9f, body.velocity.y);
        }
    }

    void CameraFollow()
    {
        cam.transform.position = transform.position + Vector3.back;
    }

    void PlayerJump()
    {
        if (Input.GetButtonDown("Jump"))
            Jump();
    }

    void PlayerAttack()
    {
        if (Input.GetButtonDown("Fire1"))
            Attack();
    }


    void PlayerBlock()
    {
        if (Input.GetButton("Fire2"))
            StartBlock();
        else
            EndBlock();
    }

    void LowHealthCheck()
    {
        if (health <= 20)
            if (hud != null)
                hud.LowHealth();
    }
    
}
