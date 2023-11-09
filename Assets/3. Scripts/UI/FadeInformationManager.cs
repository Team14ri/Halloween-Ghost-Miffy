using UnityEngine;
using UnityEngine.Events;

public class FadeInformationManager : MonoBehaviour
{
    [SerializeField] private GameObject informationUI;
    
    private TmpTextEditor _fadeInformationTmpTextEditor;
    private UIFadeController _fadeInformationController;

    private UnityEvent _unityEvent;
    
    public static FadeInformationManager Instance;
        
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
        _fadeInformationTmpTextEditor = informationUI.GetComponent<TmpTextEditor>();
        _fadeInformationController = informationUI.GetComponent<UIFadeController>();
    }
    
    private void ExecuteEvent()
    {
        _unityEvent?.Invoke();
    }
        
    public void Show(string text, float delay, UnityEvent unityEvent)
    {
        _unityEvent = unityEvent;
        
        Invoke(nameof(ExecuteEvent), _fadeInformationController.GetFadeInTime() + delay);
        
        _fadeInformationController.SetAutoFadeWaitTime(delay);
        _fadeInformationTmpTextEditor.Edit("Text", text);
        _fadeInformationController.AutoFadeInAndOut();
    }
}
