using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private EventInstance bgmInstance;
    
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
        bgmInstance = SoundManager.Instance.CreateInstance(FMODEvents.Instance.testBGM1);
        bgmInstance.start();
    }
}
