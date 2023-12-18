

using UnityEngine;


public class MeleeEnemy : Enemy
{
    public override void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(attackDamage);
    }
}