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
    public float moveSpeed;
    public float maxCharacterSpeed = 5;
    public float jumpSpeed;
    public float jumpCooldown;
    public Animator animator;

    //protected variables
    protected float lastJumpTime;
    protected SpriteRenderer sprite;
    protected bool isGrounded = true;
    protected bool isAttacking = false;
    [SerializeField] protected LayerMask collisionMask;
    protected int doubleJumpsRemaining;
    protected Rigidbody2D body;



    protected void LoadCharacter()//call in the Start function
    {
        doubleJumpsRemaining = doubleJumpsCount;
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected void GroundCheck()
    {
        if (Time.time < lastJumpTime + jumpCooldown) return;
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
        if (body.velocity.x > -maxCharacterSpeed) body.velocity = new Vector2(-moveSpeed+body.velocity.x, body.velocity.y);
        if (body.velocity.x < -maxCharacterSpeed) body.velocity = new Vector2(-maxCharacterSpeed, body.velocity.y);
        sprite.flipX = false;
        animator.SetInteger("AnimState", 2);
    }

    protected void MoveRight()
    {
        if (body.velocity.x < maxCharacterSpeed) body.velocity = new Vector2(moveSpeed+body.velocity.x, body.velocity.y);
        if (body.velocity.x > maxCharacterSpeed) body.velocity = new Vector2(maxCharacterSpeed, body.velocity.y);
        sprite.flipX = true;
        animator.SetInteger("AnimState", 2);
    }

    protected void Jump()
    {
        if (doubleJumpsRemaining > 0 && Time.time > lastJumpTime + jumpCooldown)
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

    protected virtual void Attack()
    {
        animator.SetBool("Attack", true);
        //do attack
    }


    protected virtual void AnimateGrounded()
    {
        animator.SetBool("Grounded", true);
    }

    protected virtual void AnimateJump()
    {
        animator.SetBool("Grounded", false);
        animator.SetBool("Jump", true);
    }
}
