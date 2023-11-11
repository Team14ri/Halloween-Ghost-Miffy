using System.Collections.Generic;
using DS.Core;
using UnityEngine;

public class OnOffToggleEditor : MonoBehaviour
{
    [SerializeField] private MultiDialogueHandler handler;
    [SerializeField] private GameObject horizontalOnOffPrefab;
    private readonly List<SpriteOnOff> _onOffToggles = new();

    private void OnEnable()
    {
        _onOffToggles.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        var (max, current)= handler.GetInfo();
        for (int i = 0; i < max; ++i)
        {
            GameObject temp = Instantiate(horizontalOnOffPrefab, transform.position, Quaternion.identity);
            temp.transform.SetParent(transform);
            temp.transform.localScale = new Vector3(1f, 1f, 1f);
            _onOffToggles.Add(temp.GetComponent<SpriteOnOff>());
        }
    }

    private void Update()
    {
        var (max, current)= handler.GetInfo();
        foreach (var toggle in _onOffToggles)
        {
            toggle.SetOn(false);
        }
        _onOffToggles[current].SetOn(true);
    }
}
