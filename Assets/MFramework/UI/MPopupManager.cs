using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPopupManager : MonoBehaviour
{
    public static MPopupManager Inst;

    [SerializeField] private GameObject shopUIPrefab;
    [SerializeField] private GameObject uiShopSubPrefab;
    [SerializeField] private GameObject itemOptionUIPrefab;
    [SerializeField] private GameObject popupRoot;

    private List<MPopupBase> popupList;

    private void Awake()
    {
        Inst = this;
        popupList = new List<MPopupBase>();
    }

    public MPopupBase ShowShopMainPopup()
    {
        return ShowPopup(shopUIPrefab);
    }

    public MPopupBase ShowShopSubPopup()
    {
        return ShowPopup(uiShopSubPrefab);
    }

    public MPopupBase ShowItemOptionUI()
    {
        return ShowPopup(itemOptionUIPrefab);
    }

    public void CloseAllWindow()
    {
        for (int i = popupList.Count - 1; i > 0; i--)
        {
            popupList[i].Close();
        }
        popupList = new List<MPopupBase>();
    }

    private MPopupBase ShowPopup(GameObject prefab)
    {
        MPopupBase popupBase = MUtilities.CreatePopup<MPopupBase>(prefab, popupRoot, true).GetComponent<MPopupBase>();
        popupBase.SetOnHide((popup) => {
            popupList.Remove(popup);
            Destroy(popup.gameObject);
        });
        popupList.Add(popupBase);
        return popupBase;
    }
}
