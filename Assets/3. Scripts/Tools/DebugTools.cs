using System;
using System.Text.RegularExpressions;
using Quest;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugTools : MonoBehaviour
{
    [SerializeField] private TMP_Text currentChapterField;
    [SerializeField] private TMP_Text chapterField;

    private void Update()
    {
        var currentQuestInfo = QuestManager.Instance.CurrentQuestInfo;
        currentChapterField.text = $"{(QuestChapter)currentQuestInfo[0]}@{currentQuestInfo[1]}-{currentQuestInfo[2]}-{currentQuestInfo[3]}";
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
}
