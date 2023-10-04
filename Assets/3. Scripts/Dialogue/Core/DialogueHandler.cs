using System;
using TMPro;
using UnityEngine;

namespace DS.Core
{
    public class DialogueHandler : MonoBehaviour
    {
        [SerializeField] private string ID;
        
        [SerializeField] private TMP_Text textBox;

        private void Start()
        {
            DialogueManager.Instance.Handlers.TryAdd(ID, this);
        }

        public void PlayDialogue(string text, bool skipTyping = false)
        {
            DialogueManager.Instance.PlayDialogue(textBox, text, skipTyping);
        }
    }
}

