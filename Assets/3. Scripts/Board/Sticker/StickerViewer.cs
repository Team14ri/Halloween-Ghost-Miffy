using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StickerViewer : MonoBehaviour
{
    [SerializeField] private StickerLocation location;
    [SerializeField] private List<StickerItemButton> stickerStorage;
    
    private void Init()
    {
        foreach (var storage in stickerStorage)
        {
            storage.gameObject.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        Init();
    }
    
    public void UpdateStickerBoard(StickerData sticker)
    {
        var stickerData = VariableManager.Instance.GetStickersList()
            .Where(data => data.GetValue() > 0 && 
                           data.Location == location &&
                           data.Type == StickerType.Normal)
            .ToList();

        for (int i = 0; i < stickerData.Count; ++i)
        {
            stickerStorage[i].gameObject.SetActive(true);
            stickerStorage[i].SetItem(stickerData[i]);
        }
    }
}
