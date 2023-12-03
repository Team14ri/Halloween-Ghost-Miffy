using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GaugeManager : MonoBehaviour
{
    [SerializeField] private Sprite on;
    [SerializeField] private Sprite off;

    public SoundManager.BusType[] volumeType;
    private Image[] _gaugeArray;

    private const decimal VolumeUnit = 0.2M;
    private const decimal MinVolume = 0.0M;
    private decimal _maxVolume;

    private void Start()
    {
        Init();
        UpdateGauge();
    }

    private void Init()
    {
        _gaugeArray = GetComponentsInChildren<Image>();
        _maxVolume = VolumeUnit * _gaugeArray.Length;
    }

    private void UpdateGauge()
    {
        decimal currentVolume = (decimal)SoundManager.Instance.GetVolume(volumeType.First());
        
        for (int i = 0; i < _gaugeArray.Length; ++i)
        {
            _gaugeArray[i].sprite = VolumeUnit * i < currentVolume ? on : off;
        }
    }

    public void VolumeUp()
    {
        foreach (var type in volumeType)
        {
            decimal currentVolume = (decimal)SoundManager.Instance.GetVolume(type);
            
            currentVolume += VolumeUnit;
            if (currentVolume >= _maxVolume)
            {
                currentVolume = _maxVolume;
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
            
            currentVolume -= VolumeUnit;
            if (currentVolume < MinVolume)
            {
                currentVolume = MinVolume;
            }

            SoundManager.Instance.SetVolume((float)currentVolume, type);
            UpdateGauge();
        }
    }
}