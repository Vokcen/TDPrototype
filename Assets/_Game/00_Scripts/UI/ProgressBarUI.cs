using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    public static ProgressBarUI Instance;
    [SerializeField] private TMP_Text currentWaveText;

    [SerializeField] private Image fillImage;


    private void Awake()
    {
        Instance = this;
    }
    public void ProgressBarUpdate(float currentWave,float totalWave)
    {
        currentWaveText.text = currentWave.ToString("0")+"/"+totalWave.ToString("0");
     
        FillImagePercentage(currentWave, totalWave);
    }

    private void FillImagePercentage(float current,float total)
    {
        float fillPercentage = current / total;
        fillImage.DOFillAmount(fillPercentage, .3f);

    }
}
