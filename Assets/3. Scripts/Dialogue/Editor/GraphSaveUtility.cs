using UnityEngine;

public class GraphSaveUtility
{
    private DialogueGraphView _targetGraphView;
    
    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {
        
    }

    public void LoadGraph(string fileName)
    {
        
    }
}
