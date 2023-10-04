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
            Debug.Log("Execute");
        }
    }
}