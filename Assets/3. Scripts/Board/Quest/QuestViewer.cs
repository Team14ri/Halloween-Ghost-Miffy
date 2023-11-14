using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Quest;
using Sirenix.OdinInspector;
using UnityEngine;

public class QuestViewer : MonoBehaviour
{
    [BoxGroup("Plaza"), SerializeField] private GameObject plazaFoldField;
    [BoxGroup("Plaza"), SerializeField] private Transform plazaListField;
    
    [BoxGroup("Mall"), SerializeField] private GameObject mallFoldField;
    [BoxGroup("Mall"), SerializeField] private Transform mallListField;

    [SerializeField] private GameObject questLeftItemPrefab;
    [SerializeField] private QuestRightViewEditor viewEditor;

    private readonly List<QuestLeftItemEditor> _plazaEditor = new();
    private readonly List<QuestLeftItemEditor> _mallEditor = new();

    public void ShowQuestDetail(QuestLocation location, QuestSummary summary)
    {
        foreach (var editor in _plazaEditor)
        {
            bool isSelected = editor.GetLocation() == location && editor.GetSummary().QuestID == summary.QuestID;
            editor.SetSelect(isSelected);
        }
        foreach (var editor in _mallEditor)
        {
            bool isSelected = editor.GetLocation() == location && editor.GetSummary().QuestID == summary.QuestID;
            editor.SetSelect(isSelected);
        }
        viewEditor.UpdateQuestBoard(location, summary.QuestID);
    }

    private void OnEnable()
    {
        StartCoroutine(OnEnableOneFrameLate());
    }

    private IEnumerator OnEnableOneFrameLate()
    {
        yield return null;
        
        viewEditor.DestroyChild();
        SetupQuestView(plazaFoldField, plazaListField, _plazaEditor, QuestLocation.Plaza);
        SetupQuestView(mallFoldField, mallListField, _mallEditor, QuestLocation.Mall);
        
        var currentQuestID = QuestManager.Instance.CurrentQuestInfo;
        if (currentQuestID[0] == (int)QuestLocation.Cemetery || 
            currentQuestID[0] == (int)QuestLocation.Plaza)
        {
            plazaFoldField.SetActive(true);
            _plazaEditor.LastOrDefault()?.SetSelect(true);
            _plazaEditor.LastOrDefault()?.Execute();
        }
        if (currentQuestID[0] == (int)QuestLocation.Mall || 
            currentQuestID[0] == (int)QuestLocation.Forest)
        {
            mallFoldField.SetActive(true);
            _mallEditor.LastOrDefault()?.SetSelect(true);
            _mallEditor.LastOrDefault()?.Execute();
        }
    }

    private void SetupQuestView(GameObject foldField, Transform listField, List<QuestLeftItemEditor> editors, QuestLocation location)
    {
        editors.Clear();
        
        foldField.SetActive(false);
        
        foreach (Transform child in listField)
        {
            Destroy(child.gameObject);
        }

        var currentQuestID = QuestManager.Instance.CurrentQuestInfo;
        var (summary, data) = QuestManager.Instance.GetQuestData(location);

        if (currentQuestID[0] == (int)QuestLocation.Cemetery)
        {
            currentQuestID[0] = (int)QuestLocation.Plaza;
        }

        var filteredSummary = summary.Where(s => (int)location < currentQuestID[0] ||
                                                 ((int)location == currentQuestID[0] && s.QuestID <= currentQuestID[1])).ToList();

        if (filteredSummary.Count == 0)
        {
            CreateQuestItem(listField, location, new QuestSummary
            {
                QuestID = -1,
                QuestTitle = "아직 진행할 수 없다"
            }, false, editors);
            return;
        }
        
        foreach (var s in filteredSummary)
        {
            CreateQuestItem(listField, location, s, true, editors);
        }
    }

    private void CreateQuestItem(Transform listField, QuestLocation location, QuestSummary summary, bool isButtonEnabled, List<QuestLeftItemEditor> editors)
    {
        GameObject temp = Instantiate(questLeftItemPrefab, listField.position, Quaternion.identity);
        temp.transform.SetParent(listField);
        QuestLeftItemEditor editor = temp.GetComponent<QuestLeftItemEditor>();
        editor.Set(location, summary, this);
        editor.SetButtonEnable(isButtonEnabled);
        if (isButtonEnabled)
        {
            editors.Add(editor);
        }
    }
}
