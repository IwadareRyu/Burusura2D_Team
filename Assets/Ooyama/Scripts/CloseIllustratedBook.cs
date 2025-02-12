using UnityEngine;

public class CloseIllustratedBook : MonoBehaviour
{
    [SerializeField] TitleController _titleController;
    private void OnEnable()
    {
        if (_titleController == null) _titleController = FindAnyObjectByType<TitleController>();
    }
    private void OnDisable()
    {
        _titleController.CloseTab();
    }
}
