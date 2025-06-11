using UnityEngine;

public abstract class UiBase : MonoBehaviour
{
    public abstract void Init();

    public virtual void Show(bool _isActive)
    {
        this.gameObject.SetActive(_isActive);
    }
}
