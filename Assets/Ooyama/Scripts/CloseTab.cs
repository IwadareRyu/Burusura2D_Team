using UnityEngine;

public class CloseTab : MonoBehaviour
{
    [SerializeField] TitleController _titleController;
    private void OnEnable()
    {
        if (_titleController == null) _titleController = FindAnyObjectByType<TitleController>();
    }
    private void OnDisable()
    {
        _titleController?.CloseTab();
    }
}
