using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTutorial : MonoBehaviour
{
    //constants
    protected const float groundDistance = 0.6f;
    protected const int doubleJumpsCount = 1;
    
    //public variables
    public float moveSpeed;
    public float maxCharacterSpeed = 5;
    public float jumpSpeed;
    public float jumpCooldown;
    public Attack attackObject;
    public Image fadeImage;
    public Canvas HUD;
    
    //protected variables
    protected float lastJumpTime;
    protected SpriteRenderer sprite;
    protected bool isGrounded = true;
    protected bool canMove = true;
    [SerializeField] protected LayerMask collisionMask;
    protected int doubleJumpsRemaining;
    protected Animator animator;
    protected Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FadeIn");
        LoadCharacter();
    }


    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        PlayerJump();
        PlayerAttack();
    }

    // FixedUpdate is called for physics
    private void FixedUpdate()
    {
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
            animator.SetInteger("AnimState", 0);
            body.velocity = new Vector2(body.velocity.x * 0.9f, body.velocity.y);
        }
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
    
    protected void LoadCharacter()//call in the Start function
    {
        doubleJumpsRemaining = doubleJumpsCount;
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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

    public void TakeKnockback(Vector3 source, float strength)
    {
        Vector3 direction = transform.position - source;
        body.AddForce((direction.normalized + Vector3.up/5) * strength, ForceMode2D.Impulse);
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

    protected virtual void AnimateHurt()
    {
        if (attackObject.isAttacking) return;
        animator.SetBool("Hurt", true);
        animator.SetBool("Grounded", true);
    }
    
    IEnumerator FadeIn()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        fadeImage.CrossFadeAlpha(0.0f, 1, false);
        yield return new WaitForSeconds(2);
    }
}