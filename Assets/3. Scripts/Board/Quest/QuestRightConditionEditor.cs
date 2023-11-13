using System;
using System.Collections;
using System.Collections.Generic;
using Quest;
using UnityEngine;

public class QuestRightConditionEditor : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    public void SetConditions(List<QuestCondition> conditions)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var condition in conditions)
        {
            GameObject temp = Instantiate(prefab, transform.position, Quaternion.identity);
            temp.transform.SetParent(transform);

            string conditionText = condition.ConditionText;
            if (condition.EnableVariable)
            {
                int clampValue = Mathf.Min(VariableManager.Instance.GetVariableValue(condition.VariableID), condition.EqualOrMany);
                conditionText = $"{condition.ConditionText} ({clampValue}/{condition.EqualOrMany})";
            }
            
            temp.GetComponent<TmpTextEditor>().Edit("Text", conditionText);
        }
    }
}
