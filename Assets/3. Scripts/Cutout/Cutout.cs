using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cutout : MonoBehaviour
{
    private RectTransform rectTransform;
    public float fadeSpeed = 1.0f;
    private Vector2 targetSize;
    
    public bool isBlackout = false;

    private void Awake()
    {
        if (isBlackout == true)
        {
            EnableAllChildren();
        }
        else
        {
            DisableAllChildren();
        }
    }

    private void Start()
    {
        Transform firstChild = transform.GetChild(0);
        rectTransform = firstChild.GetComponent<RectTransform>();
        
        targetSize = new Vector2(3000, 3000); // 목표 크기
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(FadeOutCoroutine());
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(FadeInCoroutine());
        }
    }

    public IEnumerator FadeInCoroutine()
    {
        EnableAllChildren();
        
        float elapsedTime = 0f;
        rectTransform.sizeDelta = Vector2.zero;
        Vector2 startSize = rectTransform.sizeDelta;

        while (elapsedTime < 1.0f)
        {
            rectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, elapsedTime);
            elapsedTime += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // 오차 보정
        rectTransform.sizeDelta = targetSize;
        
        DisableAllChildren();
    }

    public IEnumerator FadeOutCoroutine()
    {
        EnableAllChildren();

        float elapsedTime = 0f;
        Vector2 startSize = rectTransform.sizeDelta;

        while (elapsedTime < 1.0f)
        {
            rectTransform.sizeDelta = Vector2.Lerp(startSize, Vector2.zero, elapsedTime);
            elapsedTime += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // 오차 보정
        rectTransform.sizeDelta = Vector2.zero;
    }

    public void DisableAllChildren()
    {
        Transform parentTransform = transform;

        foreach (Transform child in parentTransform)
        {
            child.gameObject.SetActive(false);
        }
    }
    
    public void EnableAllChildren()
    {
        Transform parentTransform = transform;

        foreach (Transform child in parentTransform)
        {
            child.gameObject.SetActive(true);
        }
    }
}