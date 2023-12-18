using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected ObjectPool objectPool;
    protected float damage;
    public abstract void InitializePool(ObjectPool pool);
    public abstract void Seek(IDamageable target, Transform TargetTransform);

    public abstract void SetDamage(float damage);
}
