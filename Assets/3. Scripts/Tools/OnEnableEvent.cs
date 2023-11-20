using System;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnableEvent;
    [SerializeField] private UnityEvent onExitEvent;

    [SerializeField] private float exitTime;

    private void OnEnable()
    {
        onEnableEvent?.Invoke();
        CancelInvoke(nameof(OnExit));
        Invoke(nameof(OnExit), exitTime);
    }

    private void OnExit()
    {
        onExitEvent?.Invoke();
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(OnExit));
    }
}
