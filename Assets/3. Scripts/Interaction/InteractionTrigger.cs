using System;
using DS.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class InteractionTrigger : MonoBehaviour
    {
        public string title = String.Empty;
        [SerializeField] private DialogueContainer dialogueContainer;
        [SerializeField] private UnityEvent events;
        
        public void Execute()
        {
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