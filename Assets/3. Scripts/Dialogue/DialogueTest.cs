using DS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTest : MonoBehaviour
{
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private Button playDialogueButton;
    [SerializeField] public string dialogueTitle;
    [SerializeField, TextArea(10, 1)] private string dialogueText;

    public void EnterArea()
    {
        playDialogueButton.gameObject.SetActive(true);
        playDialogueButton.GetComponentInChildren<TMP_Text>().text = dialogueTitle;
        playDialogueButton.onClick.RemoveAllListeners();
        playDialogueButton.onClick.AddListener(PlayDialogue);
    }
    
    public void ExitArea()
    {
        playDialogueButton.gameObject.SetActive(false);
        DialogueManager.Instance.StopDialogue();
    }

    public void PlayDialogue()
    {
        playDialogueButton.gameObject.SetActive(false);
        DialogueManager.Instance.PlayDialogue(textBox, dialogueText);
    }
}
