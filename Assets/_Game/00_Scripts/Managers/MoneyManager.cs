using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private static MoneyManager _instance;
    public static MoneyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MoneyManager>();
            }
            return _instance;
        }
    }

    [SerializeField] private float playerMoney = 100; // Oyuncunun baþlangýç parasý
    [SerializeField] private TMP_Text moneyText;


    private void Start()
    {
        UpdateMoneyText();
    }
    public bool CanAfford(float amount)
    {
        return playerMoney >= amount;
    }

    public void SpendMoney(float amount)
    {
        playerMoney -= amount;
        UpdateMoneyText();
    }
    public void UpdateMoneyText()
    {
        moneyText.text=playerMoney.ToString("0");
    }

    public void SetMoney(float amount)
    {
        playerMoney = amount;
    }
    public void GainMoney(float amount)
    {
        playerMoney += amount;
        UpdateMoneyText();
    }
    public float GetMoney() => playerMoney;
}
