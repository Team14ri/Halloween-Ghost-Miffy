using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float interactionDelay = 0.8f;
        [SerializeField] private List<InteractionTrigger> interactionTriggers = new();

        public bool Enabled { get; set; }

        private InteractionTrigger closestInteractionTrigger;
            
        private void Start()
        {
            Enabled = true;
        }

        private void Update()
        {
            if (!Enabled || interactionTriggers.Count == 0)
            {
                closestInteractionTrigger = null;
                foreach (var trigger in interactionTriggers)
                {
                    trigger.Exit();
                }
                return;
            }
            
            foreach (var trigger in interactionTriggers)
            {
                if (trigger.gameObject.activeInHierarchy == false)
                {
                    interactionTriggers.Remove(trigger);
                    trigger.Exit();
                }
            }

            var newClosestInteractionTrigger = interactionTriggers
                .Where(c => !c.DisableInteraction)
                .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
                .FirstOrDefault();

            if (newClosestInteractionTrigger != null 
                && newClosestInteractionTrigger != closestInteractionTrigger)
            {
                foreach (var trigger in interactionTriggers)
                {
                    trigger.Exit();
                }
                closestInteractionTrigger = newClosestInteractionTrigger;
                closestInteractionTrigger.Enter();
            }
            
            if (PlayerController.Instance.InteractionInput)
            {
                PlayerController.Instance.InteractionInput = false;
                if (closestInteractionTrigger != null)
                {
                    closestInteractionTrigger.Execute();
                    closestInteractionTrigger = null;
                }
            }
        }

        private IEnumerator SetInteractionEnable(float delay)
        {
            yield return new WaitForSeconds(delay);
            Enabled = true;
        }
        
        public void SetInteractionEnableAfterDelay()
        {
            StartCoroutine(SetInteractionEnable(interactionDelay));
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

            if (closestInteractionTrigger == interactionTrigger)
                closestInteractionTrigger = null;
            
            interactionTriggers.Remove(interactionTrigger);
            interactionTrigger.Exit();
        }
    }   
}
