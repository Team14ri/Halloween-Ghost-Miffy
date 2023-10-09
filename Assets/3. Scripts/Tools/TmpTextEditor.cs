using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TmpTextEditor : MonoBehaviour
{
    public readonly Dictionary<string, TMP_Text> Texts = new();
    [SerializeField] private List<TMP_Text> textStructs = new();

    private void Awake()
    {
        foreach (var text in textStructs)
        {
            Texts.TryAdd(text.name, text);
        }
    }
    
    public TmpTextEditor Edit(string objName, string text)
    {
        if (!Texts.TryGetValue(objName, out TMP_Text value)) 
            throw new Exception($"TMP_Text not found with name: {objName}");
        value.text = text;
        return this;
    }
}
