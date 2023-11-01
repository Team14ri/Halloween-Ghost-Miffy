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
        [SerializeField] private DialogueHandler dialogueHandler;
        [SerializeField] private MultiDialogueSetter dialogueSetter;

        [SerializeField] private List<ChoiceData> choiceDataList;

        private PlayerInteractionState _interactionState;
        private List<NodeLinkData> _links;

        private bool _observeSelectChoice;
        
        private void Update()
        {
            if (!_observeSelectChoice)
                return; 
            
            if (DialogueManager.Instance.CheckDialogueEnd())
            {
                _interactionState.SelectChoice("");
            }
        }

        public void Init(PlayerInteractionState state, List<NodeLinkData> links)
        {
            _interactionState = state;
            _links = links;
            
            var oldDialogueHandler = state.lastestDialogueHandler;
            state.lastestDialogueHandler = GetComponent<DialogueHandler>();

            if (oldDialogueHandler != null)
            {
                state.currentXAxis = oldDialogueHandler.GetXAxis();
                oldDialogueHandler.DisableLookTarget();
            }
                
            state.lastestDialogueHandler.LookTarget(state.currentXAxis);
            
            choiceDataList.Clear();
            
            foreach (var link in links)
            {
                choiceDataList.Add(new ChoiceData
                {
                    text = link.PortName,
                    guid = link.TargetNodeGuid
                });
            }

            _observeSelectChoice = true;
        }

        public void PlayDialogue(string text, bool skipTyping = false)
        {
            dialogueSetter.nameBox.text = dialogueHandler.name;
            DialogueManager.Instance.PlayDialogue(dialogueSetter.textBox, text, skipTyping);
        }
    }
}