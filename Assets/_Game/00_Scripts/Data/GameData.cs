using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public float playerMoney;
    public float towerHealth;
    public int waveIndex;
    public List<TurretData> turretDataList;
}
