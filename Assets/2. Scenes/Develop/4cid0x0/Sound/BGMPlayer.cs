using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private EventInstance bgmInstance;
    
    void Start()
    {
        bgmInstance = SoundManager.Instance.CreateInstance(FMODEvents.Instance.testBGM1);
        bgmInstance.start();
    }
}
