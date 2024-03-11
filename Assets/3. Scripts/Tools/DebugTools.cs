using System;
using System.Text.RegularExpressions;
using Quest;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugTools : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown currentLocationField;
    
    [SerializeField] private TMP_Text currentChapterField;
    [SerializeField] private TMP_Text chapterField;

    private void Awake()
    {
#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
    Destroy(gameObject);
#endif
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private void Start()
    {
        switch (SceneManager.GetActiveScene().name.ToLower())
        {
            case "cemetery":
                currentLocationField.value = 0;
                break;
            case "plaza":
                currentLocationField.value = 1;
                break;
            case "plaza_retro":
                currentLocationField.value = 2;
                break;
            case "mall":
                currentLocationField.value = 3;
                break;
            case "mall_retro":
                currentLocationField.value = 4;
                break;
            case "church":
                currentLocationField.value = 5;
                break;
            case "tower":
                currentLocationField.value = 6;
                break;
            case "tower_retro":
                currentLocationField.value = 7;
                break;
            case "tower_top":
                currentLocationField.value = 8;
                break;
            case "tower_top_retro":
                currentLocationField.value = 9;
                break;
        }
    }

    private void Update()
    {
        var currentQuestInfo = QuestManager.Instance.CurrentQuestInfo;
        currentChapterField.text = $"{(QuestChapter)currentQuestInfo[0]}@{currentQuestInfo[1]}-{currentQuestInfo[2]}-{currentQuestInfo[3]}";
    }
    
    public void ChangeLocation(int value)
    {
        string sceneName = "Title";
        
        switch (value)
        {
            case 0:
                sceneName = "Cemetery";
                break;
            case 1:
                sceneName = "Plaza";
                break;
            case 2:
                sceneName = "Plaza_Retro";
                break;
            case 3:
                sceneName = "Mall";
                break;
            case 4:
                sceneName = "Mall_Retro";
                break;
            case 5:
                sceneName = "Church";
                break;
            case 6:
                sceneName = "Tower";
                break;
            case 7:
                sceneName = "Tower_Retro";
                break;
            case 8:
                sceneName = "Tower_Top";
                break;
            case 9:
                sceneName = "Tower_Top_Retro";
                break;
        }
        
        SceneTransitionController.Instance.IrisClose(() =>
        {
            SceneManagerCustom.Instance.LoadScene(sceneName, 1);
        });
    }

    public void ChangeChapter()
    {
        var match = Regex.Match(chapterField.text, @"(\d+)-(\d+)-(\d+)");

        if (match.Success)
        {
            chapterField.text = "";
            
            var chapterID = (int)QuestChapter.Ch1;
            var questID = int.Parse(match.Groups[1].Value);
            var questDetailID = int.Parse(match.Groups[2].Value);
            var questFlowID = int.Parse(match.Groups[3].Value);

            var currentQuestInfo = QuestManager.Instance.CurrentQuestInfo;
            
            if (chapterID < currentQuestInfo[0] || 
                 (chapterID == currentQuestInfo[0] && questID < currentQuestInfo[1]) ||
                 (chapterID == currentQuestInfo[0] && questID == currentQuestInfo[1] && questDetailID < currentQuestInfo[2]) ||
                 (chapterID == currentQuestInfo[0] && questID == currentQuestInfo[1] && questDetailID == currentQuestInfo[2] && questFlowID < currentQuestInfo[3]))
            {
                BGMPlayer.Instance.StopSound();
                
                QuestManager.Instance.SetQuestID($"{chapterID}@{questID}-{questDetailID}-{questFlowID}");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                return;
            }
            
            QuestManager.Instance.SetQuestID($"{chapterID}@{questID}-{questDetailID}-{questFlowID}");
        }
        
        var match2 = Regex.Match(chapterField.text, @"(\w+)@(\d+)-(\d+)-(\d+)");
                
        if (match2.Success)
        {
            chapterField.text = "";
            
            var chapterID = (int)Enum.Parse<QuestChapter>(match2.Groups[1].Value, true);
            var questID = int.Parse(match2.Groups[2].Value);
            var questDetailID = int.Parse(match2.Groups[3].Value);
            var questFlowID = int.Parse(match2.Groups[4].Value);

            var currentQuestInfo = QuestManager.Instance.CurrentQuestInfo;
            
            if (chapterID < currentQuestInfo[0] || 
                (chapterID == currentQuestInfo[0] && questID < currentQuestInfo[1]) ||
                (chapterID == currentQuestInfo[0] && questID == currentQuestInfo[1] && questDetailID < currentQuestInfo[2]) ||
                (chapterID == currentQuestInfo[0] && questID == currentQuestInfo[1] && questDetailID == currentQuestInfo[2] && questFlowID < currentQuestInfo[3]))
            {
                BGMPlayer.Instance.StopSound();
                
                QuestManager.Instance.SetQuestID($"{chapterID}@{questID}-{questDetailID}-{questFlowID}");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                return;
            }
            
            QuestManager.Instance.SetQuestID($"{chapterID}@{questID}-{questDetailID}-{questFlowID}");
        }
    }
#endif
}
