using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : Npc
{
    protected ObjectPool soldierPool;

    private bool isSearching = true;
    private bool isChasing;
    public float searchRadius = 5f;
    private Enemy targetEnemy;

    public void InitializeSoldierPool(ObjectPool pool)
    {
        soldierPool = pool;
    }
    private void Start()
    {

    }
    public void Update()
    {


        if (isSearching)
        {
            Search();
        }
        else if (isChasing)
        {
            if (!IsEnemyNearbyAttackRange())
            {
                ChaseEnemy();

            }

        }
        else
        {
            if(CanAttack())
            {

                Attack(targetEnemy);
                attackTimer = 0;
            }
        }


    }
    private void Search()
    {
        // Belirli bir bölgede devriye gez
        var col = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (var item in col)
        {
            if (item.TryGetComponent<Enemy>(out Enemy enemy))
            {
                targetEnemy = enemy;
                isChasing = true;
                isSearching = false;

            }
        }
    }
    private void ChaseEnemy()
    {
        navmeshAgent.SetDestination(targetEnemy.transform.position);


    }
    private bool IsEnemyNearbyAttackRange()
    {
        float distance = Vector3.Distance(transform.position, targetEnemy.transform.position);
        if (distance < attackRange)
        {
            navmeshAgent.SetDestination(transform.position);
            isChasing = false;
            InitEnemyAttackState();
            return true;
        }
        else
        {
            return false;
        }


    }

    public override void Attack(IDamageable damageable)
    {

        
            if(CheckEnemyIsDeath())
            {
                targetEnemy = null;
                isSearching = true;
                return;
            }
            targetEnemy.TakeDamage(attackDamage);
     
    }
    private void InitEnemyAttackState()
    {
        targetEnemy.InitSoldierAttack(this);
        transform.LookAt(targetEnemy.transform);


    }
    private bool CheckEnemyIsDeath()
    {
        if (!targetEnemy.gameObject.activeInHierarchy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override void Die()
    {

        // Düþmanýn ölüm iþlemleri burada gerçekleþtirilir
        soldierPool.ReturnObject(gameObject);


    }
}
