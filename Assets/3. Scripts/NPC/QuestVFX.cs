using System;
using Interaction;
using UnityEngine;

public class QuestVFX : MonoBehaviour
{
    [SerializeField] private InteractionTrigger trigger;
    [SerializeField] private GameObject vfx;

    private void Start()
    {
        vfx.SetActive(false);
        if (trigger.interactionButtonType == InteractionButtonType.Question)
        {
            vfx.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (trigger.interactionButtonType == InteractionButtonType.Question)
        {
            if (!vfx.activeInHierarchy)
                return;
            vfx.SetActive(true);
            return;
        }
        if (vfx.activeInHierarchy)
        {
            vfx.SetActive(false);
        }
    }
}
