using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Quest
{
    [Serializable]
    public class QuestFlow
    {
        public string QuestName;
        
        [Space(10)]
        public QuestLocation QuestLocationID; 
        public int QuestID; 
        public int QuestDetailID; 
        public int QuestFlowID; 
        
        [Space(10)]
        public List<GameObject> ActiveGameObjects;
        
        [Space(10)]
        public List<GameObject> InactiveGameObjects;
        
        [Space(10)]
        public UnityEvent ActiveEvents;
    }
    
    public class QuestFlowManager : MonoBehaviour
    {
        [SerializeField] private List<QuestFlow> QuestFlows;

        public static QuestFlowManager Instance;

        public int changeQuestDetailID; 
        
        private QuestLocation currentQuestLocation;
        private int currentQuestID; 
        private int currentQuestDetailID; 
        private int currentQuestFlowID; 
        
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
            var questInfo = QuestManager.Instance.CurrentQuestInfo;
            currentQuestLocation = (QuestLocation)questInfo[0];
            currentQuestID = questInfo[1];
            currentQuestDetailID = questInfo[2];
            changeQuestDetailID = questInfo[2];
            currentQuestFlowID = questInfo[3];
            
            UpdateScene();
        }

        private void Update()
        {
            var questInfo = QuestManager.Instance.CurrentQuestInfo;
            if (currentQuestLocation != (QuestLocation)questInfo[0] || 
                currentQuestID != questInfo[1] || 
                currentQuestDetailID != changeQuestDetailID || 
                currentQuestFlowID != questInfo[3])
            {
                currentQuestLocation = (QuestLocation)questInfo[0];
                currentQuestID = questInfo[1];
                currentQuestDetailID = changeQuestDetailID;
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

