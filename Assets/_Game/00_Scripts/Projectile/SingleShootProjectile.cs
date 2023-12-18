using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShootProjectile : Projectile
{
    private IDamageable targetDamageable;
    private Transform targetTransform;

    [SerializeField] float speed = 3;

    public override void InitializePool(ObjectPool pool)
    {
        this.objectPool = pool;
    }

    public override void Seek(IDamageable target,Transform TargetTransform)
    {

        targetDamageable = target;
        this.targetTransform = TargetTransform;
    }
    
    public override void SetDamage(float damage)
    {
        this.damage = damage;
    }

    void Update()
    {
        if (targetDamageable == null)
        {
            objectPool.ReturnObject(gameObject);
            return;
        }

        // Hedefe doðru hareket
        Vector3 dir = targetTransform.transform.position+Vector3.up - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        transform.LookAt(targetTransform.transform.position);
        if (dir.magnitude <= distanceThisFrame)
        {
            // Hedefe ulaþtýðýnda zarar ver ve yok ol
          

            targetDamageable.TakeDamage(damage);

            objectPool.ReturnObject(gameObject);
            gameObject.SetActive(false);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }
}
