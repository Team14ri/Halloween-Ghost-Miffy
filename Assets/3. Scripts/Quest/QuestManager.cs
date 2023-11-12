using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Quest
{
    [Serializable]
    public enum QuestLocation
    {
        Cemetery = 0,
        Plaza = 1,
        Mall = 2,
        Forest = 3
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
        
        [TabGroup("Quest Data", "Plaza"), SerializeField]
        private List<QuestSummary> plazaQuestSummary;
        [TabGroup("Quest Data", "Plaza"), SerializeField]
        private List<QuestData> plazaQuestData;
        
        [TabGroup("Quest Data", "Mall"), SerializeField]
        private List<QuestSummary> mallQuestSummary;
        [TabGroup("Quest Data", "Mall"), SerializeField]
        private List<QuestData> mallQuestData;

        private TmpTextEditor _questAcceptTmpTextEditor;
        private UIFadeController _questAcceptFadeController;
        
        public int[] CurrentQuestInfo
        {
            get => new int[]
            {
                PlayerPrefs.GetInt("CurrentQuest@LocationID", (int)QuestLocation.Cemetery),
                PlayerPrefs.GetInt("CurrentQuest@ID", 0),
                PlayerPrefs.GetInt("CurrentQuest@DetailID", 1),
                PlayerPrefs.GetInt("CurrentQuest@FlowID", 1)
            };  
            set
            {
                PlayerPrefs.SetInt("CurrentQuest@LocationID", value[0]);
                PlayerPrefs.SetInt("CurrentQuest@ID", value[1]);
                PlayerPrefs.SetInt("CurrentQuest@DetailID", value[2]);
                PlayerPrefs.SetInt("CurrentQuest@FlowID", value[3]);
            }
        }
        
        public (List<QuestSummary>, List<QuestData>) GetQuestData(QuestLocation location)
        {
            if (location == QuestLocation.Plaza)
            {
                return (plazaQuestSummary, plazaQuestData);
            }

            if (location == QuestLocation.Mall)
            {
                return (mallQuestSummary, mallQuestData);
            }

            return (null, null);
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
                var locationID = Enum.Parse<QuestLocation>(match.Groups[1].Value, true);
                var questID = int.Parse(match.Groups[2].Value);
                var questDetailID = int.Parse(match.Groups[3].Value);
                var questFlowID = int.Parse(match.Groups[4].Value);
                CurrentQuestInfo = new[] { (int)locationID, questID, questDetailID, questFlowID };
            }
        }
    }
}