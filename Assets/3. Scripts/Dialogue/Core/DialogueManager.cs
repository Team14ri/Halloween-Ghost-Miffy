using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DS.Core
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        
        public readonly Dictionary<string, DialogueHandler> Handlers = new();
        
        private Coroutine _typeRoutine;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        public DialogueHandler GetHandler(string id)
        {
            if (Handlers.TryGetValue(id, out DialogueHandler value))
                return value;
            throw new Exception($"DialogueHandler not found with ID: {id}");
        }

        public void PlayDialogue(TMP_Text textBox, string value)
        {
            StopDialogue();
            List<DialogueUtility.Command> commands = DialogueUtility.ParseCommands(value);
            DialogueAnimator.Instance.ChangeTextBox(textBox);
            _typeRoutine = StartCoroutine(DialogueAnimator.Instance.AnimateTextIn(commands));
        }
        
        public void StopDialogue()
        {
            this.EnsureCoroutineStopped(ref _typeRoutine);
            DialogueAnimator.Instance.StopCurrentAnimation();
        }
    }
}