using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DS
{
    public class DialogueAnimator
    {
        private TMP_Text _textBox;
        private List<DialogueUtility.Command> _commands;
        
        public void ChangeTextBox(TMP_Text textBox)
        {
            _textBox = textBox;
        }

        public IEnumerator AnimateTextIn(List<DialogueUtility.Command> commands)
        {
            _textBox.text = "";
            
            _commands = commands;
            float currentTextSpeed = DialogueUtility.TextAnimationSpeed["normal"];

            foreach (var command in _commands)
            {
                switch (command.commandType)
                {
                    case DialogueUtility.CommandType.Pause:
                        yield return new WaitForSeconds(command.floatValue);
                        break;
                    case DialogueUtility.CommandType.TextSpeedChange:
                        currentTextSpeed = command.floatValue;
                        break;
                    default:
                        foreach (var t in command.stringValue)
                        {
                            _textBox.text += t;
                            yield return new WaitForSeconds(currentTextSpeed);
                        }
                        break;
                }
            }
            
            yield return null;
        }
    }
}
