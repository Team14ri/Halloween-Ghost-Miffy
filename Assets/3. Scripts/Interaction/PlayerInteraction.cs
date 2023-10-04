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
        [SerializeField] private Button playInteractionButton;
        private TMP_Text playInteractionText;
        [SerializeField] private List<InteractionTrigger> interactionTriggers = new();

        private void Start()
        {
            playInteractionButton.gameObject.SetActive(false);
            playInteractionButton.onClick.AddListener(ExecuteInteraction);
            playInteractionText = playInteractionButton.GetComponentInChildren<TMP_Text>();
        }

        private void Update()
        {
            if (interactionTriggers.Count == 0)
            {
                playInteractionButton.gameObject.SetActive(false);
                return;
            }
            
            var closestInteractionTrigger = interactionTriggers
                .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
                .FirstOrDefault();
            
            playInteractionButton.gameObject.SetActive(true);
            playInteractionText.text = closestInteractionTrigger.title;
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
