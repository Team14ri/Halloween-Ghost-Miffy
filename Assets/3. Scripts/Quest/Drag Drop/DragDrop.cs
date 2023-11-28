using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;

    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private RectTransform _canvasRectTransform;

    private DropArea _lastDropArea;
    
    private Vector2 _originalPosition;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasRectTransform = canvas.GetComponent<RectTransform>();
        
        _originalPosition = _rectTransform.anchoredPosition;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_lastDropArea == null)
            return;

        _lastDropArea.IsFill = false;
    }
    
    private bool IsDroppedInValidArea(PointerEventData eventData)
    {
        if (!eventData.pointerCurrentRaycast.isValid)
            return false;
        
        _lastDropArea = eventData.pointerCurrentRaycast.gameObject.GetComponent<DropArea>();

        if (_lastDropArea == null)
            return false;

        if (_lastDropArea.IsFill)
            return false;

        _lastDropArea.IsFill = true;
        return true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsDroppedInValidArea(eventData))
        {
            _rectTransform.anchoredPosition = _originalPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPosition = _rectTransform.anchoredPosition + eventData.delta / canvas.scaleFactor;

        Vector2 canvasSize = _canvasRectTransform.sizeDelta;
        
        Vector2 rectSize = _rectTransform.sizeDelta;

        float minX = (rectSize.x - canvasSize.x) * 0.5f;
        float maxX = (canvasSize.x - rectSize.x) * 0.5f;
        float minY = (rectSize.y - canvasSize.y) * 0.5f;
        float maxY = (canvasSize.y - rectSize.y) * 0.5f;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        _rectTransform.anchoredPosition = newPosition;
    }
}
