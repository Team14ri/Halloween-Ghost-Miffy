using System;
using UnityEngine;

namespace Quest
{
    [Serializable]
    public enum QuestLocation
    {
        Cemetery = 0,
        Plaza = 1,
        Valley = 2,
        Forest = 3
    }
    
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance;

        [SerializeField] private GameObject questAcceptUI;
        private TmpTextEditor _questAcceptTmpTextEditor;
        private UIFadeController _questAcceptFadeController;
        
        public int[] CurrentQuestInfo
        {
            get => new int[]
            {
                PlayerPrefs.GetInt("CurrentQuest@LocationID", (int)QuestLocation.Plaza),
                PlayerPrefs.GetInt("CurrentQuest@ID", 1),
                PlayerPrefs.GetInt("CurrentQuest@DetailID", 1),
                PlayerPrefs.GetInt("CurrentQuest@FlowID", 1)
            };  
            set
            {
                PlayerPrefs.GetInt("CurrentQuest@LocationID", value[0] );
                PlayerPrefs.GetInt("CurrentQuest@ID", value[1]);
                PlayerPrefs.GetInt("CurrentQuest@DetailID", value[2]);
                PlayerPrefs.GetInt("CurrentQuest@FlowID", value[3]);
            }
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
    }
}