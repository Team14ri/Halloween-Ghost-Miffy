using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionController : MonoBehaviour
{
    [SerializeField] private bool initOpenOnEnable;

    [SerializeField] private RectTransform transitionUI;

    [SerializeField] private float irisMaxValue = 2200f;
    
    [SerializeField] private float irisOpenTime = 1f;
    [SerializeField] private AnimationCurve IrisOpenAccelerationCurve = new(new Keyframe(0, 0), new Keyframe(1, 1));
    
    [SerializeField] private float irisCloseTime = 1f;
    [SerializeField] private AnimationCurve IrisCloseAccelerationCurve = new(new Keyframe(0, 0), new Keyframe(1, 1));
    
    public static SceneTransitionController Instance;
    
    private Coroutine _transitionRoutine;
    
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
        StartCoroutine(StartOneFrameLate());
    }

    private IEnumerator StartOneFrameLate()
    {
        yield return null;
        IrisOpen(null);
    }

    private void OnEnable()
    {
        if (transitionUI == null || initOpenOnEnable)
            return;
            
        transitionUI.sizeDelta = new Vector2(0f, 0f);
    }

    public void IrisOpen(Action action)
    {
        if (transitionUI == null)
            return;
        
        this.EnsureCoroutineStopped(ref _transitionRoutine);
        _transitionRoutine = StartCoroutine(IrisOpenProcess(action));
    }
    
    public void IrisClose(Action action)
    {
        if (transitionUI == null)
            return;
        
        this.EnsureCoroutineStopped(ref _transitionRoutine);
        _transitionRoutine = StartCoroutine(IrisCloseProcess(action));

        if (BGMPlayer.Instance == null)
            return;
        
        BGMPlayer.Instance.StopBGM();
    }
    
    private IEnumerator IrisOpenProcess(Action action)
    {
        float elapsedTime = 0;

        while (elapsedTime < irisOpenTime)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / irisOpenTime;

            float curveValue = IrisOpenAccelerationCurve.Evaluate(percentage);
            float irisValue = Mathf.Lerp(0, irisMaxValue, curveValue);
            transitionUI.sizeDelta = new Vector2(irisValue, irisValue);
            yield return null;
        }
        transitionUI.sizeDelta = new Vector2(irisMaxValue, irisMaxValue);
        action?.Invoke();
    }
    
    private IEnumerator IrisCloseProcess(Action action)
    {
        float elapsedTime = 0;

        while (elapsedTime < irisCloseTime)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / irisCloseTime;

            float curveValue = IrisCloseAccelerationCurve.Evaluate(percentage);
            float irisValue = Mathf.Lerp(irisMaxValue, 0, curveValue);
            transitionUI.sizeDelta = new Vector2(irisValue, irisValue);
            yield return null;
        }
        transitionUI.sizeDelta = new Vector2(0, 0);
        action?.Invoke();
    }
}
