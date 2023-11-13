using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UICallbackContext : MonoBehaviour
{
    [SerializeField] private GameObject target;
    
    public void EnterUI(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UIManager.Instance.EnterUI(target);
        }
    }
    
    public void EnterUIAlone(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UIManager.Instance.EnterUIAlone(target);
        }
    }
}
