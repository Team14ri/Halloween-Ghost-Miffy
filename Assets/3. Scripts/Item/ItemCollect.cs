using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollect : MonoBehaviour
{
    [SerializeField] private TMP_Text titleField;
    [SerializeField] private Image imageField;
    
    public static ItemCollect Instance;

    private Animator _animator;
    
    private static readonly int Trigger = Animator.StringToHash("Trigger");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void ShowPopup(string title, Sprite itemSprite)
    {
        _animator.SetTrigger(Trigger);
        titleField.text = title;
        imageField.sprite = itemSprite;
    }
}
