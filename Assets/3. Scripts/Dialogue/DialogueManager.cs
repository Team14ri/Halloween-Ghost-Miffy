using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DS
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        
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

        public void PlayDialogue(TMP_Text textBox, string value)
        {
            StopDialogue();
            List<DialogueUtility.Command> commands = DialogueUtility.ParseCommands(value);
            DialogueAnimator.Instance.ChangeTextBox(textBox);
            _typeRoutine = StartCoroutine(DialogueAnimator.Instance.AnimateTextIn(commands));
            
            // 출력 테스트
            // foreach (var command in commands)
            // {
            //     Debug.Log($"{command.commandType}, {command.textAnimationType}, \"{command.stringValue}\", {command.floatValue}, {command.startIndex}, {command.endIndex}");
            // }
        }
        
        public void StopDialogue()
        {
            this.EnsureCoroutineStopped(ref _typeRoutine);
            DialogueAnimator.Instance.StopCurrentAnimation();
        }
    }
}