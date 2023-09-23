using DS;
using TMPro;
using UnityEngine;

namespace DS
{
    public class DialogueHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text textBox;
        
        public void PlayDialogue(string text)
        {
            DialogueManager.Instance.PlayDialogue(textBox, text);
        }
    }
}

