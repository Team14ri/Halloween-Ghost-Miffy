using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GaugeManager : MonoBehaviour
{
    [SerializeField] private Sprite on;
    [SerializeField] private Sprite off;

    public SoundManager.BusType[] volumeType;
    private Image[] gaugeArray;

    private const decimal volumeUnit = 0.2M;
    private const decimal minVolume = 0.0M;
    private decimal maxVolume;

    private void Start()
    {
        Init();
        UpdateGauge();
    }

    private void Init()
    {
        gaugeArray = GetComponentsInChildren<Image>();
        maxVolume = 0.2M * gaugeArray.Length;
    }

    private void UpdateGauge()
    {
        decimal currentVolume = (decimal)SoundManager.Instance.GetVolume(volumeType.First());
        Debug.Log(currentVolume);
        
        for (int i = 0; i < gaugeArray.Length; ++i)
        {
            if (volumeUnit * i < currentVolume)
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
        foreach (var type in volumeType)
        {
            decimal currentVolume = (decimal)SoundManager.Instance.GetVolume(type);
            currentVolume += volumeUnit;
            if (currentVolume >= maxVolume)
            {
                currentVolume = maxVolume;
            }
        
            SoundManager.Instance.SetVolume((float)currentVolume, type);
            UpdateGauge();
        }
    }
    
    public void VolumeDown()
    {
        foreach (var type in volumeType)
        {
            decimal currentVolume = (decimal)SoundManager.Instance.GetVolume(type);
            currentVolume -= volumeUnit;
            if (currentVolume < minVolume)
            {
                currentVolume = minVolume;
            }

            SoundManager.Instance.SetVolume((float)currentVolume, type);
            UpdateGauge();
        }
    }
}