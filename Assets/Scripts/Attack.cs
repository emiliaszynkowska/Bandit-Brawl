using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    static Vector3 leftScale = new Vector3(-1, 1, 1);
    static Vector3 rightScale = new Vector3(1, 1, 1);

    public float attackDamage;
    public bool isAttacking = false;

    public abstract void StartAttack();
    public abstract bool CanAttack();

    public void LookLeft()
    {
        transform.localScale = leftScale;
    }

    public void LookRight()
    {
        transform.localScale = rightScale;
    }
}
