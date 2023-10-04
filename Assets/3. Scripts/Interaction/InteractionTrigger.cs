using System;
using DS.Core;
using DS.Runtime;
using UnityEngine;

namespace Interaction
{
    public class InteractionTrigger : MonoBehaviour
    {
        public string title = String.Empty;
        [SerializeField] private DialogueHandler dialogueHandler;
        [SerializeField] private DialogueContainer dialogueContainer;
        
        public void Execute()
        {
            // Debug.Log("Execute");
            // PlayerController.Instance.StopMovementInput = !PlayerController.Instance.StopMovementInput;
            PlayerController.Instance.stateMachine.ChangeState(
                new PlayerInteractionState(PlayerController.Instance, PlayerController.Instance.stateMachine, 
                    dialogueHandler, dialogueContainer));
        }
    }
}