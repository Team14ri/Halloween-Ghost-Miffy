using System;
using System.Collections.Generic;
using Quest;
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
    
    public void SetItem(string title, string description, List<QuestCondition> conditions, bool clear = false)
    {
        _textEditor.Edit("Title", clear ? $"<s>{title}</s>" : title)
            .Edit("Description", description);
        conditionEditor.SetConditions(conditions);
        clearStamp.SetActive(clear);
    }
}
