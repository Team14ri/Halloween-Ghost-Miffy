using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class VoiceReferences : MonoBehaviour
{
    [Serializable] public class VoiceData
    {
        public string name;
        public EventReference eventReference;
    }
    public VoiceData[] voiceDataArray;
    public Dictionary<string, EventReference> voiceDictionary;

    public static VoiceReferences instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("한 씬에 VoiceReferences가 여러 개 있습니다.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            InitVoiceDictionary();
        }
    }
    
    private void InitVoiceDictionary()
    {
        voiceDictionary = new Dictionary<string, EventReference>();

        foreach (var data in voiceDataArray)
        {
            voiceDictionary[data.name] = data.eventReference;
        }
    }
}