using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretBehaviour : MonoBehaviour, IUpgradeable
{



    [SerializeField] private List<Transform> firePoint; // Ate� noktas�
    [SerializeField] private float attackRange = 10f; // Ate� menzili
    [SerializeField] private GameObject projectilePrefab; // Ate� edilecek mermi prefab�
    [SerializeField] private GameObject bombProjectilePrefab;

    [SerializeField] private float fireRate = 1f; // Ate� h�z�
    [SerializeField] private float damage = 10f; // Turret hasar�
    [SerializeField] private int maxUpgradeLevel = 3; // Turret'�n maksimum seviyesi
    [SerializeField] private int currentUpgradeLevel = 1; // Turret'�n mevcut seviyesi
    [SerializeField] private float[] upgradeCosts; // Turret y�kseltme maliyetleri
    [SerializeField] List<TurretVisual> turretVisualList;

    private Transform activeRotator;
    private List<Transform> activeRecoilObjectList;
    private float nextFireTime; // Bir sonraki ate� zaman�

    private ObjectPool projectilePool; // Mermi havuzu
    private ObjectPool bombProjectilePool;

    private ObjectPool activePool;
    private Enemy activeTarget;

    //UI Stuff
    private TurretUpgradeUI turretUpgradeUI;

    public TurretData turretData;

    private void Awake()
    {
        turretUpgradeUI = GetComponent<TurretUpgradeUI>();
    }
    private void Start()
    {
        InitializeProjectilePool();

        UpdateTurretData();
        turretUpgradeUI.Initialize(currentUpgradeLevel, GetUpgradeCost());
        UpgradeTowerVisualMesh();
        CheckProjectileLevel();
    }

    public void UpdateTurretData()
    {
        turretData.turretdamage = damage;
        turretData.currentLevel = currentUpgradeLevel;
        turretData.turretFireRate = fireRate;
    }
    public void LoadTurretData()
    {
        currentUpgradeLevel = turretData.currentLevel;
        damage = turretData.turretdamage;
        fireRate = turretData.turretFireRate;
    }
    private void InitializeProjectilePool()
    {
        projectilePool = new ObjectPool(projectilePrefab, 10); // 10 adet mermiyle ba�lat�yoruz, iste�e ba�l� olarak art�r�labilir
        bombProjectilePool = new ObjectPool(bombProjectilePrefab, 10);
    }
    private void Update()
    {
        if (CanFire())
        {
            RecoilGun();
            AimToTarget();
            Fire();
        }
    }

    private bool CanFire()
    {


        return Time.time >= nextFireTime && HasTargetInRange();
    }
    private bool HasTargetInRange()
    {
        if (activeTarget != null)
        {
            if (activeTarget.gameObject.activeInHierarchy)
            {

                return true;

            }
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy)) // D��man�n tag'ini uygun �ekilde ayarlay�n
            {
                activeTarget = enemy;
                return true;
            }
        }
        return false;
    }
    private void RecoilGun()
    {
        activeRecoilObjectList.ForEach(x => x.transform.DOLocalMoveX(-.7f, .1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            activeRecoilObjectList.ForEach(x => x.transform.DOLocalMoveX(0, .1f));
        }));
    }
    private void AimToTarget()
    {
        Vector3 lookPosition = activeTarget.transform.position;
        lookPosition.y = activeRotator.transform.localPosition.y;
        activeRotator.DOLookAt(lookPosition, .2f);
    }
    private void Fire()
    {
        for (int i = 0; i < firePoint.Count; i++)
        {
            Projectile projectile = activePool.GetObject().GetComponent<Projectile>(); // Object Pool'dan mermi al�n�r
            projectile.transform.position = firePoint[i].position;
            projectile.transform.rotation = firePoint[i].rotation;
            projectile.Seek(activeTarget, activeTarget.transform);
            projectile.SetDamage(damage);
            projectile.InitializePool(activePool);

            // Mermi aktif edilir
            projectile.gameObject.SetActive(true);
        }
        // Mermi data entegrasyonu



        // Ate� etme i�lemi kontrol
        // Debug.Log("Turret ate� etti!");

        // Ate� etme s�re s�f�rlanmas�
        nextFireTime = Time.time + 1f / fireRate;
    }

    public bool CanUpgrade()
    {
        return currentUpgradeLevel < maxUpgradeLevel;
    }

    public float GetUpgradeCost()
    {
        return upgradeCosts[currentUpgradeLevel - 1];
    }

    public void Upgrade()
    {
        if (CanUpgrade())
        {
            float upgradeCost = GetUpgradeCost();

            // Para kontrol� yap�n
            if (MoneyManager.Instance.CanAfford(upgradeCost))
            {
                // Turret'� y�kselt
                currentUpgradeLevel++;
                damage += 5f; // �rnek: Her seviye art���nda hasar� artt�r
                MoneyManager.Instance.SpendMoney(upgradeCost); // Para harcama i�lemi
                turretUpgradeUI.Initialize(currentUpgradeLevel, GetUpgradeCost());
                Debug.Log("Turret y�kseltildi! Yeni seviye: " + currentUpgradeLevel);
                UpdateTurretData();
                UpgradeTowerVisualMesh();
                CheckProjectileLevel();
            }
            else
            {
                Debug.Log("Yeterli para yok!");
            }
        }

    }
    public void CheckProjectileLevel()
    {
        if (currentUpgradeLevel > 2)
        {
            activePool = bombProjectilePool;
        }
        else 
        {
            activePool = projectilePool;
        }
    }
    public void UpgradeTowerVisualMesh()
    {
        turretVisualList.ForEach(x => x.gameObject.SetActive(false));
        TurretVisual activeTurretVisual = turretVisualList[currentUpgradeLevel - 1];
        activeTurretVisual.gameObject.SetActive(true);
        firePoint = activeTurretVisual.firePoint;
        turretUpgradeUI.UpdateOutline(activeTurretVisual.outLine);
        activeRotator = activeTurretVisual.gunRotator;
        activeRecoilObjectList = activeTurretVisual.recoilObjectList;


    }
    public void OnMouseDown()
    {
        if (turretUpgradeUI.GetTurretUpgradeUIActiveStatus())
        {
            ShowTurretUpgradeUI();


        }
        else
        {
            turretUpgradeUI.Hide();

        }
    }


    public void ShowTurretUpgradeUI()
    {
        turretUpgradeUI.Initialize(currentUpgradeLevel, GetUpgradeCost());
        turretUpgradeUI.Show();
    }
}
