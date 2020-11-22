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
    public float blockCooldown = 3;
    public Attack attackObject;
    public Image fadeImage;
    public GameObject slamParticles;
    public TutorialController tutorialController;
    public int dummiesHit;
    public int dummiesBlocked;

    //protected variables
    protected float lastJumpTime;
    protected float lastBlockTime = -10;
    protected float lastBlockDummyTime = -10;
    protected SpriteRenderer sprite;
    protected bool isGrounded = true;
    protected bool isBlocking = false;
    protected bool canMove = true;
    [SerializeField] protected LayerMask collisionMask;
    protected int doubleJumpsRemaining;
    protected Animator animator;
    protected Rigidbody2D body;
    protected Vector2 slamSize = new Vector2(4, 2);
    protected float slamKnockback = 15;
    protected float slamDamage = 5f;
    [SerializeField] protected SoundManager sound;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FadeIn");
        LoadCharacter();
    }

    protected void LoadCharacter() //call in the Start function
    {
        doubleJumpsRemaining = doubleJumpsCount;
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        sound = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        PlayerJump();
        PlayerAttack();
        PlayerBlock();
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
    
    void PlayerBlock()
    {
        if (Input.GetButton("Fire2"))
            StartBlock();
        else
            EndBlock();
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
            if (col != null && col.gameObject.name.Equals("Dummy B"))
            {
                dummiesHit++;
                switch (dummiesHit)
                {
                    case(4):
                        tutorialController.DummyDamage("B");
                        tutorialController.SetComplete(1);
                        break;
                    case(5):
                        tutorialController.DummyDamage("B");
                        tutorialController.SetComplete(2);
                        break;
                    case(6):
                        tutorialController.DummyDamage("B");
                        tutorialController.SetComplete(3);
                        tutorialController.LearnBlock();
                        break;
                }
                break;
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
            if (Time.time > lastBlockDummyTime + blockCooldown)
            {
                lastBlockDummyTime = Time.time;
                
            }
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
            sound.PlayDamage();
            AnimateHurt();
        }
        else
        {
            sound.PlayBlockedDamage();
            dummiesBlocked++;
            switch (dummiesBlocked)
            {
                case (1):
                    tutorialController.SetComplete(1);
                    break;
                case (2):
                    tutorialController.SetComplete(2);
                    break;
                case (3):
                    tutorialController.SetComplete(3);
                    tutorialController.EndTutorial();
                    break;
            }
        }
        lastBlockTime = Time.time;
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

    IEnumerator FadeIn()
    {
        fadeImage.color = Color.black;
        fadeImage.canvasRenderer.SetAlpha(1.0f);
        fadeImage.CrossFadeAlpha(0.0f, 1, false);
        yield return new WaitForSeconds(2);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Ring 1"))
        {
            other.gameObject.SetActive(false);
            tutorialController.SetComplete(1);
        }
        else if (other.gameObject.name.Equals("Ring 2"))
        {
            other.gameObject.SetActive(false);
            tutorialController.SetComplete(2);
            tutorialController.LearnJump();
        }
        else if (other.gameObject.name.Equals("Ring 3"))
        {
            other.gameObject.SetActive(false);
            tutorialController.SetComplete(3);
            tutorialController.LearnAttack();
        }
    }
    
}