using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDefaultTutorial : Attack
{
    public float attackTime;
    public float attackCooldown;

    public ContactFilter2D contactFilter;
    
    Collider2D attackCollider;
    PlayerTutorial parent;
    private int dummiesHit = 0;

    void Start()
    {
        attackCollider = GetComponent<Collider2D>();
        parent = GetComponentInParent<PlayerTutorial>();
    }

    public override void StartAttack()
    {
        StartCoroutine(DoAttack());
    }

    public override bool CanAttack()
    {
        Collider2D[] results = new Collider2D[8];
        return attackCollider.OverlapCollider(contactFilter, results) > 0;
    }

    IEnumerator DoAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackTime);
        Collider2D[] results = new Collider2D[8];
        attackCollider.OverlapCollider(contactFilter, results);
        foreach (Collider2D col in results)
        {
            if (col != null && (col.gameObject.name.Equals("Dummy A") || col.gameObject.name.Equals("Dummy B")))
            {
                dummiesHit++;
                switch (dummiesHit)
                {
                    case(1):
                        parent.tutorialController.DummyDamage("A");
                        parent.tutorialController.SetComplete(1);
                        break;
                    case(2):
                        parent.tutorialController.DummyDamage("A");
                        parent.tutorialController.SetComplete(2);
                        break;
                    case(3):
                        parent.tutorialController.DummyDamage("A");
                        parent.tutorialController.SetComplete(3);
                        parent.tutorialController.LearnSlam();
                        break;
                    case(4):
                        parent.tutorialController.DummyDamage("B");
                        parent.tutorialController.SetComplete(1);
                        break;
                    case(5):
                        parent.tutorialController.DummyDamage("B");
                        parent.tutorialController.SetComplete(2);
                        break;
                    case(6):
                        parent.tutorialController.DummyDamage("B");
                        parent.tutorialController.SetComplete(3);
                        parent.tutorialController.LearnBlock();
                        break;
                }
                break;
            } 
        }
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        yield return null;

    }
}