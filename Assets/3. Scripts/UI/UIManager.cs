using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public PlayerInput PlayerInput { get; private set; }

    [SerializeField] private GameObject escMenu;
    
    private readonly Stack<GameObject> _uiStack = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        PlayerInput = GetComponent<PlayerInput>();
    }

    private bool CheckAlreadyOpen(GameObject obj)
    {
        var topObj =  _uiStack.FirstOrDefault();

        if (topObj == null)
            return false;
        
        return topObj == obj && topObj.activeInHierarchy;
    }

    public void EnterUI(GameObject obj)
    {
        if (CheckAlreadyOpen(obj))
        {
            EscapeOneUI();
            return;
        }
        _uiStack.Last().SetActive(false);
        _uiStack.Push(obj);
        obj.SetActive(true);
        SoundManager.Instance.PlaySound("UI_QuestOpen");
    }
    
    public void EnterUIAlone(GameObject obj)
    {
        if (CheckAlreadyOpen(obj))
        {
            EscapeAllUI();
            return;
        }
        EscapeAllUI();
        _uiStack.Push(obj);
        obj.SetActive(true);
        SoundManager.Instance.PlaySound("UI_QuestOpen");
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
            if (_uiStack.Count == 0)
            {
                EnterUIAlone(escMenu);
                return;
            }
            EscapeOneUI();
            SoundManager.Instance.PlaySound("UI_Button_Click");
        }

        if (context.canceled) { }
    }
}
