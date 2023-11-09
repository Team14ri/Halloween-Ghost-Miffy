using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeInformationExecute : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private string text;
    
    [SerializeField] private UnityEvent startUnityEvent;
    [SerializeField] private UnityEvent endUnityEvent;
    
    public void Execute()
    {
        startUnityEvent?.Invoke();
        FadeInformationManager.Instance.Show(text, delay, endUnityEvent);
    }
}
