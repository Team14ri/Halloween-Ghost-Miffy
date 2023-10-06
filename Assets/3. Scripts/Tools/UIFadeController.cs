using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeController : MonoBehaviour
{
    [SerializeField] private GameObject activeTarget;

    [SerializeField] private float fadeInTime = 1.1f;
    [SerializeField] private float fadeOutTime = 1.4f;
    [SerializeField] private float autoFadeWaitTime = 4f;

    [SerializeField] private List<Image> images;
    [SerializeField] private List<TMP_Text> texts;

    private void Start()
    {
        Invoke(nameof(AutoFadeInAndOut), 1f);
    }

    public void AutoFadeInAndOut()
    {
        SetAlpha(images, 0f);
        SetAlpha(texts, 0f);

        StartCoroutine(FadeInProcess());

        Invoke(nameof(FadeOut), fadeInTime + autoFadeWaitTime);
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInProcess());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutProcess());
    }

    IEnumerator FadeInProcess()
    {
        activeTarget.SetActive(true);
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0, 1, t / fadeInTime);
            SetAlpha(images, alpha);
            SetAlpha(texts, alpha);
            yield return null;
        }
        SetAlpha(images, 1);
        SetAlpha(texts, 1);
    }

    IEnumerator FadeOutProcess()
    {
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1, 0, t / fadeOutTime);
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
