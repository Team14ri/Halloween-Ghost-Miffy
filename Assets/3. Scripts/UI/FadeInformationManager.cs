using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeInformationManager : MonoBehaviour
{
    [SerializeField] private GameObject informationUI;
    
    [BoxGroup("Cutscene"), SerializeField] private Image cutsceneField;
    [BoxGroup("Cutscene"), SerializeField] private Sprite defaultImage;

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

        cutsceneField.sprite = defaultImage;
    }
    
    private void ExecuteEvent()
    {
        PlayerController.Instance.PlayerInput.enabled = true;
        _unityEvent?.Invoke();
    }
        
    public void Show(string text, float delay, Sprite cutsceneImage, UnityEvent unityEvent)
    {
        PlayerController.Instance.PlayerInput.enabled = false;
        
        _unityEvent = unityEvent;

        Invoke(nameof(ExecuteEvent), _fadeInformationController.GetFadeInTime() + delay);
        
        cutsceneField.sprite = cutsceneImage ? cutsceneImage : defaultImage;
        
        _fadeInformationController.SetAutoFadeWaitTime(delay);
        _fadeInformationTmpTextEditor.Edit("Text", text);
        _fadeInformationController.AutoFadeInAndOut();
    }
}
