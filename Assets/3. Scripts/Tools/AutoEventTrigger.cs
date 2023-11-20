using System;
using UnityEngine;
using UnityEngine.Events;

public class AutoEventTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnterEvent;
    [SerializeField] private UnityEvent onExitEvent;

    [SerializeField] private float exitTime;

    public void Enter()
    {
        onEnterEvent?.Invoke();
        CancelInvoke(nameof(Exit));
        Invoke(nameof(Exit), exitTime);
    }

    private void OnEnable()
    {
        CancelInvoke(nameof(Exit));
    }

    private void Exit()
    {
        onExitEvent?.Invoke();
    }
}
