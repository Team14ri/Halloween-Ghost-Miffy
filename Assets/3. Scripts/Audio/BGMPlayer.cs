using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class BGMPlayer : MonoBehaviour
{
    private EventInstance bgmInstance;
    private EventInstance ambInstance;

    private string curEventName = null;
    private string curBGMEventName = null;
    private string curAMBEventName = null;
    
    public static BGMPlayer Instance { get; private set; }
    // TODO: 테스트 용, 삭제 필요
    [SerializeField] private bool playOnStart;
    
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
    
    private void OnEnable()
    {
        SetCurEventName(SceneManager.GetActiveScene().name);
        if (playOnStart)
        {
            PlaySound(curEventName);
        }
    }

    public void SetCurEventName(string eventName)
    {
        curEventName = eventName;
        curBGMEventName = "BGM_" + eventName;
        curAMBEventName = "AMB_" + eventName;
    }

    public void PlaySound(string eventName)
    {
        if (eventName == null)
        {
            Debug.Log("eventName에 해당하는 event가 존재하지 않습니다.");
            return;
        }
        
        SetCurEventName(eventName);
        bgmInstance = SoundManager.Instance.CreateInstance(FMODEvents.Instance.eventDictionary[curBGMEventName]);
        ambInstance = SoundManager.Instance.CreateInstance(FMODEvents.Instance.eventDictionary[curAMBEventName]);

        if (!bgmInstance.isValid() || !ambInstance.isValid())
        {
            Debug.Log("FMODEvents로부터 event를 받아오는데 실패했습니다.");
            return;
        }
        
        bgmInstance.start();
        ambInstance.start();
    }

    public void ChangeBGM(string eventName)
    {
        FMODEvents.Instance.eventDictionary.TryGetValue(curBGMEventName, out EventReference currentEventRef);
        FMODEvents.Instance.eventDictionary.TryGetValue(eventName, out EventReference newEventRef);
        
        if (currentEventRef.IsNull || newEventRef.IsNull)
            return;
        
        if (currentEventRef.Guid == newEventRef.Guid)
        {
            Debug.Log("같은 event이므로 BGM재생을 재개합니다.");
            return;
        }
        
        SoundManager.Instance.CleanUp();
        PlaySound(eventName);
    }

    public void StopSound()
    {
        bgmInstance.stop(STOP_MODE.ALLOWFADEOUT);
        ambInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }


}
