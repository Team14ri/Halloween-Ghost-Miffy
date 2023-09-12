using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoSizeByText : MonoBehaviour
{
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private GameObject _gameObject;

    private void Update()
    {
        Debug.Log(textBox.GetRenderedValues(true));
        _gameObject.GetComponent<RectTransform>().sizeDelta = textBox.GetRenderedValues(true);
    }
}
