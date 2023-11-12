using UnityEngine;
using UnityEngine.UI;

public class SpriteOnOff : MonoBehaviour
{
    [SerializeField] private bool on;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Image target;

    private void Start()
    {
        SetOn(on);
    }

    public void SetOn(bool value)
    {
        on = value;
        target.sprite = on ? onSprite : offSprite;
    }
}
