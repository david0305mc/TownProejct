using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMainPopup : MPopupBase
{
    // prefab
    [SerializeField] private ShopMainItem shopMainItemPrefab;


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
            var item = MUtilities.CreateInstance(shopMainItemPrefab.gameObject, scrollRect.content.gameObject, true).GetComponent<ShopMainItem>();
            item.SetCategory(categories[i], (category)=> {
                var popup = MPopupManager.Inst.ShowShopSubPopup().GetComponent<ShopSubPopup>();
                popup.SetData(category);
            });

        }
    }
}
