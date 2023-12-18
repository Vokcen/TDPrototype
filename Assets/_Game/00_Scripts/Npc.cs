using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Npc : MonoBehaviour, IDamageable
{
    [SerializeField]protected float maxHealth = 100;
   
    [SerializeField] protected float attackDamage = 10f; // Düþmanýn verdiði hasar
    [SerializeField] protected float attackRate = 1f;
    [SerializeField] protected float attackRange = 2; //  düþmanýn saldýrý menzili

    protected float attackTimer;
    protected float health = 100f; // Düþmanýn saðlýðý

  protected NavMeshAgent navmeshAgent;

    public Transform Transform { get =>transform; set => throw new System.NotImplementedException(); }

    protected void Awake()
    {
        navmeshAgent= GetComponent<NavMeshAgent>();
    }
    protected void Start()
    {
        health = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }
    public abstract void Attack(IDamageable damageable);
    public abstract void Die();

    protected bool CanAttack()
    {
        attackTimer += Time.deltaTime;


        if (attackTimer >= attackRate)
        {
            return true;
        }
        return false;
    }

    public void MoveEnemy()
    {

       // transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
    }
}
