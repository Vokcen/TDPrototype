

// RangedEnemy sýnýfý, Enemy sýnýfýndan türetilir ve Ranged düþmanlarýnýn özelliklerini içerir

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

    // Ekstra özellikler veya davranýþlar buraya eklenir
    public override void Attack(IDamageable damageable)
    {
        Projectile projectile = projectilePool.GetObject().GetComponent<Projectile>(); // Object Pool'dan mermi alýnýr
        projectile.transform.position = projectileStartPosition.position;
        projectile.transform.rotation = projectileStartPosition.rotation;
        projectile.Seek(damageable,damageable.Transform);
        projectile.SetDamage(attackDamage);
        projectile.InitializePool(projectilePool);

        // Mermi aktif edilir
        projectile.gameObject.SetActive(true);

    }
}