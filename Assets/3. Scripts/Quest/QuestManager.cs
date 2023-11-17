using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Quest
{
    // [Serializable]
    // public enum QuestLocation
    // {
    //     Cemetery = 0,
    //     Plaza = 1,
    //     Mall = 2,
    //     Forest = 3
    // }
    
    [Serializable]
    public enum QuestChapter
    {
        Ch1 = 1,
        Ch2 = 2,
        Ch3 = 3,
    }

    [Serializable]
    public class QuestCondition
    {
        public string ConditionText;
        
        public bool EnableVariable;
        [ShowIf("EnableVariable")] public string VariableID;
        [ShowIf("EnableVariable")] public int EqualOrMany;
    }

    [Serializable]
    public class QuestData
    {
        [BoxGroup("Quest Info")] public string QuestTitle;
        [BoxGroup("Quest Info"), TextArea(3, 10)] public string QuestDescription;
        [BoxGroup("Quest Info")] public int QuestID;
        [BoxGroup("Quest Info")] public int QuestDetailID;
        [BoxGroup("Quest Info")] public int QuestFlowID;

        [BoxGroup("Quest Condition")] public List<QuestCondition> QuestConditions;
    }
    
    [Serializable]
    public class QuestSummary
    {
        [BoxGroup("Quest Info")] public int QuestID;
        [BoxGroup("Quest Condition")] public string QuestTitle;
    }
    
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance;

        [SerializeField] private GameObject questAcceptUI;

        [TabGroup("Quest Data", "Chapter 01"), SerializeField]
        private List<QuestSummary> chapter01QuestSummary;
        [TabGroup("Quest Data", "Chapter 01"), SerializeField]
        private List<QuestData> chapter01QuestData;
        
        [TabGroup("Quest Data", "Chapter 02"), SerializeField]
        private List<QuestSummary> chapter02QuestSummary;
        [TabGroup("Quest Data", "Chapter 02"), SerializeField]
        private List<QuestData> chapter02QuestData;
        
        [TabGroup("Quest Data", "Chapter 03"), SerializeField]
        private List<QuestSummary> chapter03QuestSummary;
        [TabGroup("Quest Data", "Chapter 03"), SerializeField]
        private List<QuestData> chapter03QuestData;

        private TmpTextEditor _questAcceptTmpTextEditor;
        private UIFadeController _questAcceptFadeController;
        
        public int[] CurrentQuestInfo
        {
            get => new int[]
            {
                PlayerPrefs.GetInt("CurrentQuest@ChapterID", (int)QuestChapter.Ch1),
                PlayerPrefs.GetInt("CurrentQuest@ID", 0),
                PlayerPrefs.GetInt("CurrentQuest@DetailID", 0),
                PlayerPrefs.GetInt("CurrentQuest@FlowID", 1)
            };  
            set
            {
                PlayerPrefs.SetInt("CurrentQuest@ChapterID", value[0]);
                PlayerPrefs.SetInt("CurrentQuest@ID", value[1]);
                PlayerPrefs.SetInt("CurrentQuest@DetailID", value[2]);
                PlayerPrefs.SetInt("CurrentQuest@FlowID", value[3]);
            }
        }
        
        public (List<QuestSummary>, List<QuestData>) GetQuestData(QuestChapter chapter)
        {
            List<QuestSummary> returnSummary = new();
            List<QuestData> returnData = new();
            
            switch (chapter)
            {
                case QuestChapter.Ch1:
                    returnSummary = chapter01QuestSummary;
                    returnData = chapter01QuestData;
                    break;
                case QuestChapter.Ch2:
                    returnSummary = chapter02QuestSummary;
                    returnData = chapter02QuestData;
                    break;
                case QuestChapter.Ch3:
                    returnSummary = chapter03QuestSummary;
                    returnData = chapter03QuestData;
                    break;
            }

            return (returnSummary, returnData);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _questAcceptTmpTextEditor = questAcceptUI.GetComponent<TmpTextEditor>();
            _questAcceptFadeController = questAcceptUI.GetComponent<UIFadeController>();
        }
        
        public void Accept(string questType, string questTitle)
        {
            _questAcceptTmpTextEditor.Edit("Quest Type", questType)
                .Edit("Quest Title", questTitle);
            _questAcceptFadeController.AutoFadeInAndOut();
        }
        
        public void AcceptMainQuest(string questTitle)
        {
            _questAcceptTmpTextEditor.Edit("Quest Type", "MAIN QUEST")
                .Edit("Quest Title", questTitle);
            _questAcceptFadeController.AutoFadeInAndOut();
        }
        
        public void AcceptSubQuest(string questTitle)
        {
            _questAcceptTmpTextEditor.Edit("Quest Type", "SUB QUEST")
                .Edit("Quest Title", questTitle);
            _questAcceptFadeController.AutoFadeInAndOut();
        }
        
        public void SetQuestID(string input)
        {
            var match = Regex.Match(input, @"(\w+)@(\d+)-(\d+)-(\d+)");
            if (match.Success)
            {
                var chapterID = Enum.Parse<QuestChapter>(match.Groups[1].Value, true);
                var questID = int.Parse(match.Groups[2].Value);
                var questDetailID = int.Parse(match.Groups[3].Value);
                var questFlowID = int.Parse(match.Groups[4].Value);
                CurrentQuestInfo = new[] { (int)chapterID, questID, questDetailID, questFlowID };
            }
        }
    }
}