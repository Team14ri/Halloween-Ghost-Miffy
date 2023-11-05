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
        
        [SerializeField] private DialogueSetter dialogueSetter;

        [SerializeField] private CinemachineFreeLook cinemachineFreeLook;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            DialogueManager.Instance.Handlers[ID] = this;
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

        public string GetName()
        {
            return Name;
        }

        public void SetAnimation(string id)
        {
            if (_animator == null)
                return;
            
            if (!_animator.HasState(0, Animator.StringToHash(id)))
            {
                Debug.LogWarning($"{id} 애니메이션이 존재하지 않습니다.");
                return;
            }
            
            _animator.CrossFade(id, 0f);
        }

        public void PlayDialogue(string text, bool skipTyping = false)
        {
            dialogueSetter.nameBox.text = Name;
            DialogueManager.Instance.PlayDialogue(dialogueSetter.textBox, text, skipTyping);
        }
    }
}

