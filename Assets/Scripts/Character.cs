using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    //constants
    protected const float groundDistance = 0.6f;
    protected const int doubleJumpsCount = 1;

    //public variables
    public float health = 100;
    public float maxHealth = 100;
    public float moveSpeed = 0.5f;
    public float maxCharacterSpeed = 5;
    public float jumpSpeed = 14;
    public float jumpCooldown = 0.3f;
    public float blockCooldown = 3;
    public Attack attackObject;
    public HUD hud;
    public GameObject slamParticles;
    public bool canMove = true;

    //protected variables
    protected float lastJumpTime = -10;
    protected float lastBlockTime = -10;
    protected SpriteRenderer sprite;
    protected bool isGrounded = true;
    protected bool isBlocking = false;
    [SerializeField] protected LayerMask collisionMask;
    protected int doubleJumpsRemaining;
    protected Animator animator;
    protected Vector2 slamSize = new Vector2(4, 2);
    protected float slamKnockback = 15;
    protected float slamDamage = 5f;
    [SerializeField] protected SoundManager sound;

    protected Rigidbody2D body;

    protected void LoadCharacter()//call in the Start function
    {
        doubleJumpsRemaining = doubleJumpsCount;
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        SetHUD(hud);
        sound = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    protected void GroundCheck()
    {
        if (Time.time < lastJumpTime + jumpCooldown ) return;
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
        if (attackObject.isAttacking || !canMove || isBlocking) return;
        if (body.velocity.x > -maxCharacterSpeed) body.velocity = new Vector2(-moveSpeed+body.velocity.x, body.velocity.y);
        if (body.velocity.x < -maxCharacterSpeed) body.velocity = new Vector2(-maxCharacterSpeed, body.velocity.y);
        sprite.flipX = false;
        attackObject.LookLeft();
        animator.SetInteger("AnimState", 2);
    }

    protected void MoveRight()
    {
        if (attackObject.isAttacking || !canMove || isBlocking) return;
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
            sound.PlayJump();
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
        if (attackObject.isAttacking || !canMove || isBlocking) return;
        if (!isGrounded && body.velocity.y < 0)
        {
            AirAttack();
        }
        else
        {
            AnimateAttack();
            attackObject.StartAttack();
            sound.PlayWeaponSwing();
        }
    }

    public virtual void AirAttack()
    {
        sound.PlayWeaponSwing();
        attackObject.isAttacking = true;
        StartCoroutine(Slam());
    }

    IEnumerator Slam()
    {
        while (!isGrounded)
        {
            body.velocity = new Vector2(0, -jumpSpeed*1.5f);
            yield return null;
        }
        Instantiate(slamParticles, transform);
        Collider2D[] results = new Collider2D[8];
        results = Physics2D.OverlapBoxAll(transform.position,slamSize,0);
        foreach (Collider2D col in results)
        {
            if (col != null)
            {
                Character c = col.gameObject.GetComponent<Character>();
                if (c != null && !c.Equals(this))
                {
                    c.TakeDamage(slamDamage);
                    c.TakeKnockback(transform.position, slamKnockback);
                }
            }
        }
        lastJumpTime = Time.time;
        sound.PlaySlam();
        yield return new WaitForSeconds(jumpCooldown);
        attackObject.isAttacking = false;
    }

    public virtual void StartBlock()
    {
        if (attackObject.isAttacking || !canMove) return;
        if (Time.time > lastBlockTime + blockCooldown)
        {
            if (!isBlocking) sound.PlayBlock();
            isBlocking = true;
            AnimateStay();
        }
        else
        {
            isBlocking = false;
        }

    }

    public virtual void EndBlock()
    {
        if (!isBlocking) return;
        isBlocking = false;
        sound.PlayBlock();
        AnimateStay();
    }

    public void TakeDamage(float damage)
    {
        if (!isBlocking)
        {
            health -= damage;
            sound.PlayDamage();
            AnimateHurt();
        }
        else
        {
            sound.PlayBlockedDamage();
        }
        lastBlockTime = Time.time;
        if (health <=0)
        {
            health = 0;
            Death();
        }
        UpdateHUD();
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
    }

    protected virtual void AnimateAttack()
    {
        if (attackObject.isAttacking) return;
        animator.SetBool("Attack", true);
        animator.SetBool("Grounded", true);
    }

    protected virtual void AnimateStay()
    {
        animator.SetInteger("AnimState", 0);
        if (isBlocking)
        {
            animator.SetInteger("AnimState", 1);
            animator.SetBool("Grounded", true);
        }
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
