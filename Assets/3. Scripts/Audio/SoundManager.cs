using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve crossfadeCurve;

    private List<EventInstance> _eventInstances;
    private readonly Dictionary<BusType, Bus> _busDictionary = new();
    
    public enum BusType
    {
        Master,
        BGM,
        AMB,
        SFX
    }
    
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _eventInstances = new List<EventInstance>();
            
            _busDictionary.Add(BusType.Master, RuntimeManager.GetBus("{adad2423-c25d-4945-9aeb-435077848bbd}"));
            _busDictionary.Add(BusType.BGM, RuntimeManager.GetBus("{ea6f7763-a705-4240-a8fe-c397e21ca0e9}"));
            _busDictionary.Add(BusType.AMB, RuntimeManager.GetBus("{bed5f750-b5a9-40f3-843b-4bc574cf8e2f}"));
            _busDictionary.Add(BusType.SFX, RuntimeManager.GetBus("{1127e86d-81c8-4e7f-9866-7b56d22ab475}"));
            
            SetVolume(EncryptedPlayerPrefs.GetFloat("Volume@Master", 1f), BusType.Master);
            SetVolume(EncryptedPlayerPrefs.GetFloat("Volume@BGM", 1f), BusType.BGM);
            SetVolume(EncryptedPlayerPrefs.GetFloat("Volume@AMB", 1f), BusType.AMB);
            SetVolume(EncryptedPlayerPrefs.GetFloat("Volume@SFX", 1f), BusType.SFX);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public void CleanUp()
    {
        foreach (EventInstance eventInstance in _eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    public void SetVolume(float vol, BusType type)
    {
        if (_busDictionary.TryGetValue(type, out var bus))
        {
            bus.setVolume(vol);
            switch (type)
            {
                case BusType.Master:
                    EncryptedPlayerPrefs.SetFloat("Volume@Master", vol);
                    break;
                case BusType.BGM:
                    EncryptedPlayerPrefs.SetFloat("Volume@BGM", vol);
                    break;
                case BusType.AMB:
                    EncryptedPlayerPrefs.SetFloat("Volume@AMB", vol);
                    break;
                case BusType.SFX:
                    EncryptedPlayerPrefs.SetFloat("Volume@SFX", vol);
                    break;
            }
        }
        else
        {
            Debug.LogError("BusType에 해당하는 Bus를 찾을 수 없습니다.");
        }
    }

    public float GetVolume(BusType type)
    {
        if (_busDictionary.TryGetValue(type, out var bus))
        {
            bus.getVolume(out var vol);
            return vol;
        }

        Debug.LogError("BusType에 해당하는 Bus를 찾을 수 없습니다.");
        return -1;
    }

    public void Crossfade(float goalVol, float goalTime, BusType type)
    {
        StartCoroutine(CrossfadeCoroutine(goalVol, goalTime, type));
    }

    private IEnumerator CrossfadeCoroutine(float goalVol, float goalTime, BusType type)
    {
        float startTime = Time.time;
        _busDictionary[type].getVolume(out var startVol);

        while (Time.time < startTime + goalTime)
        {
            float elapsed = Time.time - startTime;
            float t = Mathf.Clamp01(elapsed / goalTime);

            float curveValue = Mathf.Clamp01(Instance.crossfadeCurve.Evaluate(t));
            float currentVolume = Mathf.Lerp(startVol, goalVol, curveValue);
            _busDictionary[type].setVolume(currentVolume);

            yield return null;
        }

        _busDictionary[type].setVolume(goalVol);
    }

    public void PlaySound(string key)
    {
        RuntimeManager.PlayOneShot(FMODEvents.Instance.eventDictionary[key]);
    }
    public void PlaySound(string key, Vector3 pos)
    {
        RuntimeManager.PlayOneShot(FMODEvents.Instance.eventDictionary[key], pos);
    }
}