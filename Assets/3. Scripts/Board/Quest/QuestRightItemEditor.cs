using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TmpTextEditor))]
public class QuestRightItemEditor : MonoBehaviour
{
    [SerializeField] private QuestRightConditionEditor conditionEditor;
    [SerializeField] private GameObject clearStamp;
    private TmpTextEditor _textEditor;

    private void Awake()
    {
        _textEditor = GetComponent<TmpTextEditor>();
    }
    
    public void SetItem(string title, string description, List<string> conditions, bool clear = false)
    {
        _textEditor.Edit("Title", title).Edit("Description", description);
        conditionEditor.SetConditions(conditions, clear);
        clearStamp.SetActive(clear);
    }
}
