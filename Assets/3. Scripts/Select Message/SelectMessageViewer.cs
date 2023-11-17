using DS.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectMessageViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    private string _guid;
    
    public void Set(string text, string guid)
    {
        transform.localScale = Vector3.one;
        textField.text = text;
        _guid = guid;
    }
    
    public void Click()
    {
        MultiDialogueHandler.Instance.SelectChoice(_guid);
    }
}