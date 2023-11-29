using System;
using DS.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class InteractionTrigger : MonoBehaviour
    {
        public bool disableInteraction;
        
        public InteractionButtonType interactionButtonType;
        [SerializeField] private InteractionButtonController interactionButtonController;
        
        [SerializeField] private DialogueContainer dialogueContainer;
        [SerializeField] private UnityEvent enterEvents;
        [SerializeField] private UnityEvent exitEvents;

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
            
            enterEvents?.Invoke();
            
            if (dialogueContainer != null)
            {
                PlayerController.Instance.StateMachine.ChangeState(
                    new PlayerInteractionState(PlayerController.Instance, 
                        PlayerController.Instance.StateMachine, dialogueContainer,
                        () => { exitEvents?.Invoke(); }));
            }
        }
        
        public void SetInteractionButtonType(string type)
        {
            interactionButtonType = Enum.Parse<InteractionButtonType>(type, true);
        }
        
        private void SetInteractionEnable()
        {
            disableInteraction = false;
        }
        
        public void DisableUntil(float seconds)
        {
            disableInteraction = true;
            Invoke(nameof(SetInteractionEnable), seconds);
        }
    }
}