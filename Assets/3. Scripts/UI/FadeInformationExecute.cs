using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class FadeInformationExecute : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField, TextArea(5, 10)] private string text;
    
    [BoxGroup("Cutscene"), SerializeField] private Sprite cutsceneImage;
    
    [SerializeField] private UnityEvent startUnityEvent;
    [SerializeField] private UnityEvent endUnityEvent;
    
    public void Execute()
    {
        startUnityEvent?.Invoke();
        FadeInformationManager.Instance.Show(text, delay, cutsceneImage, endUnityEvent);
    }
}
