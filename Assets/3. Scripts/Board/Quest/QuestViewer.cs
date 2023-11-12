using UnityEngine;

public class QuestViewer : MonoBehaviour
{
    [SerializeField] private GameObject plazaFoldField;
    [SerializeField] private GameObject mallFoldField;

    private void OnEnable()
    {
        plazaFoldField.SetActive(false);
        mallFoldField.SetActive(false);
    }
}
