using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour,IDamageable
{
    public static TowerBehaviour instance;

   [SerializeField] private float towerMaxHealth;
    private float towerCurrentHealth=100;

    public Transform Transform { get => transform; set => throw new System.NotImplementedException(); }

    private void Awake()
    {
        instance = this;
    }
  
    public void TakeDamage(float damage)
    {
        towerCurrentHealth -= damage;
        IsTowerDestroy();
    }

    private void IsTowerDestroy()
    {
        if (towerCurrentHealth < 0)
        {
        }
    }
 
    public void SetTowerHealth(float amount)
    {
        towerCurrentHealth = amount;
    }
    public float GetTowerHealth() => towerCurrentHealth;
}
