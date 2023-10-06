using System;
using UnityEngine;

namespace Quest
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance;

        [SerializeField] private GameObject questAcceptUI;
        private TmpTextEditor _questAcceptTmpTextEditor;
        private UIFadeController _questAcceptFadeController;
        
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