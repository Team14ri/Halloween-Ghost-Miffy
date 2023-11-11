using FMOD.Studio;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private EventInstance bgmInstance;
    public string bgmEventName = null;
    
    public static BGMPlayer Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("한 씬에 BGMPlayer가 여러 개 있습니다.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        if (bgmEventName == null)
        {
            Debug.Log("bgmName에 해당하는 event가 존재하지 않습니다.");
            return;
        }
        
        bgmInstance = SoundManager.Instance.CreateInstance(FMODEvents.Instance.eventDictionary[bgmEventName]);

        if (!bgmInstance.isValid())
        {
            Debug.Log("FMODEvents로부터 event를 받아오는데 실패했습니다.");
            return;
        }
        
        bgmInstance.start();
    }
}
