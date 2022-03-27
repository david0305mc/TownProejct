using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MPopupBase : MonoBehaviour
{
    [SerializeField] private Button closeBtn = default;

    private void Awake()
    {
        closeBtn?.onClick.AsObservable().Subscribe(_ => {
            Close();
        });
    }

    public virtual void Close()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
