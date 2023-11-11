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
        plazaFieldImage.gameObject.SetActive(false);
        mallFieldImage.gameObject.SetActive(false);
        forestFieldImage.gameObject.SetActive(false);
        
        if (plazaSticker.GetValue() > 0)
        {
            plazaFieldImage.gameObject.SetActive(true);
            plazaFieldImage.sprite = plazaSticker.Sprite;
        }
        if (mallSticker.GetValue() > 0)
        {
            mallFieldImage.gameObject.SetActive(true);
            mallFieldImage.sprite = mallSticker.Sprite;
        }
        if (forestSticker.GetValue() > 0)
        {
            forestFieldImage.gameObject.SetActive(true);
            forestFieldImage.sprite = forestSticker.Sprite;
        }
    }
}
