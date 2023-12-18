using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<TurretBehaviour> turrets;
    public SaveLoadManager saveLoadManager;
    public GameData currentGameData;



    void Start()
    {
        LoadGameData();
    }
    public void OnApplicationQuit()
    {
        SaveGameData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            print("s" +
                "");
            SaveGameData();
        }
    }


    public void SaveGameData()
    {


        currentGameData = SetGameData();

        // Turret bilgilerini ekleyin
        foreach (TurretBehaviour turret in turrets)
        {
            currentGameData.turretDataList.Add(turret.turretData);
        }

        saveLoadManager.SaveGame(currentGameData);
    }

    private static GameData SetGameData()
    {
        return new GameData
        {
            playerMoney = MoneyManager.Instance.GetMoney(),
            towerHealth = TowerBehaviour.instance.GetTowerHealth(),
            waveIndex = EnemyWaveController.Instance.GetCurrentWaveIndex(),
            turretDataList = new List<TurretData>()
        };
    }

    public void LoadGameData()
    {
        GameData loadedData = saveLoadManager.LoadGame();
        if (loadedData != null)
        {
            GetGameData(loadedData);
        }
        else
        {
            currentGameData = new GameData();
            // Yeni bir oyun baþlatýlýyor, baþlangýç verileri ayarlý
        }
    }

    private void GetGameData(GameData loadedData)
    {
        MoneyManager.Instance.SetMoney(loadedData.playerMoney);
        TowerBehaviour.instance.SetTowerHealth(loadedData.towerHealth);
        EnemyWaveController.Instance.SetCurrentWaveIndex(loadedData.waveIndex);

        // Turret bilgilerini yükleyin
        for (int i = 0; i < turrets.Count; i++)
        {
            if (i < loadedData.turretDataList.Count)
            {
                turrets[i].turretData = loadedData.turretDataList[i];
                turrets[i].LoadTurretData();
            }
        }
    }
}
