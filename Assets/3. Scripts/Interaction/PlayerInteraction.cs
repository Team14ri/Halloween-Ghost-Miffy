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

        private bool _enabled;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (value == false)
                {
                    this.EnsureCoroutineStopped(ref _eventRoutine);
                }
                _enabled = value;
            }
        }

        private InteractionTrigger closestInteractionTrigger;
            
        private Coroutine _eventRoutine;
        
        private void Start()
        {
            Enabled = true;
        }

        private void Update()
        {
            for (int i = interactionTriggers.Count - 1; i >= 0; i--)
            {
                var trigger = interactionTriggers[i];
                if (!trigger.gameObject.activeInHierarchy)
                {
                    interactionTriggers.RemoveAt(i);
                    trigger.Exit();
                }
            }
            
            if (!Enabled || interactionTriggers.Count == 0)
            {
                closestInteractionTrigger = null;
                foreach (var trigger in interactionTriggers)
                {
                    trigger.Exit();
                }
                return;
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

        public void SetInteractionEnable()
        {
            Enabled = true;
        }

        private IEnumerator _SetInteractionEnableAfterDelay()
        {
            yield return new WaitForSeconds(interactionDelay);

            SetInteractionEnable(); 
        }
        
        public void SetInteractionEnableAfterDelay()
        {
            _eventRoutine = StartCoroutine(_SetInteractionEnableAfterDelay());
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
