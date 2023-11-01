using System;
using Cinemachine;
using TMPro;
using UnityEngine;

namespace DS.Core
{
    public class MultiDialogueHandler : MonoBehaviour
    {
        [SerializeField] private DialogueHandler dialogueHandler;
        [SerializeField] private MultiDialogueSetter dialogueSetter;

        public void PlayDialogue(string text, bool skipTyping = false)
        {
            dialogueSetter.nameBox.text = dialogueHandler.name;
            DialogueManager.Instance.PlayDialogue(dialogueSetter.textBox, text, skipTyping);
        }
    }
}