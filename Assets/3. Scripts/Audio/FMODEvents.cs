using System;
using FMODUnity;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SerializableDictionary<T>
{

    public List<SerializeData<T>> BGM;
    public List<SerializeData<T>> AMB;
    public List<SerializeData<T>> SFX;

    private Dictionary<string, T> dict = new Dictionary<string, T>();

    public Dictionary<string, T> getDict()
    {
        for (int i = 0; i < BGM.Count; i++)
        {
            dict.Add(BGM[i].key, BGM[i].eventReference);
        }
        for (int i = 0; i < AMB.Count; i++)
        {
            dict.Add(AMB[i].key, AMB[i].eventReference);
        }
        for (int i = 0; i < SFX.Count; i++)
        {
            dict.Add(SFX[i].key, SFX[i].eventReference);
        }

        return dict;
    }
}
[Serializable]
public class SerializeData<T>
{
    public string key;
    public T eventReference;
}

public class FMODEvents : MonoBehaviour
{
    public SerializableDictionary<EventReference> serializableDictionary;
    public Dictionary<string, EventReference> eventDictionary;

    public static FMODEvents Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("한 씬에 FMODEvents가 여러 개 있습니다.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            eventDictionary = serializableDictionary.getDict();
        }
    }
}
