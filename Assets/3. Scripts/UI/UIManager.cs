using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    private readonly Stack<GameObject> _uiStack = new();
    
    public void EnterUI(GameObject obj)
    {
        _uiStack.Push(obj);
        obj.SetActive(true);
    }
    
    public void EnterUIAlone(GameObject obj)
    {
        EscapeAllUI();
        _uiStack.Push(obj);
        obj.SetActive(true);
    }
    
    public void EscapeOneUI()
    {
        if (_uiStack.Count == 0)
            return;

        GameObject obj = _uiStack.Pop();
        obj.SetActive(false);

        if (_uiStack.Count != 0)
        {
            _uiStack.First().SetActive(true);
        }
    }
    
    public void EscapeAllUI()
    {
        while (_uiStack.Count != 0)
        {
            GameObject obj = _uiStack.Pop();
            obj.SetActive(false);
        }
    }
    
    public void OnESC(InputAction.CallbackContext context)
    {
        if (context.started) { }

        if (context.performed)
        {
            EscapeOneUI();
        }

        if (context.canceled) { }
    }
}
