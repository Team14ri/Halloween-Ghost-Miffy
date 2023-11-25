using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform dropAreaPosition;
    public bool IsFill { get; set; }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        if (eventData.pointerDrag.GetComponent<DragDrop>() == null)
            return;

        Transform droppedItemTransform = eventData.pointerDrag.GetComponent<Transform>();

        droppedItemTransform.position = dropAreaPosition.position;
    }
}
