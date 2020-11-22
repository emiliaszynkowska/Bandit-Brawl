using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAttack : MonoBehaviour
{
    public GameObject slamParticles;
    protected Vector2 slamSize = new Vector2(4, 2);
    protected float slamKnockback = 10;
    protected float slamDamage = 5f;
    protected float jumpSpeed = 14;
    protected float jumpCooldown = 3;
    protected float lastJumpTime = -10;
    protected bool wait = true;
    protected Rigidbody2D body;
    [SerializeField] protected SoundManager sound;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        StartCoroutine("Jump");
    }
    
    IEnumerator Jump()
    {
        if (wait)
        {
            yield return new WaitForSeconds(2);
            wait = false;
        }
        if (Time.time > lastJumpTime + jumpCooldown)
        {
            lastJumpTime = Time.time;
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            sound.PlayJump();
            yield return new WaitForSeconds(1.25f);
            Slam();
        }
    }

    void Slam()
    { 
        body.velocity = new Vector2(0, -jumpSpeed*1.5f);
        Instantiate(slamParticles,transform);
        Collider2D[] results = new Collider2D[8];
        results = Physics2D.OverlapBoxAll(transform.position,slamSize,0);
        foreach (Collider2D col in results)
        {
            if (col != null)
            {
                PlayerTutorial c = col.gameObject.GetComponent<PlayerTutorial>();
                if (c != null)
                {
                    c.TakeKnockback(transform.position, slamKnockback);
                    c.TakeDamage(0);
                }
            }
        }
        sound.PlaySlam();
    }
}
