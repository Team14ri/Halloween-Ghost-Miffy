using System;
using DS.Runtime;
using UnityEngine;

namespace Interaction
{
    public class InteractionTrigger : MonoBehaviour
    {
        public string title = String.Empty;
        [SerializeField] private DialogueContainer dialogueContainer;
        
        public void Execute()
        {
            PlayerController.Instance.stateMachine.ChangeState(
                new PlayerInteractionState(PlayerController.Instance, 
                    PlayerController.Instance.stateMachine, dialogueContainer));
        }
    }
}