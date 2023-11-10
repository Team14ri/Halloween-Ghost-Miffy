using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRightConditionEditor : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    public void SetConditions(List<string> conditions, bool clear = false)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var text in conditions)
        {
            GameObject temp = Instantiate(prefab, transform.position, Quaternion.identity);
            temp.transform.SetParent(transform);

            string conditionText = text;
            if (clear)
            {
                conditionText = $"<s>{text}</s>";
            }
            temp.GetComponent<TmpTextEditor>().Edit("Text", conditionText);
        }
    }
}
