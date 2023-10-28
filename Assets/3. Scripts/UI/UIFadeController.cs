using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIFadeControllerMode
{
    None,
    AutoFadeInAndOut,
    FadeIn,
    FadeOut
}

public class UIFadeController : MonoBehaviour
{
    [SerializeField] private UIFadeControllerMode modeOnEnable = UIFadeControllerMode.None;
    
    [SerializeField] private List<GameObject> activeTarget;

    [SerializeField] private float autoFadeWaitTime = 4f;
    
    [SerializeField] private float fadeInTime = 1.4f;
    [SerializeField] private AnimationCurve FadeInAccelerationCurve = new(new Keyframe(0, 0), new Keyframe(1, 1));
    
    [SerializeField] private float fadeOutTime = 2.2f;
    [SerializeField] private AnimationCurve FadeOutAccelerationCurve = new(new Keyframe(0, 1), new Keyframe(1, 0));

    [SerializeField] private List<Image> images;
    [SerializeField] private List<TMP_Text> texts;
    
    private Coroutine _fadeRoutine;

    private void OnEnable()
    {
        switch (modeOnEnable)
        {
            case UIFadeControllerMode.None:
                break;
            case UIFadeControllerMode.AutoFadeInAndOut:
                AutoFadeInAndOut();
                break;
            case UIFadeControllerMode.FadeIn:
                FadeIn();
                break;
            case UIFadeControllerMode.FadeOut:
                FadeOut();
                break;
        }
    }

    public void AutoFadeInAndOut()
    {
        FadeIn();

        Invoke(nameof(FadeOut), fadeInTime + autoFadeWaitTime);
    }

    public void FadeIn()
    {
        SetAlpha(images, 0f);
        SetAlpha(texts, 0f);
        
        this.EnsureCoroutineStopped(ref _fadeRoutine);
        _fadeRoutine = StartCoroutine(FadeInProcess());
    }

    public void FadeOut()
    {
        this.EnsureCoroutineStopped(ref _fadeRoutine);
        _fadeRoutine = StartCoroutine(FadeOutProcess());
    }

    private IEnumerator FadeInProcess()
    {
        foreach (var obj in activeTarget)
        {
            obj.SetActive(true);
        }
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            float alpha = FadeInAccelerationCurve.Evaluate(Mathf.Min(t + (Time.deltaTime / fadeInTime), 1f));
            SetAlpha(images, alpha);
            SetAlpha(texts, alpha);
            yield return null;
        }
        
        SetAlpha(images, 1);
        SetAlpha(texts, 1);
    }

    private IEnumerator FadeOutProcess()
    {
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            float alpha = FadeOutAccelerationCurve.Evaluate(Mathf.Min(t + (Time.deltaTime / fadeOutTime), 1f));
            SetAlpha(images, alpha);
            SetAlpha(texts, alpha);
            yield return null;
        }
        
        SetAlpha(images, 0);
        SetAlpha(texts, 0);
        
        foreach (var obj in activeTarget)
        {
            obj.SetActive(false);
        }
    }

    private void SetAlpha<T>(List<T> objList, float alpha) where T : Component
    {
        foreach (var obj in objList)
        {
            Color color;
            if (obj is Image imageObj)
            {
                color = imageObj.color;
                color.a = alpha;
                imageObj.color = color;
            }
            else if (obj is TMP_Text textObj)
            {
                color = textObj.color;
                color.a = alpha;
                textObj.color = color;
            }
        }
    }
}
