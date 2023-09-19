using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoSizeByText : MonoBehaviour
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
