using System;
using System.Collections.Generic;
using DS.Runtime;
using UnityEngine;

namespace DS.Core
{
    [Serializable]
    public struct ChoiceData
    {
        public string text;
        public string guid;
    }
    
    public class MultiDialogueHandler : MonoBehaviour
    {
        private DialogueHandler _dialogueHandler;
        [SerializeField] private MultiDialogueSetter dialogueSetter;

        [SerializeField] private int _currentIndex;
        [SerializeField] private List<ChoiceData> choiceDataList;

        private PlayerInteractionState _interactionState;
        private List<NodeLinkData> _links;

        private bool _observeDialogueEnd;
        
        public static MultiDialogueHandler Instance;
        
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
            _dialogueHandler = PlayerController.Instance.GetComponent<DialogueHandler>();
        }

        private void Update()
        {
            if (!_observeDialogueEnd)
                return; 
            
            if (DialogueManager.Instance.CheckDialogueEnd())
            {
                _observeDialogueEnd = false;
                Invoke(nameof(InitIndex), 1f);
            }
        }

        public void Init(PlayerInteractionState state, List<NodeLinkData> links)
        {
            _currentIndex = 0;
            
            _interactionState = state;
            _links = links;

            choiceDataList.Clear();
            
            foreach (var link in links)
            {
                choiceDataList.Add(new ChoiceData
                {
                    text = link.PortName,
                    guid = link.TargetNodeGuid
                });
            }
            
            dialogueSetter.leftButton.interactable = true;
            dialogueSetter.rightButton.interactable = true;
            dialogueSetter.selectButton.interactable = true;

            _observeDialogueEnd = true;
        }
        
        private void Exit()
        {
            dialogueSetter.nameBox.text = "";
            dialogueSetter.textBox.text = "";
        }
        
        private void InitIndex()
        {
            UpdateIndex(0);
        }
        
        public void UpdateIndex(int updateIdx)
        {
            _currentIndex += updateIdx;
            _currentIndex = Mathf.Clamp(_currentIndex, 0, choiceDataList.Count - 1);

            dialogueSetter.leftButton.interactable = true;
            dialogueSetter.rightButton.interactable = true;

            if (_currentIndex == 0)
            {
                dialogueSetter.leftButton.interactable = false;
            }
            
            if (_currentIndex == choiceDataList.Count - 1)
            {
                dialogueSetter.rightButton.interactable = false;
            }
            
            ShowMultiDialogue();
        }

        private void ShowMultiDialogue()
        {
            dialogueSetter.nameBox.text = _dialogueHandler.GetName();
            dialogueSetter.textBox.text = choiceDataList[_currentIndex].text;
        }
        
        public void SelectChoice()
        {
            dialogueSetter.selectButton.interactable = false;
            _interactionState.SelectChoice(choiceDataList[_currentIndex].guid);
            Exit();
        }
    }
}