using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Quest;
using Sirenix.OdinInspector;
using UnityEngine;

public class QuestViewer : MonoBehaviour
{
    [TabGroup("Chapter", "Chapter 01"), SerializeField] private GameObject chapter01FoldField;
    [TabGroup("Chapter", "Chapter 01"), SerializeField] private Transform chapter01ListField;
    
    [TabGroup("Chapter", "Chapter 02"), SerializeField] private GameObject chapter02FoldField;
    [TabGroup("Chapter", "Chapter 02"), SerializeField] private Transform chapter02ListField;

    [SerializeField] private GameObject questLeftItemPrefab;
    [SerializeField] private QuestRightViewEditor viewEditor;

    private readonly List<QuestLeftItemEditor> _chapter01Editor = new();
    private readonly List<QuestLeftItemEditor> _chapter02Editor = new();

    public void ShowQuestDetail(QuestChapter chapter, QuestSummary summary)
    {
        foreach (var editor in _chapter01Editor)
        {
            bool isSelected = editor.GetChapter() == chapter && editor.GetSummary().QuestID == summary.QuestID;
            editor.SetSelect(isSelected);
        }
        foreach (var editor in _chapter02Editor)
        {
            bool isSelected = editor.GetChapter() == chapter && editor.GetSummary().QuestID == summary.QuestID;
            editor.SetSelect(isSelected);
        }
        viewEditor.UpdateQuestBoard(chapter, summary.QuestID);
    }

    private void OnEnable()
    {
        StartCoroutine(OnEnableOneFrameLate());
    }

    private IEnumerator OnEnableOneFrameLate()
    {
        yield return null;
        
        viewEditor.DestroyChild();
        SetupQuestView(chapter01FoldField, chapter01ListField, _chapter01Editor, QuestChapter.Ch1);
        SetupQuestView(chapter02FoldField, chapter02ListField, _chapter02Editor, QuestChapter.Ch2);
        
        var currentQuestInfo = QuestManager.Instance.CurrentQuestInfo;

        switch (currentQuestInfo[0])
        {
            case (int)QuestChapter.Ch1:
                chapter01FoldField.SetActive(true);
                _chapter01Editor.LastOrDefault()?.SetSelect(true);
                _chapter01Editor.LastOrDefault()?.Execute();
                break;
            case (int)QuestChapter.Ch2:
                chapter02FoldField.SetActive(true);
                _chapter02Editor.LastOrDefault()?.SetSelect(true);
                _chapter02Editor.LastOrDefault()?.Execute();
                break;
        }
    }

    private void SetupQuestView(GameObject foldField, Transform listField, List<QuestLeftItemEditor> editors, QuestChapter chapter)
    {
        editors.Clear();
        
        foldField.SetActive(false);
        
        foreach (Transform child in listField)
        {
            Destroy(child.gameObject);
        }

        var currentQuestID = QuestManager.Instance.CurrentQuestInfo;
        var (summary, data) = QuestManager.Instance.GetQuestData(chapter);

        var filteredSummary = summary.Where(s => (int)chapter < currentQuestID[0] ||
                                                 ((int)chapter == currentQuestID[0] && s.QuestID <= currentQuestID[1])).ToList();

        if (filteredSummary.Count == 0)
        {
            CreateQuestItem(listField, chapter, new QuestSummary
            {
                QuestID = -1,
                QuestTitle = "아직 진행할 수 없다"
            }, false, editors);
            return;
        }
        
        foreach (var s in filteredSummary)
        {
            CreateQuestItem(listField, chapter, s, true, editors);
        }
    }

    private void CreateQuestItem(Transform listField, QuestChapter chapter, QuestSummary summary, bool isButtonEnabled, List<QuestLeftItemEditor> editors)
    {
        GameObject temp = Instantiate(questLeftItemPrefab, listField.position, Quaternion.identity);
        temp.transform.SetParent(listField);
        QuestLeftItemEditor editor = temp.GetComponent<QuestLeftItemEditor>();
        editor.Set(chapter, summary, this);
        editor.SetButtonEnable(isButtonEnabled);
        if (isButtonEnabled)
        {
            editors.Add(editor);
        }
    }
}
