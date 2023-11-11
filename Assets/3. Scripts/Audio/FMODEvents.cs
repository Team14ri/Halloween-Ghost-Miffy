using System;
using FMODUnity;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SerializableDictionary<T>
{
    public List<SerializeData<T>> data;
    private Dictionary<string, T> dict = new Dictionary<string, T>();

    public Dictionary<string, T> getDict()
    {
        for (int i = 0; i < data.Count; i++)
        {
            dict.Add(data[i].key, data[i].value);
        }

        return dict;
    }
}
[Serializable]
public class SerializeData<T>
{
    public string key;
    public T value;
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
            DontDestroyOnLoad(gameObject);
            eventDictionary = serializableDictionary.getDict();
        }
    }
}
