using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Enemy : Npc
{


    protected ObjectPool enemyPool;
    protected TowerBehaviour towerBehaviour;
    private IDamageable towerDamageable;

    private Soldier targetSoldier;

   

  new  private void Awake()
    {
        base.Awake();
        towerBehaviour = TowerBehaviour.instance;

    }
    new protected void Start()
    {
        base.Start();
        towerDamageable = towerBehaviour.GetComponent<IDamageable>();

    }
    public void InitializeEnemyPool(ObjectPool pool)
    {
        enemyPool = pool;
    }


    public override void Die()
    {
        // Düþmanýn ölüm iþlemleri burada gerçekleþtirilir
   
        MoneyManager.Instance.GainMoney(maxHealth/2);
        enemyPool.ReturnObject(gameObject);
    }

    public void Update()
    {

        if (targetSoldier != null)
        {
            if (CanAttack())
            {
                if(CheckSoldierIsDeath())
                {
                    targetSoldier = null;
                    return;
                }

                Attack(targetSoldier.GetComponent<IDamageable>());
                attackTimer = 0f; // Timer'ý sýfýrla
            }
            return;
        } 

        if (TargetIsNearby())
        {
            if (CanAttack())
            {
                Attack(towerDamageable);
                attackTimer = 0f; // Timer'ý sýfýrla
            }
        }

    }

    private bool CheckSoldierIsDeath()
    {
        if (!targetSoldier.gameObject.activeInHierarchy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool TargetIsNearby()
    {
        //float distance = Vector3.Distance(towerBehaviour.transform.position, transform.position);
        //if (distance <= attackRange)
        //{
        //    moveSpeed = 0;
        //    return true;
        //}
      
            float distance = Vector3.Distance(transform.position, towerBehaviour.transform.position);
            if (distance < attackRange)
            {
                navmeshAgent.SetDestination(transform.position);
             
                return true;
            }
            else
            {
            navmeshAgent.SetDestination(towerBehaviour.transform.position);
            return false;
            }


     

    }

    public void InitSoldierAttack(Soldier soldier)
    {
        targetSoldier= soldier;
        navmeshAgent.SetDestination(transform.position);
        transform.LookAt(targetSoldier.transform);
    }

    public void ImproveStats(float maxHealth,float damage)
    {
        this.maxHealth *= maxHealth;
        this.attackDamage *= damage;
    }

}
