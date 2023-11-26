using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerJumpQuest : MonoBehaviour
{
    [SerializeField] private Image gauge;
    
    private float _increaseValue; 
    private float _decreaseValue; 
    private float _currentGauge; 
    private float _targetGauge;
    
    private Action _doneAction;
    
    private bool _executeDone;

    public void Init(float increase, float decrease, float targetValue, Action doneAction)
    {
        PlayerController.Instance.PlayerInput.enabled = false;
        gauge.fillAmount = 0f;
        
        _increaseValue = increase;
        _decreaseValue = decrease;
        
        _currentGauge = 0f;
        _targetGauge = targetValue;
        _doneAction = doneAction;

        _executeDone = false;
    }

    private void FixedUpdate()
    {
        if (_executeDone)
            return;

        gauge.fillAmount = Mathf.Clamp(_currentGauge / _targetGauge, 0f, 1f);
        
        if (_currentGauge >= _targetGauge)
        {
            _executeDone = true;
            _doneAction?.Invoke();
            gameObject.SetActive(false);
            return;
        }
        
        _currentGauge = Mathf.Clamp(_currentGauge - _decreaseValue, 0f, _targetGauge);
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _currentGauge = Mathf.Clamp(_currentGauge + _increaseValue, 0f, _targetGauge);
        }
    }
}
