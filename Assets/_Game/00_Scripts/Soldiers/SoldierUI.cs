using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoldierUI : MonoBehaviour
{

  [SerializeField]  private float[] soldierPurchaseCost;
    private int soldierPurchaseLevel=1;
    private SoldierManager soldierManager;

    [SerializeField] TMP_Text soldierPriceText;
    private void Start()
    {
        soldierManager=SoldierManager.Instance;
        UpdateTexts();

    }
   


    public void BuySoldier()
    {
        if(MoneyManager.Instance.CanAfford(GetSoldierCostPrice()))
        {
 
            soldierPurchaseLevel++;
            soldierManager.SpawnSoldier();
            UpdateTexts();
            MoneyManager.Instance.SpendMoney(GetSoldierCostPrice()); // Para harcama iþlemi
        }
    }


    private void UpdateTexts()
    {
        soldierPriceText.text = GetSoldierCostPrice().ToString("0");
    }
  

    private float GetSoldierCostPrice()
    {
        return soldierPurchaseCost[soldierPurchaseLevel-1];
    }

}
