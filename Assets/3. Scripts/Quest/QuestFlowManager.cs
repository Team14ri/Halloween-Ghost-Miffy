using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Quest
{
    [Serializable]
    public enum QuestClearDataType
    {
        SaveDataCondition
    }
    
    [Serializable]
    public class QuestClearData
    {
        public QuestClearDataType type;
    }
    
    [Serializable]
    public class QuestFlow
    {
        [TabGroup("Settings", "Quest Info")]
        public string QuestName;
        [TabGroup("Settings", "Quest Info"), Space(5)]
        public QuestLocation QuestLocationID; 
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
        public QuestLocation NextQuestLocationID;
        [TabGroup("Settings", "Auto Clear Quest"), ShowIf("enableAutoQuestClear"), Space(5)]
        public int NextQuestID; 
        [TabGroup("Settings", "Auto Clear Quest"), ShowIf("enableAutoQuestClear"), Space(5)]
        public int NextQuestDetailID; 
        [TabGroup("Settings", "Auto Clear Quest"), ShowIf("enableAutoQuestClear"), Space(5)]
        public int NextQuestFlowID;
        [TabGroup("Settings", "Auto Clear Quest"), ShowIf("enableAutoQuestClear"), Space(10)]
        public List<QuestClearData> QuestClearData;
    }
    
    public class QuestFlowManager : MonoBehaviour
    {
        [SerializeField] private List<QuestFlow> QuestFlows;

        public static QuestFlowManager Instance;

        private QuestLocation currentQuestLocation;
        private int currentQuestID; 
        private int currentQuestDetailID; 
        private int currentQuestFlowID; 
        
        private void Awake()
        {
            // TODO: 삭제하기
            // PlayerPrefs.DeleteAll();
            
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
            var questInfo = QuestManager.Instance.CurrentQuestInfo;
            currentQuestLocation = (QuestLocation)questInfo[0];
            currentQuestID = questInfo[1];
            currentQuestDetailID = questInfo[2];
            currentQuestFlowID = questInfo[3];

            UpdateScene();
        }

        private void Update()
        {
            var questInfo = QuestManager.Instance.CurrentQuestInfo;
            if (currentQuestLocation != (QuestLocation)questInfo[0] || 
                currentQuestID != questInfo[1] || 
                currentQuestDetailID != questInfo[2] || 
                currentQuestFlowID != questInfo[3])
            {
                currentQuestLocation = (QuestLocation)questInfo[0];
                currentQuestID = questInfo[1];
                currentQuestDetailID = questInfo[2];
                currentQuestFlowID = questInfo[3];
                
                UpdateScene();
            }
        }

        private void UpdateScene()
        {
            QuestFlow currentFlow = null;
            foreach (var flow in QuestFlows)
            {
                if (flow.QuestLocationID < currentQuestLocation ||
                    (flow.QuestLocationID == currentQuestLocation && flow.QuestID < currentQuestID) ||
                    (flow.QuestLocationID == currentQuestLocation && flow.QuestID == currentQuestID && flow.QuestDetailID < currentQuestDetailID) ||
                    (flow.QuestLocationID == currentQuestLocation && flow.QuestID == currentQuestID && flow.QuestDetailID == currentQuestDetailID && flow.QuestFlowID <= currentQuestFlowID))
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
            
            currentFlow.ActiveEvents?.Invoke();
        }
    }
}

