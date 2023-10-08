using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private List<InteractionTrigger> interactionTriggers = new();

        public bool Enabled { get; set; }
            
        private void Start()
        {
            Enabled = true;
        }

        private void Update()
        {
            foreach (var trigger in interactionTriggers)
            {
                trigger.Exit();
            }
            
            if (!Enabled || interactionTriggers.Count == 0)
                return;

            var closestInteractionTrigger = interactionTriggers
                .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
                .FirstOrDefault();

            closestInteractionTrigger.Enter();
        }

        private void OnTriggerEnter(Collider other)
        {
            var interactionTrigger = other.GetComponent<InteractionTrigger>();
            
            if (interactionTrigger == null)
                return;
            
            interactionTriggers.Add(interactionTrigger);
        }

        private void OnTriggerExit(Collider other)
        {
            var interactionTrigger = other.GetComponent<InteractionTrigger>();
            
            if (interactionTrigger == null)
                return;
            
            interactionTriggers.Remove(interactionTrigger);
            interactionTrigger.Exit();
        }
        
        private void ExecuteInteraction()
        {
            if (interactionTriggers.Count == 0)
                return;
            
            var closestInteractionTriggers = interactionTriggers
                .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
                .FirstOrDefault();

            closestInteractionTriggers.Execute();
        }
    }   
}
