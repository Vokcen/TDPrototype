using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyWaveController s�n�f�, d��man dalgalar�n� y�netir
public class EnemyWaveController : MonoBehaviour
{
    public static EnemyWaveController Instance;

    [SerializeField] private EnemyWave[] waves; // Oyundaki d��man dalgalar�
    [SerializeField] private Transform[] enemySpawnPoints;
        private int currentWaveIndex = 0; // �u anki dalga index'i

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
                // D��man tipine g�re Object Pool olu�tur
                if (!enemyPools.ContainsKey(enemyPrefab))
                {
                    enemyPools[enemyPrefab] = new ObjectPool(enemyPrefab.gameObject, 20); // 20 adet �n bellekte tut
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
            //D��man data ayar�
            enemy.InitializeEnemyPool(enemyPool);
            // D��man�n konumunu ve rotasyonunu ayarla
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

// EnemyWave s�n�f�, bir d��man dalga �zelliklerini ve y�netimini i�erir
[System.Serializable]
public class EnemyWave
{
    public Enemy[] enemies; // Dalga i�indeki d��manlar
    public float delayBetweenEnemies = 1f; // D��manlar aras�ndaki gecikme s�resi
}