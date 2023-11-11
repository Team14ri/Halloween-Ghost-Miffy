using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;
    private float currentMasterVolume = 1.0f;
    
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
            DontDestroyOnLoad(gameObject);
            eventInstances = new List<EventInstance>();
        }
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstance.setVolume(currentMasterVolume);
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

    public void SetMasterVolume(float vol)
    {
        currentMasterVolume = vol;
        
        foreach (var instance in eventInstances)
        {
            instance.setVolume(vol);
        }
    }
}