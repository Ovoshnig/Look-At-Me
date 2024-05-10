using UnityEngine;

public interface ISelectable
{
    public bool IsSelected { get; set; }
}

public abstract class SelectableObject : MonoBehaviour, ISelectable
{
    private bool _isSelected;

    public bool IsSelected
    {
        get
        { 
            return _isSelected; 
        }
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                React();
            }
        }
    }

    protected abstract void React();
}
