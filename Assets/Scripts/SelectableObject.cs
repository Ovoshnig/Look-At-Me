using UnityEngine;

public interface ISelectable
{
    void SetSelected(bool isSelected);
    bool GetSelected();
}

public class SelectableObject : MonoBehaviour, ISelectable
{
    protected bool _isSelect = false;
    protected bool _isStartCoroutineAllowed = true;

    public virtual void SetSelected(bool isSelect) => _isSelect = isSelect;
    
    public bool GetSelected() => _isSelect;
}
