using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    private string _fileName = "New Narrative";
    
    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphView()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }
    
    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView()
        {
            name = "Dialogue Graph"
        };
        
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }
    
    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField("File Name:");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolbar.Add(fileNameTextField);
        
        toolbar.Add(new Button(SaveData)
        {
            text = "Save Data"
        });
        toolbar.Add(new Button(LoadData)
        {
            text = "Load Data"
        });
        
        toolbar.Add(new Button(() => _graphView.CreateNode("Dialogue Node"))
        {
            text = "Create Node"
        });
        
        rootVisualElement.Add(toolbar);
    }
    
    private void SaveData()
    {
        throw new NotImplementedException();
    }

    private void LoadData()
    {
        throw new NotImplementedException();
    }
}
