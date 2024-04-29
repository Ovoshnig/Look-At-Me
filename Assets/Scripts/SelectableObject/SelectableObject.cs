using UnityEngine;

public interface ISelectable
{
    void SetSelected(bool isSelected);
    bool GetSelected();
}

public class SelectableObject : MonoBehaviour, ISelectable
{
    protected int delayTime;

    protected bool IsSelect = false;

    public virtual void SetSelected(bool isSelect) => IsSelect = isSelect;
    
    public bool GetSelected() => IsSelect;
}
