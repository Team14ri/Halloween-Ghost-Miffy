using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class BGMPlayer : MonoBehaviour
{
    private EventInstance bgmInstance;
    private string currentEventName = null;

    public static BGMPlayer Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.LogError("한 씬에 BGMPlayer가 여러 개 있습니다.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    private void Start()
    {
        currentEventName = "TestMapA";
        PlayBGM(currentEventName);
    }

    public void PlayBGM(string eventName)
    {
        if (eventName == null)
        {
            Debug.Log("eventName에 해당하는 event가 존재하지 않습니다.");
            return;
        }
        bgmInstance = SoundManager.Instance.CreateInstance(FMODEvents.Instance.eventDictionary[eventName]);
        currentEventName = eventName;
            
        if (!bgmInstance.isValid())
        {
            Debug.Log("FMODEvents로부터 event를 받아오는데 실패했습니다.");
            return;
        }
        bgmInstance.start();
    }

    public void ChangeBGM(string eventName)
    {
        EventReference currentEventRef = FMODEvents.Instance.eventDictionary[currentEventName];
        EventReference newEventRef = FMODEvents.Instance.eventDictionary[eventName];
        
        if (currentEventRef.Guid == newEventRef.Guid)
        {
            Debug.Log("같은 event이므로 BGM재생을 재개합니다.");
            return;
        }
        
        SoundManager.Instance.CleanUp();
        PlayBGM(eventName);
    }

    public void StopBGM()
    {
        bgmInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }
}
