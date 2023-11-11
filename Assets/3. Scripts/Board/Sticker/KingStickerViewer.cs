using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class KingStickerViewer : MonoBehaviour
{
    [TabGroup("Big Sticker", "Plaza"), SerializeField]
    private StickerData plazaSticker;
    [TabGroup("Big Sticker", "Plaza"), SerializeField]
    private Image plazaFieldImage;
    
    [TabGroup("Big Sticker", "Mall"), SerializeField]
    private StickerData mallSticker;
    [TabGroup("Big Sticker", "Mall"), SerializeField]
    private Image mallFieldImage;
    
    [TabGroup("Big Sticker", "Forest"), SerializeField]
    private StickerData forestSticker;
    [TabGroup("Big Sticker", "Forest"), SerializeField]
    private Image forestFieldImage;

    private void OnEnable()
    {
        plazaFieldImage.color = new Color(1f, 1f, 1f, 0.3f);
        plazaFieldImage.sprite = plazaSticker.Sprite;
        
        mallFieldImage.color = new Color(1f, 1f, 1f, 0.3f);
        mallFieldImage.sprite = mallSticker.Sprite;
        
        forestFieldImage.color = new Color(1f, 1f, 1f, 0.3f);
        forestFieldImage.sprite = forestSticker.Sprite;
        
        if (plazaSticker.GetValue() > 0)
        {
            plazaFieldImage.color = new Color(1f, 1f, 1f, 1f);
        }
        if (mallSticker.GetValue() > 0)
        {
            mallFieldImage.color = new Color(1f, 1f, 1f, 1f);
        }
        if (forestSticker.GetValue() > 0)
        {
            forestFieldImage.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
