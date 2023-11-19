using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class AutoResizeParent : MonoBehaviour
{
    [SerializeField] private bool appendSize;
    
    [Header("Adjustment Settings")]
    [SerializeField] private bool adjustWidth = true;
    [SerializeField] private float offsetWidth;
    
    [SerializeField] private bool adjustHeight = true;
    [SerializeField] private float offsetHeight;
    
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    private bool updateParentLayout;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        if (parentRectTransform != null)
        {
            updateParentLayout = true;
        }
        
        if (appendSize)
        {
            AdjustSizeTotal();
        }
        else
        {
            AdjustSizeMax();
        }
    }

    private void Update()
    {
        if (appendSize)
        {
            AdjustSizeTotal();
        }
        else
        {
            AdjustSizeMax();
        }
    }
    
    private void AdjustSizeTotal()
    {
        float totalWidth = 0;
        float totalHeight = 0;

        foreach (RectTransform child in rectTransform)
        {
            if (!child.gameObject.activeSelf)
                continue;
            
            if (adjustWidth)
            {
                totalWidth += child.rect.width;
            }

            if (adjustHeight)
            {
                totalHeight += child.rect.height;
            }
        }

        Vector2 oldSize = rectTransform.sizeDelta;
        bool updateLayout = false;
        
        if (adjustWidth)
        {
            if (!Mathf.Approximately(totalWidth + offsetWidth, oldSize.x))
            {
                updateLayout = true;
            }
            rectTransform.sizeDelta = new Vector2(totalWidth + offsetWidth, rectTransform.sizeDelta.y);
        }

        if (adjustHeight)
        {
            if (!Mathf.Approximately(oldSize.y, totalHeight + offsetHeight))
            {
                updateLayout = true;
            }
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalHeight + offsetHeight);
        }

        if (updateParentLayout && updateLayout)
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform);
        }
    }

    private void AdjustSizeMax()
    {
        float maxWidth = 0f;
        float maxHeight = 0f;

        foreach (RectTransform child in rectTransform)
        {
            if (!child.gameObject.activeSelf)
                continue;
            
            if (adjustWidth && child.rect.width > maxWidth)
            {
                maxWidth = child.rect.width;
            }

            if (adjustHeight && child.rect.height > maxHeight)
            {
                maxHeight = child.rect.height;
            }
        }

        Vector2 oldSize = rectTransform.sizeDelta;
        bool updateLayout = false;
        
        if (adjustWidth)
        {
            if (!Mathf.Approximately(maxWidth + offsetWidth, oldSize.x))
            {
                updateLayout = true;
            }
            rectTransform.sizeDelta = new Vector2(maxWidth + offsetWidth, rectTransform.sizeDelta.y);
        }

        if (adjustHeight)
        {
            if (!Mathf.Approximately(oldSize.y, maxHeight + offsetHeight))
            {
                updateLayout = true;
            }
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, maxHeight + offsetHeight);
        }

        if (updateParentLayout && updateLayout)
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform);
        }
    }
}
