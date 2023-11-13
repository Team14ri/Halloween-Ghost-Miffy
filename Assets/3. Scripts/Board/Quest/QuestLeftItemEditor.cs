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
    private QuestLocation _location;
    private QuestSummary _summary;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    
    public void Execute()
    {
        _viewer.ShowQuestDetail(_location, _summary);
    }

    public QuestLocation GetLocation()
    {
        return _location;
    }

    public QuestSummary GetSummary()
    {
        return _summary;
    }

    public void SetButtonEnable(bool enable)
    {
        _button.interactable = enable;
    }

    public void Set(QuestLocation location, QuestSummary summary, QuestViewer viewer)
    {
        textField.text = summary.QuestTitle;
        
        _location = location;
        _summary = summary;
        _viewer = viewer;
    }
    
    public void SetSelect(bool isSelect)
    {
        selectBackground.SetActive(isSelect);
        textField.text = $"{(isSelect ? "<color=#DFD0BD>" : "")}{_summary.QuestTitle}";
    }
}
