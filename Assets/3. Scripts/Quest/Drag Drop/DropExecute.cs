using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DropExecute : MonoBehaviour
{
    [SerializeField] private List<DropArea> dropAreas;
    [SerializeField] private UnityEvent unityEvent;

    private bool _executeDone;

    private void Start()
    {
        _executeDone = false;
    }

    private void Update()
    {
        if (_executeDone)
            return;
        
        bool dropAreasFill = dropAreas.All(x => x.IsFill);
        if (dropAreasFill)
        {
            _executeDone = true;
            unityEvent.Invoke();
        }
    }
}
