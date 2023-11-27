using System;
using UnityEngine;

[Serializable]
public enum InteractionButtonType
{
    Talk,
    Question,
    Find
}

public class InteractionButtonController : MonoBehaviour
{
    [SerializeField] private UIFadeController uiFadeController;
    
    [SerializeField] private GameObject talkButton;
    [SerializeField] private GameObject questionButton;
    [SerializeField] private GameObject findButton;
    
    public void Enable(InteractionButtonType type)
    {
        switch (type)
        {
            case InteractionButtonType.Talk:
                talkButton.SetActive(true);
                break;
            case InteractionButtonType.Question:
                questionButton.SetActive(true);
                break;
            case InteractionButtonType.Find:
                findButton.SetActive(true);
                break;
        }
        uiFadeController.FadeIn();
    }
    
    public void Disable()
    {
        talkButton.SetActive(false);
        questionButton.SetActive(false);
        findButton.SetActive(false);
        uiFadeController.FadeOut();
    }
}
