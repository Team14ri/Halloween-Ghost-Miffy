using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve crossfadeCurve;

    private List<EventInstance> eventInstances;
    private Dictionary<BusType, Bus> busDictionary = new Dictionary<BusType, Bus>();
    
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
        if (Instance != null)
        {
            //Debug.LogError("한 씬에 SoundManager가 여러 개 있습니다.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // transform.parent = null;
            // DontDestroyOnLoad(transform.gameObject);
            eventInstances = new List<EventInstance>();
            
            busDictionary.Add(BusType.Master, FMODUnity.RuntimeManager.GetBus("{adad2423-c25d-4945-9aeb-435077848bbd}"));
            busDictionary.Add(BusType.BGM, FMODUnity.RuntimeManager.GetBus("{ea6f7763-a705-4240-a8fe-c397e21ca0e9}"));
            busDictionary.Add(BusType.AMB, FMODUnity.RuntimeManager.GetBus("{bed5f750-b5a9-40f3-843b-4bc574cf8e2f}"));
            busDictionary.Add(BusType.SFX, FMODUnity.RuntimeManager.GetBus("{1127e86d-81c8-4e7f-9866-7b56d22ab475}"));
        }
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    public void SetVolume(float vol, BusType type)
    {
        if (busDictionary.TryGetValue(type, out var bus))
        {
            bus.setVolume(vol);
        }
        else
        {
            Debug.LogError("BusType에 해당하는 Bus를 찾을 수 없습니다.");
        }
    }

    public float GetVolume(BusType type)
    {
        if (busDictionary.TryGetValue(type, out var bus))
        {
            float vol;
            bus.getVolume(out vol);
            return vol;
        }
        else
        {
            Debug.LogError("BusType에 해당하는 Bus를 찾을 수 없습니다.");
            return -1;
        }
    }

    public void Crossfade(float goalVol, float goalTime, BusType type)
    {
        StartCoroutine(CrossfadeCoroutine(goalVol, goalTime, type));
    }

    private IEnumerator CrossfadeCoroutine(float goalVol, float goalTime, BusType type)
    {
        float startTime = Time.time;
        float startVol = 0f;
        busDictionary[type].getVolume(out startVol);

        while (Time.time < startTime + goalTime)
        {
            float elapsed = Time.time - startTime;
            float t = Mathf.Clamp01(elapsed / goalTime);

            float curveValue = Mathf.Clamp01(SoundManager.Instance.crossfadeCurve.Evaluate(t));
            float currentVolume = Mathf.Lerp(startVol, goalVol, curveValue);
            busDictionary[type].setVolume(currentVolume);

            yield return null;
        }

        busDictionary[type].setVolume(goalVol);
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