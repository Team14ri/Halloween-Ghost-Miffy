using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeController : MonoBehaviour
{
    [SerializeField] private GameObject activeTarget;

    [SerializeField] private float autoFadeWaitTime = 4f;
    
    [SerializeField] private float fadeInTime = 1.4f;
    [SerializeField] private AnimationCurve FadeInAccelerationCurve = new(new Keyframe(0, 0), new Keyframe(1, 1));
    
    [SerializeField] private float fadeOutTime = 2.2f;
    [SerializeField] private AnimationCurve FadeOutAccelerationCurve = new(new Keyframe(0, 1), new Keyframe(1, 0));

    [SerializeField] private List<Image> images;
    [SerializeField] private List<TMP_Text> texts;
    
    private Coroutine _fadeRoutine;

    public void AutoFadeInAndOut()
    {
        SetAlpha(images, 0f);
        SetAlpha(texts, 0f);

        FadeIn();

        Invoke(nameof(FadeOut), fadeInTime + autoFadeWaitTime);
    }

    public void FadeIn()
    {
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
        activeTarget.SetActive(true);
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
        activeTarget.SetActive(false);
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
