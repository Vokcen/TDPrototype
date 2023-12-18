using UnityEngine;

public interface IDamageable
{
    public Transform Transform { get; set; }
    public void TakeDamage(float damage);
}