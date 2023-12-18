using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    public static SoldierManager Instance;
    [SerializeField]private GameObject soldierPrefab;
    [SerializeField] private Transform soldierSpawnPoint;
    private ObjectPool soldierPool;
    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        InitializeSoldierPool();
    }

    private void InitializeSoldierPool()
    {
        soldierPool = new ObjectPool(soldierPrefab, 10); 

    }
    public void SpawnSoldier()
    {
        Soldier newSoldier = soldierPool.GetObject().GetComponent<Soldier>();
        newSoldier.transform.position = soldierSpawnPoint.position;
        newSoldier.InitializeSoldierPool(soldierPool);
        newSoldier.gameObject.SetActive(true);
        // Daha fazla asker özellikleri ayarlanabilir
    }

    public void ReturnPoolByObject(Soldier soldier)
    {

        soldierPool.ReturnObject(soldier.gameObject);

    }
}
