using System;
using Quest;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestLeftItemEditor : MonoBehaviour
{
    [SerializeField] private GameObject selectBackground;
    [SerializeField] private TMP_Text textField;

    private Button _button;
    private QuestViewer _viewer;
    private QuestChapter _chapter;
    private QuestSummary _summary;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    
    public void Execute()
    {
        _viewer.ShowQuestDetail(_chapter, _summary);
    }

    public QuestChapter GetChapter()
    {
        return _chapter;
    }

    public QuestSummary GetSummary()
    {
        return _summary;
    }

    public void SetButtonEnable(bool enable)
    {
        _button.interactable = enable;
    }

    public void Set(QuestChapter chapter, QuestSummary summary, QuestViewer viewer)
    {
        textField.text = summary.QuestTitle;
        
        _chapter = chapter;
        _summary = summary;
        _viewer = viewer;
    }
    
    public void SetSelect(bool isSelect)
    {
        selectBackground.SetActive(isSelect);
        textField.text = $"{(isSelect ? "<color=#DFD0BD>" : "")}{_summary.QuestTitle}";
    }
}
