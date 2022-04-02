using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSubPopup : MPopupBase
{
    // prefab
    [SerializeField] private ShopSubItem shopSubItemPrefab;


    // reference
    [SerializeField] private ScrollRect scrollRect;


    private void Start()
    {
        GameData.ShopCategory[] categories = new GameData.ShopCategory[] {
            GameData.ShopCategory.ARMY,
            GameData.ShopCategory.DECORATIONS,
            GameData.ShopCategory.DEFENCE,
            GameData.ShopCategory.OTHER,
            GameData.ShopCategory.RESOURCES,
            GameData.ShopCategory.TREASURE
        };

        for (int i = 0; i < 5; i++)
        {
            var item = MUtilities.CreateInstance(shopSubItemPrefab.gameObject, scrollRect.content.gameObject, true).GetComponent<ShopSubItem>();
            //item.SetCategory(categories[i]);

        }
    }
}
