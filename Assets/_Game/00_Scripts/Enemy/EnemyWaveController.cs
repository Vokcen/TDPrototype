using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyWaveController sýnýfý, düþman dalgalarýný yönetir
public class EnemyWaveController : MonoBehaviour
{
    public static EnemyWaveController Instance;

    [SerializeField] private EnemyWave[] waves; // Oyundaki düþman dalgalarý
    [SerializeField] private Transform[] enemySpawnPoints;
        private int currentWaveIndex = 0; // Þu anki dalga index'i

    [SerializeField] private float spawnTimer;
    private float timer;
    private Dictionary<Enemy, ObjectPool> enemyPools = new Dictionary<Enemy, ObjectPool>();


    ProgressBarUI progressBar;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        progressBar = ProgressBarUI.Instance;
        progressBar.ProgressBarUpdate(currentWaveIndex, waves.Length);
        InitializeEnemyPools();
    }
    private void InitializeEnemyPools()
    {
        foreach (EnemyWave wave in waves)
        {
            foreach (Enemy enemyPrefab in wave.enemies)
            {
                // Düþman tipine göre Object Pool oluþtur
                if (!enemyPools.ContainsKey(enemyPrefab))
                {
                    enemyPools[enemyPrefab] = new ObjectPool(enemyPrefab.gameObject, 20); // 20 adet ön bellekte tut
                }
            }
            }

    }
    public void ReturnPoolByObject(Enemy enemy)
    {
         
        if (enemyPools.ContainsKey(enemy))
        {
            
            enemyPools[enemy].ReturnObject(enemy.gameObject);
        }
       
    }
    private void Update()
    {
        if (CanSpawnWave())
        {
            timer = spawnTimer;
            StartCoroutine(SpawnWave());
        }
    }

    private bool CanSpawnWave()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            return true;
        }
        return false;
    }

  

    private IEnumerator SpawnWave()
    {
        EnemyWave currentWave = waves[currentWaveIndex];

        WaitForSeconds waitForSeconds = new WaitForSeconds(currentWave.delayBetweenEnemies);
   
        foreach (Enemy enemyPrefab in currentWave.enemies)
        {
            ObjectPool enemyPool = enemyPools[enemyPrefab];
            Enemy enemy = enemyPool.GetObject().GetComponent<Enemy>();
            //Düþman data ayarý
            enemy.InitializeEnemyPool(enemyPool);
            // Düþmanýn konumunu ve rotasyonunu ayarla
            int randomSpawnIndex = Random.Range(0, enemySpawnPoints.Length);
            enemy.transform.position = enemySpawnPoints[randomSpawnIndex].position;
            enemy.transform.rotation = enemyPrefab.transform.rotation;
            enemy.ImproveStats(1.04f * (currentWaveIndex+1)/10, 1.06f * (currentWaveIndex + 1) / 10);
            enemy.gameObject.SetActive(true);
            yield return waitForSeconds;
        }
 
        currentWaveIndex++;
        progressBar.ProgressBarUpdate(currentWaveIndex, waves.Length);
    }
    
    public void SetCurrentWaveIndex(int index)
    {
        currentWaveIndex = index;
    }
    public int GetCurrentWaveIndex() => currentWaveIndex;
}

// EnemyWave sýnýfý, bir düþman dalga özelliklerini ve yönetimini içerir
[System.Serializable]
public class EnemyWave
{
    public Enemy[] enemies; // Dalga içindeki düþmanlar
    public float delayBetweenEnemies = 1f; // Düþmanlar arasýndaki gecikme süresi
}