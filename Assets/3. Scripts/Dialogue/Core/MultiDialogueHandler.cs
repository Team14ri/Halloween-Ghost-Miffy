using System;
using System.Collections.Generic;
using DS.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

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
        [SerializeField] private MultiDialogueSetter dialogueSetter;

        private int _currentIndex;
        [SerializeField] private List<ChoiceData> choiceDataList = new();
        
        private Action<string> _returnAction;

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

        public void OnChangeSelect(InputAction.CallbackContext context)
        {
            if (dialogueSetter.textBox.text == "")
                return;
            
            if (context.started)
            {
                var input = context.ReadValue<Vector2>();
                if (input.x >= 0)
                {
                    UpdateIndex(1);
                }
                else
                {
                    UpdateIndex(-1);
                }
            }
        }
        
        public void OnInteraction(InputAction.CallbackContext context)
        {
            if (dialogueSetter.textBox.text == "")
                return;
            
            if (context.started)
            {
                SelectChoice();
            }
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

        public void Init(List<NodeLinkData> links, Action<string> returnAction)
        {
            _currentIndex = 0;
            
            _returnAction = returnAction;

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

            _observeDialogueEnd = true;
        }
        
        private void Exit()
        {
            dialogueSetter.textBox.text = "";
        }
        
        private void InitIndex()
        {
            UpdateIndex(0);
        }
        
        public void UpdateIndex(int updateIdx)
        {
            if (choiceDataList.Count == 0)
                return;
            
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
            dialogueSetter.textBox.text = choiceDataList[_currentIndex].text;
        }
        
        public void SelectChoice()
        {
            _returnAction?.Invoke(choiceDataList[_currentIndex].guid);
            Exit();
        }

        public (int, int) GetInfo()
        {
            return (choiceDataList.Count, _currentIndex);
        }
    }
}