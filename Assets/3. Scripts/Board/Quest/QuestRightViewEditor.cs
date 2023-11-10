using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestRightData
{
    public string title;
    [TextArea(3, 10)]
    public string description;
    public List<string> conditions;
    public bool clear;
}

[RequireComponent(typeof(TmpTextEditor))]
public class QuestRightViewEditor : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject linePrefab;

    [SerializeField] private string questTitle;
    [SerializeField] private List<QuestRightData> questRightData;
    
    private TmpTextEditor _textEditor;

    private void Awake()
    {
        _textEditor = GetComponent<TmpTextEditor>();
    }
    
    private void Start()
    {
        SetView(questTitle, questRightData);
    }

    public void SetView(string title, List<QuestRightData> data)
    {
        _textEditor.Edit("Title", title);
        
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < data.Count; ++i)
        {
            GameObject temp = Instantiate(itemPrefab, parent.position, Quaternion.identity);
            temp.transform.SetParent(parent);
            
            temp.GetComponent<QuestRightItemEditor>().SetItem(questRightData[i].title, 
                questRightData[i].description, questRightData[i].conditions, questRightData[i].clear);

            if (i < data.Count - 1)
            {
                GameObject line = Instantiate(linePrefab, parent.position, Quaternion.identity);
                line.transform.SetParent(parent);
            }
        }
    }
}
