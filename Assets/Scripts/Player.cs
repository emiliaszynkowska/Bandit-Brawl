using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inherits from Character class
public class Player : Character
{
    Rigidbody2D body;
    public Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        jumpsRemaining = doubleJumpsCount;
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        PlayerJump();
    }

    void GroundCheck()
    {
        if (Time.time < lastJumpTime + jumpCooldown) return;
        isGrounded = Physics2D.Raycast(body.position, Vector2.down, groundDistance ,collisionMask);
        Debug.DrawRay(body.position, Vector3.down* groundDistance, Color.green, 0);
        Debug.Log(isGrounded);
        if (isGrounded)
        {
            AnimateGrounded();
            jumpsRemaining = doubleJumpsCount;
        }
        else
        {
            AnimateJump();
        }
    }

    private void AnimateGrounded()
    {
        animator.SetBool("Grounded", true);
        animator.SetFloat("AirSpeed", 0);
    }

    private void AnimateJump()
    {
        animator.SetBool("Grounded", false);
        animator.SetBool("Jump", true);
        animator.SetFloat("AirSpeed", body.velocity.magnitude);
    }

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
            body.velocity = new Vector2(moveSpeed,body.velocity.y);
            sprite.flipX = true;
            animator.SetInteger("AnimState", 2);
        }
        if (horizontalmove < 0)
        {
            body.velocity = new Vector2(-moveSpeed, body.velocity.y);
            sprite.flipX = false;
            animator.SetInteger("AnimState", 2);
        }
        else if (horizontalmove == 0)
        {
            animator.SetInteger("AnimState", 0);
        }
        

    }

    void CameraFollow()
    {
        cam.transform.position = transform.position + Vector3.back;
    }

    void PlayerJump()
    {
        if (jumpsRemaining>0 && Input.GetButtonDown("Jump") && Time.time > lastJumpTime + jumpCooldown)
        {
            lastJumpTime = Time.time;
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            jumpsRemaining--;
        }
    }
}
