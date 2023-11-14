using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve crossfadeCurve;

    private List<EventInstance> eventInstances;
    private Bus masterBus;
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
            transform.parent = null;
            DontDestroyOnLoad(transform.gameObject);
            eventInstances = new List<EventInstance>();
            masterBus = FMODUnity.RuntimeManager.GetBus("{adad2423-c25d-4945-9aeb-435077848bbd}");
        }
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
        masterBus.setVolume(vol);
    }

    public void Crossfade(float goalVol, float goalTime)
    {
        StartCoroutine(CrossfadeCoroutine(goalVol, goalTime));
    }

    private IEnumerator CrossfadeCoroutine(float goalVol, float goalTime)
    {
        float startTime = Time.time;
        float startVol = currentMasterVolume;

        while (Time.time < startTime + goalTime)
        {
            float elapsed = Time.time - startTime;
            float t = Mathf.Clamp01(elapsed / goalTime);

            float curveValue = Mathf.Clamp01(SoundManager.Instance.crossfadeCurve.Evaluate(t));
            currentMasterVolume = Mathf.Lerp(startVol, goalVol, curveValue);
            masterBus.setVolume(currentMasterVolume);

            yield return null;
        }

        currentMasterVolume = goalVol;
        masterBus.setVolume(currentMasterVolume);
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