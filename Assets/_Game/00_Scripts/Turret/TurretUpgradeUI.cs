using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurretUpgradeUI : MonoBehaviour
{

    [SerializeField]private TMP_Text levelText;
    [SerializeField]private TMP_Text upgradeCostText;
    [SerializeField] GameObject uiObject;
    [SerializeField] private Outline visualOutline;
    bool isAnimating;

    public void Initialize(int levelAmount,float upgradeCost)
    { 
        if(levelAmount<3)
        {
            levelText.text = levelAmount.ToString("0");
            upgradeCostText.text = upgradeCost.ToString("0");
        }else
        {
            levelText.text = levelAmount.ToString("Max");
            upgradeCostText.text = upgradeCost.ToString("Max");
        }
   
    }
  

    public void Show()
    {
        if (isAnimating) return;
        isAnimating = true;
        uiObject.SetActive(true);
        visualOutline.enabled = true;
        uiObject.transform.DOLocalMoveZ(2, .3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            isAnimating = false;
        });
        uiObject.transform.DOScale(Vector3.one, .3f);

       
    }

    public void Hide()
    {
        if (isAnimating) return;
        isAnimating = true;
        visualOutline.enabled = false;
        uiObject.transform.DOLocalMoveZ(0, .3f).OnComplete(() =>
        {
            isAnimating = false;
            uiObject.SetActive(false);
        });
        uiObject.transform.DOScale(Vector3.zero, .3f);


    }

    public bool GetTurretUpgradeUIActiveStatus()
    {
        if (uiObject.activeInHierarchy) return false;
        else return true;
      
    }

    public void UpdateOutline(Outline outLine)
    {
        visualOutline = outLine;
    }

}
