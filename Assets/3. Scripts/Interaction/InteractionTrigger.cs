using System;
using DS.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class InteractionTrigger : MonoBehaviour
    {
        public InteractionButtonType interactionButtonType;
        [SerializeField] private InteractionButtonController interactionButtonController;
        
        [SerializeField] private DialogueContainer dialogueContainer;
        [SerializeField] private UnityEvent events;
        
        public bool DisableInteraction { get; private set; }
        
        public void Enter()
        {
            interactionButtonController.Enable(interactionButtonType);
        }
        
        public void Exit()
        {
            interactionButtonController.Disable();
        }
        
        public void Execute()
        {
            interactionButtonController.Disable();
            
            if (dialogueContainer != null)
            {
                PlayerController.Instance.StateMachine.ChangeState(
                    new PlayerInteractionState(PlayerController.Instance, 
                        PlayerController.Instance.StateMachine, dialogueContainer));
            }
            events?.Invoke();
        }
        
        private void SetInteractionEnable()
        {
            DisableInteraction = false;
        }
        
        public void DisableUntil(float seconds)
        {
            DisableInteraction = true;
            Invoke(nameof(SetInteractionEnable), seconds);
        }
    }
}