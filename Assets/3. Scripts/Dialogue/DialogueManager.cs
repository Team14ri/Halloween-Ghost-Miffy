using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class DialogueManager : MonoBehaviour
    {
        private Button _playDialogueButton;
        
        private void Start()
        {
            _playDialogueButton.onClick.AddListener(PlayDialogue);
        }

        private void PlayDialogue()
        {
            string input = "테스트 테스트 <speed:5>속도 증가 <pause:1><anim:wave>애니메이션</anim><speed:1>속도 보통 <pause:3>3초 멈춤";
            List<DialogueUtility.Command> commands = DialogueUtility.ParseCommands(input);

            // 출력 테스트
            foreach (var command in commands)
            {
                Debug.Log($"{command.commandType}, {command.textAnimationType}, \"{command.stringValue}\", {command.floatValue}");
            }
        }
    }
}