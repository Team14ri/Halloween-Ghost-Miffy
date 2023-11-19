using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DS.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Quest
{
    [Serializable]
    public enum QuestClearConditionType
    {
        Item,
        Variable
    }
    
    [Serializable]
    public class QuestClearCondition
    {
        public QuestClearConditionType type;
        public string variableID;
        public int equalOrMany;

        public bool IsClear()
        {
            switch (type)
            {
                case QuestClearConditionType.Item:
                    return VariableManager.Instance.GetItemValue(variableID) >= equalOrMany;
                default:
                    return PlayerPrefs.GetInt(variableID, 0) >= equalOrMany;
            }
        }
    }
    
    [Serializable]
    public class QuestFlow
    {
        [TabGroup("Settings", "Quest Info")]
        public QuestChapter QuestChapterID; 
        [TabGroup("Settings", "Quest Info"), Space(5)]
        public int QuestID; 
        [TabGroup("Settings", "Quest Info"), Space(5)]
        public int QuestDetailID; 
        [TabGroup("Settings", "Quest Info"), Space(5)]
        public int QuestFlowID;

        [TabGroup("Settings", "Update Quest")]
        public List<GameObject> ActiveGameObjects;
        [TabGroup("Settings", "Update Quest"), Space(5)]
        public List<GameObject> InactiveGameObjects;
        [TabGroup("Settings", "Update Quest"), Space(5)]
        public UnityEvent ActiveEvents;
        
        [TabGroup("Settings", "Auto Clear Quest")]
        public bool enableAutoQuestClear;
        [TabGroup("Settings", "Auto Clear Quest"), ShowIf("enableAutoQuestClear"), Space(10)]
        public QuestChapter NextQuestChapterID;
        [TabGroup("Settings", "Auto Clear Quest"), ShowIf("enableAutoQuestClear"), Space(5)]
        public int NextQuestID; 
        [TabGroup("Settings", "Auto Clear Quest"), ShowIf("enableAutoQuestClear"), Space(5)]
        public int NextQuestDetailID; 
        [TabGroup("Settings", "Auto Clear Quest"), ShowIf("enableAutoQuestClear"), Space(5)]
        public int NextQuestFlowID;
        [TabGroup("Settings", "Auto Clear Quest"), ShowIf("enableAutoQuestClear"), Space(10)]
        public List<QuestClearCondition> QuestClearConditions;
        
        public bool QuestClearConditionsState()
        {
            bool result = true;
            foreach (var condition in QuestClearConditions)
            {
                if (!condition.IsClear())
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
    
    public class QuestFlowManager : MonoBehaviour
    {
        [SerializeField] private bool resetQuestData;
        [ShowIf("resetQuestData"), Space(10)]
        public QuestChapter resetQuestChapterID = QuestChapter.Ch1;
        [ShowIf("resetQuestData"), Space(5)]
        public int resetQuestID; 
        [ShowIf("resetQuestData"), Space(5)]
        public int resetQuestDetailID; 
        [ShowIf("resetQuestData"), Space(5)]
        public int resetQuestFlowID = 1;

        [SerializeField] private List<QuestFlow> QuestFlows;

        public static QuestFlowManager Instance;

        private QuestChapter currentQuestChapterID;
        private int currentQuestID; 
        private int currentQuestDetailID; 
        private int currentQuestFlowID;

        private bool observeUpdate = true;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (resetQuestData)
            {
                QuestManager.Instance.CurrentQuestInfo = new[]
                    { (int)resetQuestChapterID, resetQuestID, resetQuestDetailID, resetQuestFlowID };
            }
#endif
            
            var questInfo = QuestManager.Instance.CurrentQuestInfo;
            currentQuestChapterID = (QuestChapter)questInfo[0];
            currentQuestID = questInfo[1];
            currentQuestDetailID = questInfo[2];
            currentQuestFlowID = questInfo[3];

            UpdateScene();
        }

        private void Update()
        {
            if (!observeUpdate)
                return;
            
            var questInfo = QuestManager.Instance.CurrentQuestInfo;
            if (currentQuestChapterID != (QuestChapter)questInfo[0] || 
                currentQuestID != questInfo[1] || 
                currentQuestDetailID != questInfo[2] || 
                currentQuestFlowID != questInfo[3])
            {
                currentQuestChapterID = (QuestChapter)questInfo[0];
                currentQuestID = questInfo[1];
                currentQuestDetailID = questInfo[2];
                currentQuestFlowID = questInfo[3];
                
                UpdateScene();
                
                return;
            }

            var currentQuest = QuestFlows.FirstOrDefault(flow => 
                flow.QuestChapterID == currentQuestChapterID && 
                flow.QuestID == currentQuestID && 
                flow.QuestDetailID == currentQuestDetailID && 
                flow.QuestFlowID == currentQuestFlowID);

            if (currentQuest == null ||
                !currentQuest.enableAutoQuestClear ||
                !currentQuest.QuestClearConditionsState())
                return;
            
            QuestManager.Instance.CurrentQuestInfo = new[]
                { (int)currentQuest.NextQuestChapterID, currentQuest.NextQuestID, currentQuest.NextQuestDetailID, currentQuest.NextQuestFlowID };
        }
        
        public void SetQuestID(string id)
        {
            QuestManager.Instance.SetQuestID(id);
        }
        
        public void AcceptMainQuest(string questTitle)
        {
            QuestManager.Instance.AcceptMainQuest(questTitle);
        }
        
        public void AddOneItem(string id)
        {
            VariableManager.Instance.AddOneItem(id);
        }
        
        public void AddOneItemWithoutPopup(string id)
        {
            VariableManager.Instance.AddOneItem(id, false);
        }
        
        public void ResetItem(string id)
        {
            VariableManager.Instance.ResetItem(id);
        }

        public void SetPlayerTransform(Transform tr)
        {
            PlayerController.Instance.gameObject.transform.position = tr.position;
        }

        public void ExecuteDialogue(DialogueContainer dialogueContainer)
        {
            PlayerController.Instance.Interaction.Enabled = false;
            
            PlayerController.Instance.StateMachine.ChangeState(
                new PlayerInteractionState(PlayerController.Instance, 
                    PlayerController.Instance.StateMachine, dialogueContainer,
                    () => { PlayerController.Instance.Interaction.Enabled = true; }));
        }

        public void ChangeFlowManager(QuestFlowManager newManager)
        {
            observeUpdate = false;
            newManager.gameObject.SetActive(true);
        }
        
        private IEnumerator UpdateFlowEvent(QuestFlow flow)
        {
            yield return null;
            flow.ActiveEvents?.Invoke();
        }

        private void UpdateScene()
        {
            QuestFlow currentFlow = null;
            foreach (var flow in QuestFlows)
            {
                if (flow.QuestChapterID < currentQuestChapterID ||
                    (flow.QuestChapterID == currentQuestChapterID && flow.QuestID < currentQuestID) ||
                    (flow.QuestChapterID == currentQuestChapterID && flow.QuestID == currentQuestID && flow.QuestDetailID < currentQuestDetailID) ||
                    (flow.QuestChapterID == currentQuestChapterID && flow.QuestID == currentQuestID && flow.QuestDetailID == currentQuestDetailID && flow.QuestFlowID <= currentQuestFlowID))
                {
                    currentFlow = flow;
                    foreach (var obj in flow.ActiveGameObjects)
                    {
                        obj.SetActive(true);
                    }
                    foreach (var obj in flow.InactiveGameObjects)
                    {
                        obj.SetActive(false);
                    }
                }
            }

            if (currentFlow == null)
                return;
            
            StartCoroutine(UpdateFlowEvent(currentFlow));
        }
    }
}

