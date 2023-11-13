using System;
using System.Collections.Generic;
using System.Linq;
using Quest;
using UnityEngine;

[RequireComponent(typeof(TmpTextEditor))]
public class QuestRightViewEditor : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject linePrefab;

    [SerializeField] private int questID;
    
    private TmpTextEditor _textEditor;

    private void Awake()
    {
        _textEditor = GetComponent<TmpTextEditor>();
    }
    
    public void DestroyChild()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void UpdateQuestBoard(QuestLocation location, int id)
    {
        var currentQuestID = QuestManager.Instance.CurrentQuestInfo;
        var (summary, data) = QuestManager.Instance.GetQuestData(location);
        var targetSummary = summary.FirstOrDefault(item => item.QuestID == id);
        
        if (targetSummary == null)
            return;

        var targetData = data.Where(item => item.QuestID == id &&
            (item.QuestID < currentQuestID[1] ||
             (item.QuestID == currentQuestID[1] && item.QuestDetailID < currentQuestID[2]) ||
             (item.QuestID == currentQuestID[1] && item.QuestDetailID == currentQuestID[2] && item.QuestFlowID <= currentQuestID[3])))
             .Reverse().ToList();

        SetView(targetSummary.QuestTitle, location, targetData);
    }

    private void SetView(string title, QuestLocation location, List<QuestData> data)
    {
        _textEditor.Edit("Title", title);
        
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        var currentQuestID = QuestManager.Instance.CurrentQuestInfo;
        for (int i = 0; i < data.Count; ++i)
        {
            GameObject temp = Instantiate(itemPrefab, parent.position, Quaternion.identity);
            temp.transform.SetParent(parent);

            bool isQuestClear = (int)location < currentQuestID[0] ||
                                ((int)location == currentQuestID[0] && data[i].QuestID < currentQuestID[1]) ||
                                ((int)location == currentQuestID[0] && data[i].QuestID == currentQuestID[1] && data[i].QuestDetailID < currentQuestID[2]) ||
                                ((int)location == currentQuestID[0] && data[i].QuestID == currentQuestID[1] && data[i].QuestDetailID == currentQuestID[2] && data[i].QuestFlowID < currentQuestID[3]);
            
            temp.GetComponent<QuestRightItemEditor>().SetItem(data[i].QuestTitle, 
                data[i].QuestDescription, data[i].QuestConditions, isQuestClear);

            if (i < data.Count - 1)
            {
                GameObject line = Instantiate(linePrefab, parent.position, Quaternion.identity);
                line.transform.SetParent(parent);
            }
        }
    }
}
