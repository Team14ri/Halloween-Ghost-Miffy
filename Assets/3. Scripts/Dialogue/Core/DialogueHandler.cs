using System;
using Cinemachine;
using TMPro;
using UnityEngine;

namespace DS.Core
{
    public class DialogueHandler : MonoBehaviour
    {
        [SerializeField] private string ID;
        [SerializeField] private string Name;
        
        [SerializeField] private TMP_Text nameBox;
        [SerializeField] private TMP_Text textBox;

        [SerializeField] private CinemachineFreeLook cinemachineFreeLook;

        private void Start()
        {
            DialogueManager.Instance.Handlers.TryAdd(ID, this);
        }
        
        public void LookTarget(float xAxis)
        {
            if (cinemachineFreeLook == null)
                return;
            
            cinemachineFreeLook.m_XAxis.Value = xAxis;
            cinemachineFreeLook.Priority = 11;
        }
        
        public void DisableLookTarget()
        {
            if (cinemachineFreeLook == null)
                return;
            cinemachineFreeLook.Priority = 9;
        }
        
        public float GetXAxis()
        {
            if (cinemachineFreeLook == null)
                return 0f;
            return cinemachineFreeLook.m_XAxis.Value;
        }

        public void PlayDialogue(string text, bool skipTyping = false)
        {
            nameBox.text = Name;
            DialogueManager.Instance.PlayDialogue(textBox, text, skipTyping);
        }
    }
}

