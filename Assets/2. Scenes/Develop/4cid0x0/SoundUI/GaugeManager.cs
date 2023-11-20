using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GaugeManager : MonoBehaviour
{
    [SerializeField] private Sprite on;
    [SerializeField] private Sprite off;

    public SoundManager.BusType volumeType;
    private Image[] gaugeArray;

    private const float volumeUnit = 0.1f;

    private void Start()
    {
        gaugeArray = GetComponentsInChildren<Image>();
        UpdateGauge();
    }

    private void UpdateGauge()
    {
        float currentVolume = SoundManager.Instance.GetVolume(volumeType);

        for (int i = 0; i < 10; ++i)
        {
            if ((float)i * volumeUnit < currentVolume)
            {
                gaugeArray[i].sprite = on;
            }
            else
            {
                gaugeArray[i].sprite = off;
            }
        }
    }

    public void VolumeUp()
    {
        float currentVolume = SoundManager.Instance.GetVolume(volumeType);
        currentVolume += volumeUnit;

        if (currentVolume > 1)
        {
            currentVolume = 1;
        }
        
        SoundManager.Instance.SetVolume(currentVolume, volumeType);
        UpdateGauge();
    }
    
    public void VolumeDown()
    {
        float currentVolume = SoundManager.Instance.GetVolume(volumeType);
        currentVolume -= volumeUnit;
        
        if (currentVolume < 0)
        {
            currentVolume = 0;
        }
        
        Debug.Log(currentVolume);
        SoundManager.Instance.SetVolume(currentVolume, volumeType);
        UpdateGauge();
    }
}