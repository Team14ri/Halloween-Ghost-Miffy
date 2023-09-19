using TMPro;
using UnityEngine;

public class DynamicScaleTextbox : MonoBehaviour
{
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Vector2 sizeOffset = new(0, 0);
    
    private void Update()
    {
        targetObject.GetComponent<RectTransform>().sizeDelta = textBox.GetRenderedValues(true);
        if (textBox.text.Length != 0)
        {
            targetObject.GetComponent<RectTransform>().sizeDelta += sizeOffset;
        }
    }
}
