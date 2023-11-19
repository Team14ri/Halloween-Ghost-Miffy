using System;
using System.Collections;
using System.Collections.Generic;
using Quest;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PolaroidViewer : MonoBehaviour
{
    [SerializeField] private Sprite offImage;
    [SerializeField] private Sprite onImage;

    [SerializeField] private Image polaroidField;
    
    [BoxGroup("Quest Info"), SerializeField]
    private QuestChapter chapterID; 
    [BoxGroup("Quest Info"), SerializeField, Space(5)]
    private int questID; 
    [BoxGroup("Quest Info"), SerializeField, Space(5)]
    private int questDetailID; 
    [BoxGroup("Quest Info"), SerializeField, Space(5)]
    private int questFlowID;
    private void OnEnable()
    {
        polaroidField.sprite = offImage;
        
        var currentQuestInfo = QuestManager.Instance.CurrentQuestInfo;
            
        if ((int)chapterID < currentQuestInfo[0] || 
            ((int)chapterID == currentQuestInfo[0] && questID < currentQuestInfo[1]) ||
            ((int)chapterID == currentQuestInfo[0] && questID == currentQuestInfo[1] && questDetailID < currentQuestInfo[2]) ||
            ((int)chapterID == currentQuestInfo[0] && questID == currentQuestInfo[1] && questDetailID == currentQuestInfo[2] && questFlowID <= currentQuestInfo[3]))
        {
            polaroidField.sprite = onImage;
        }
    }
}

