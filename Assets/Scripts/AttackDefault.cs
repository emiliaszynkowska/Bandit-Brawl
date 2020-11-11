using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDefault : Attack
{

    public float attackTime;
    public float attackCooldown;
    public float attackKnockback;


    public ContactFilter2D contactFilter;
    
    Collider2D attackCollider;
    Character parent;

    void Start()
    {
        attackCollider = GetComponent<Collider2D>();
        parent = GetComponentInParent<Character>();
    }

    public override void StartAttack()
    {
        StartCoroutine(DoAttack());
    }

    IEnumerator DoAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackTime);
        Collider2D[] results = new Collider2D[8];
        attackCollider.OverlapCollider(contactFilter, results);
        foreach (Collider2D col in results)
        {
            if (col != null)
            {
                Character c = col.gameObject.GetComponent<Character>();
                if (c != null && !c.Equals(parent))
                {
                    c.TakeDamage(attackDamage);
                    c.TakeKnockback(transform.position, attackKnockback);
                }
            }
        }
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        yield return null;

    }
}
