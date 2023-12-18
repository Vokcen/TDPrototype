using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : Enemy
{
    [SerializeField] GameObject explosionFx;

    public override void Attack(IDamageable damageable)
    {
    
        ExplosionFx();
        damageable.TakeDamage(attackDamage);
        EnemyWaveController.Instance.ReturnPoolByObject(this);
        gameObject.SetActive(false);
    }

    private void ExplosionFx()

    {
        explosionFx.transform.parent = null;
        explosionFx.SetActive(true);
    }


}
