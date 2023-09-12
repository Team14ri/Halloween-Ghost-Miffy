using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textBox;
        [SerializeField] private Button playDialogueButton;
        
        private void Start()
        {
            playDialogueButton.onClick.AddListener(PlayDialogue1);
        }
        
        private void PlayDialogue1() {
            PlayDialogue(_textBox, "<speed:0.03><anim:shake>으아악!\n</anim><pause:0.3><speed:0.15>배.. 고... 파<pause:0.5><speed:0.05>\n너.. <pause:0.2>혹시.... <pause:0.24><anim:wave>먹을 것</anim> 좀 있어?");
        }

        private Coroutine _typeRoutine;
        
        private void PlayDialogue(TMP_Text textBox, string value)
        {
            this.EnsureCoroutineStopped(ref _typeRoutine);
            List<DialogueUtility.Command> commands = DialogueUtility.ParseCommands(value);
            DialogueAnimator.Instance.ChangeTextBox(textBox);
            _typeRoutine = StartCoroutine(DialogueAnimator.Instance.AnimateTextIn(commands));
            
            // 출력 테스트
            foreach (var command in commands)
            {
                Debug.Log($"{command.commandType}, {command.textAnimationType}, \"{command.stringValue}\", {command.floatValue}, {command.startIndex}, {command.endIndex}");
            }
        }
    }
}