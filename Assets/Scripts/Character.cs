﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    //constants
    protected const float groundDistance = 0.6f;
    protected const int doubleJumpsCount = 1;

    //public variables
    public float health;
    public float maxHealth;
    public float moveSpeed;
    public float maxCharacterSpeed = 5;
    public float jumpSpeed;
    public float jumpCooldown;
    public Attack attackObject;
    public HUD hud;

    //protected variables
    protected float lastJumpTime;
    protected SpriteRenderer sprite;
    protected bool isGrounded = true;
    protected bool canMove = true;
    [SerializeField] protected LayerMask collisionMask;
    protected int doubleJumpsRemaining;
    protected Animator animator;
    protected Rigidbody2D body;

    protected void LoadCharacter()//call in the Start function
    {
        doubleJumpsRemaining = doubleJumpsCount;
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (hud != null) SetHUD(hud);
    }

    protected void GroundCheck()
    {
        if (Time.time < lastJumpTime + jumpCooldown || attackObject.isAttacking) return;
        isGrounded = Physics2D.Raycast(body.position, Vector2.down, groundDistance, collisionMask);
        Debug.DrawRay(body.position, Vector3.down * groundDistance, Color.green, 0);
        if (isGrounded)
        {
            AnimateGrounded();
            doubleJumpsRemaining = doubleJumpsCount;
        }
        else
        {
            AnimateJump();
        }
    }

    protected void MoveLeft()
    {
        if (attackObject.isAttacking || !canMove) return;
        if (body.velocity.x > -maxCharacterSpeed) body.velocity = new Vector2(-moveSpeed+body.velocity.x, body.velocity.y);
        if (body.velocity.x < -maxCharacterSpeed) body.velocity = new Vector2(-maxCharacterSpeed, body.velocity.y);
        sprite.flipX = false;
        attackObject.LookLeft();
        animator.SetInteger("AnimState", 2);
    }

    protected void MoveRight()
    {
        if (attackObject.isAttacking || !canMove) return;
        if (body.velocity.x < maxCharacterSpeed) body.velocity = new Vector2(moveSpeed+body.velocity.x, body.velocity.y);
        if (body.velocity.x > maxCharacterSpeed) body.velocity = new Vector2(maxCharacterSpeed, body.velocity.y);
        sprite.flipX = true;
        attackObject.LookRight();
        animator.SetInteger("AnimState", 2);
    }

    protected void Jump()
    {
        if (doubleJumpsRemaining > 0 && Time.time > lastJumpTime + jumpCooldown && canMove)
        {
            lastJumpTime = Time.time;
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            if (isGrounded)
            {
                doubleJumpsRemaining = doubleJumpsCount;
            }
            else
            {
                doubleJumpsRemaining--;
            }
        }
    }

    public virtual void Attack()
    {
        if (attackObject.isAttacking || !canMove) return;
        AnimateAttack();
        attackObject.StartAttack();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        AnimateHurt();
        UpdateHUD();
        if (health <=0)
        {
            health = 0;
            Death();
        }
        Debug.Log(name + " has "+health+" health");
    }

    public void TakeKnockback(Vector3 source, float strength)
    {
        Vector3 direction = transform.position - source;
        body.AddForce((direction.normalized + Vector3.up/5) * strength, ForceMode2D.Impulse);
    }

    public void Death()
    {
        Debug.Log(name + " has died");
        AnimateDeath();
        canMove = false;
        CapsuleCollider2D capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        capsuleCollider2D.size = new Vector2(0.8f, 0.2f);
        capsuleCollider2D.direction = CapsuleDirection2D.Horizontal;
        capsuleCollider2D.offset = Vector2.up/5;
        Destroy(gameObject, 5);
    }

    protected virtual void AnimateGrounded()
    {
        if (attackObject.isAttacking) return;
        animator.SetBool("Grounded", true);
    }

    protected virtual void AnimateJump()
    {
        if (attackObject.isAttacking) return;
        animator.SetBool("Grounded", false);
        //animator.SetBool("Jump", true);
    }

    protected virtual void AnimateAttack()
    {
        if (attackObject.isAttacking) return;
        animator.SetBool("Attack", true);
        animator.SetBool("Grounded", true);
    }

    protected virtual void AnimateHurt()
    {
        if (attackObject.isAttacking) return;
        animator.SetBool("Hurt", true);
        animator.SetBool("Grounded", true);
    }

    protected virtual void AnimateDeath()
    {
        animator.SetBool("Death", true);
    }

    protected void SetHUD(HUD newhud)
    {
        hud = newhud;
        hud.SetMax(maxHealth);
    }

    protected void UpdateHUD()
    {
        if (hud != null)
        {
            hud.UpdateHealth(health);
        }
    }
}
