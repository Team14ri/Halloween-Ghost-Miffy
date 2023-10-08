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
                PlayerController.Instance.stateMachine.ChangeState(
                    new PlayerInteractionState(PlayerController.Instance, 
                        PlayerController.Instance.stateMachine, dialogueContainer));
            }
            events?.Invoke();
        }
    }
}