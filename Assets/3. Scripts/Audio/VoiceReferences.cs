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
    public Dictionary<string, EventReference> VoiceDictionary;

    public static VoiceReferences Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            InitVoiceDictionary();
        }
    }
    
    private void InitVoiceDictionary()
    {
        VoiceDictionary = new Dictionary<string, EventReference>();

        foreach (var data in voiceDataArray)
        {
            VoiceDictionary[data.name] = data.eventReference;
        }
    }
}