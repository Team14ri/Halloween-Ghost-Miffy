using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Stack<GameObject> uiStack = new();
    
    public void EnterUI(GameObject obj)
    {
        uiStack.Push(obj);
        obj.SetActive(true);
    }
    
    public void EnterUIAlone(GameObject obj)
    {
        EscapeAllUI();
        uiStack.Push(obj);
        obj.SetActive(true);
    }
    
    public void EscapeOneUI()
    {
        if (uiStack.Count == 0)
            return;

        GameObject obj = uiStack.Pop();
        obj.SetActive(false);
    }
    
    public void EscapeAllUI()
    {
        while (uiStack.Count != 0)
        {
            GameObject obj = uiStack.Pop();
            obj.SetActive(false);
        }
    }
}
