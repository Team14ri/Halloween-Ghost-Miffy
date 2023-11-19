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
    [TabGroup("Big Sticker", "Plaza"), SerializeField]
    private Sprite plazaFieldOn;
    [TabGroup("Big Sticker", "Plaza"), SerializeField]
    private Sprite plazaFieldOff;
    
    [TabGroup("Big Sticker", "Mall"), SerializeField]
    private StickerData mallSticker;
    [TabGroup("Big Sticker", "Mall"), SerializeField]
    private Image mallFieldImage;
    [TabGroup("Big Sticker", "Mall"), SerializeField]
    private Sprite mallFieldOn;
    [TabGroup("Big Sticker", "Mall"), SerializeField]
    private Sprite mallFieldOff;
    
    private void OnEnable()
    {
        plazaFieldImage.sprite = plazaFieldOff;
        mallFieldImage.sprite = mallFieldOff;
        
        if (plazaSticker.GetValue() > 0)
        {
            plazaFieldImage.sprite = plazaFieldOn;
        }
        if (mallSticker.GetValue() > 0)
        {
            mallFieldImage.sprite = mallFieldOn;
        }
    }
}
