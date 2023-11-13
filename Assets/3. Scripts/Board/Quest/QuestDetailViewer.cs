using System;
using System.Linq;
using Quest;
using TMPro;
using UnityEngine;

public class QuestDetailViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text questTitle;
    [SerializeField] private TMP_Text questSubTitle;
    [SerializeField] private GameObject questDetailItemPrefab;

    [SerializeField] private Transform targetParent;

    private int[] _saveQuestID;
    
    private void Start()
    {
        _saveQuestID = QuestManager.Instance.CurrentQuestInfo;
        UpdateQuestDetail();
    }

    private void Update()
    {
        var updateQuestID = QuestManager.Instance.CurrentQuestInfo;
        var sequenceEqual = _saveQuestID.SequenceEqual(updateQuestID);
        if (!sequenceEqual)
        {
            _saveQuestID = updateQuestID;
            UpdateQuestDetail();
        }
    }

    private void UpdateQuestDetail()
    {
        foreach (Transform child in targetParent)
        {
            Destroy(child.gameObject);
        }
        
        var currentQuestID = QuestManager.Instance.CurrentQuestInfo;
        var (summary, data) = QuestManager.Instance.GetQuestData((QuestLocation)currentQuestID[0]);

        var targetSummary = summary.FirstOrDefault(item => item.QuestID == currentQuestID[1]);
        
        var targetData = data.LastOrDefault(item => item.QuestID < currentQuestID[1] ||
            (item.QuestID == currentQuestID[1] && item.QuestDetailID < currentQuestID[2]) ||
            (item.QuestID == currentQuestID[1] && item.QuestDetailID == currentQuestID[2] && item.QuestFlowID <= currentQuestID[3]));

        questTitle.text = targetSummary?.QuestTitle ?? "";
        questSubTitle.text = targetData?.QuestTitle ?? "";

        foreach (var condition in targetData.QuestConditions)
        {
            GameObject temp = Instantiate(questDetailItemPrefab, targetParent.position, Quaternion.identity);
            temp.transform.SetParent(targetParent);
            string conditionText = "";
            if (condition.EnableVariable)
            {
                int clampValue = Mathf.Min(VariableManager.Instance.GetVariableValue(condition.VariableID), condition.EqualOrMany);
                conditionText = $"{clampValue}/{condition.EqualOrMany}";
            }
            temp.GetComponent<TmpTextEditor>().Edit("Title", $"{condition.ConditionText}")
                .Edit("Condition", conditionText);
        }
    }
}
