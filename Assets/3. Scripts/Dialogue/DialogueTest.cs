using DS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTest : MonoBehaviour
{
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private Button playDialogueButton;
    [SerializeField, TextArea(10, 1)] private string dialogueText;
    
    private void Start()
    {
        playDialogueButton.onClick.AddListener(PlayDialogue);
    }

    private void PlayDialogue()
    {
        DialogueManager.Instance.PlayDialogue(textBox, dialogueText);
    }
}
