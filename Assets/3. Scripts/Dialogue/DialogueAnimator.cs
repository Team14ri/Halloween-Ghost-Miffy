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
            _commands = commands;
            yield return null;
        }
    }
}
