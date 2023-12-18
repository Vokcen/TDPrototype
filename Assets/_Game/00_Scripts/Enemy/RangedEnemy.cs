

// RangedEnemy s�n�f�, Enemy s�n�f�ndan t�retilir ve Ranged d��manlar�n�n �zelliklerini i�erir

using System;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileStartPosition;

    ObjectPool projectilePool;

  new  private void Start()
    {
        base.Start();
        InitializeProjectilePool();
    }

    private void InitializeProjectilePool()
    {
        projectilePool = new ObjectPool(projectilePrefab, 10);
    }

    // Ekstra �zellikler veya davran��lar buraya eklenir
    public override void Attack(IDamageable damageable)
    {
        Projectile projectile = projectilePool.GetObject().GetComponent<Projectile>(); // Object Pool'dan mermi al�n�r
        projectile.transform.position = projectileStartPosition.position;
        projectile.transform.rotation = projectileStartPosition.rotation;
        projectile.Seek(damageable,damageable.Transform);
        projectile.SetDamage(attackDamage);
        projectile.InitializePool(projectilePool);

        // Mermi aktif edilir
        projectile.gameObject.SetActive(true);

    }
}