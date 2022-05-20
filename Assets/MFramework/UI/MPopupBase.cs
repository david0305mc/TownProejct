using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MPopupBase : MonoBehaviour
{
    [SerializeField] private Button closeBtn = default;

    private System.Action<MPopupBase> hideAction;
    private void Awake()
    {
        closeBtn?.onClick.AsObservable().Subscribe(_ => {
            Close();
        });
    }

    public void SetOnHide(System.Action<MPopupBase> hide)
    {
        hideAction = hide;
    }

    public virtual void Close()
    {
        hideAction?.Invoke(this);
    }
}
