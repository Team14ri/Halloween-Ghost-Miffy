using UnityEngine;

public class LocationPopupManager : MonoBehaviour
{
    [SerializeField] private float showDelay;
    
    [SerializeField] private string locationType = "WELCOME TO";
    [SerializeField] private string locationName;
    
    [SerializeField] private GameObject locationUI;
    
    private TmpTextEditor _locationTmpTextEditor;
    private UIFadeController _locationFadeController;
    
    private void Start()
    {
        _locationTmpTextEditor = locationUI.GetComponent<TmpTextEditor>();
        _locationFadeController = locationUI.GetComponent<UIFadeController>();
        
        Invoke(nameof(Show), showDelay);
    }
        
    public void Show()
    {
        _locationTmpTextEditor.Edit("Location Type", locationType)
            .Edit("Location Title", locationName);
        _locationFadeController.AutoFadeInAndOut();
    }
}
