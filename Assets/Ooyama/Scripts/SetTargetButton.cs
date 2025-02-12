using UnityEngine;
using UnityEngine.UI;

public class SetTargetButton : MonoBehaviour
{
    [SerializeField] private TitleController _titleController;
    [SerializeField] private Button _button;
    private void OnEnable()
    {
        if (_titleController == null) _titleController = FindAnyObjectByType<TitleController>();
        _titleController.SetTarget(_button);
    }
}
