using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : Projectile
{
    private Transform targetTransform;

    [SerializeField] float speed = 3;

    [SerializeField] private GameObject explosionFx;
    public override void InitializePool(ObjectPool pool)
    {
        this.objectPool = pool;
    }

    public override void Seek(IDamageable target, Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }

    public override void SetDamage(float damage)
    {
        this.damage = damage;

    }
    void Update()
    {
        if (targetTransform == null)
        {
            objectPool.ReturnObject(gameObject);
            return;
        }

        // Hedefe doðru hareket
        Vector3 dir = targetTransform.transform.position + Vector3.up - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        transform.LookAt(targetTransform.transform.position);
        if (dir.magnitude <= distanceThisFrame)
        {
            // Hedefe ulaþtýðýnda zarar ver ve yok ol




            Explosion();
            objectPool.ReturnObject(gameObject);
            gameObject.SetActive(false);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    public void Explosion()
    {
        ApplyExplosionDamageNearbyEnemy();
        ExplosionFx();
    }

    private void ApplyExplosionDamageNearbyEnemy()
    {
        var col = Physics.OverlapSphere(transform.position, 2);
        foreach (var item in col)
        {
            if (item.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    private void ExplosionFx()
    {
        explosionFx.transform.parent = null;
        explosionFx.SetActive(true);
    }
}
