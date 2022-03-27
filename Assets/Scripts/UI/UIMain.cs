using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [SerializeField] private Button shopBtn;

    private void Start()
    {
        InitUI();
    }

    public void InitUI()
    {
        shopBtn.onClick.AsObservable().Subscribe(_ =>
        {
            MPopupManager.Inst.ShowShopMainPopup();
        });
    }
}
