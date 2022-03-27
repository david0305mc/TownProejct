using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPopupManager : MonoBehaviour
{
    public static MPopupManager Inst;

    [SerializeField] private GameObject shopUIPrefab;
    [SerializeField] private GameObject popupRoot;

    private List<MPopupBase> popupList;

    private void Awake()
    {
        Inst = this;
        popupList = new List<MPopupBase>();
    }

    public void ShowShopMainPopup()
    {
        ShowPopup(shopUIPrefab);
    }

    public MPopupBase ShowPopup(GameObject prefab)
    {
        MPopupBase popupBase = MUtilities.CreatePopup<MPopupBase>(prefab, popupRoot, true).GetComponent<MPopupBase>();
        popupList.Add(popupBase);
        return popupBase;
    }
}
