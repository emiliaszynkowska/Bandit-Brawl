using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    //constants
    protected const float groundDistance = 0.5f;
    protected const int doubleJumpsCount = 2;

    //public variables
    public float health;
    public float moveSpeed;
    public float jumpSpeed;
    public float jumpCooldown;
    public Animator animator;

    //protected variables
    protected float lastJumpTime;
    protected SpriteRenderer sprite;
    protected bool isGrounded = true;
    [SerializeField] protected LayerMask collisionMask;
    protected int jumpsRemaining;

   /* private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Foreground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == ("Foreground"))
        {
            isGrounded = false;
        }
    }*/
}
