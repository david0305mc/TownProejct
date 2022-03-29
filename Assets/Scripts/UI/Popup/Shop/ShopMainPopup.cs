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

        GameData.Category[] categories = new GameData.Category[] {
            GameData.Category.ARMY,
            GameData.Category.DECORATIONS,
            GameData.Category.DEFENCE,
            GameData.Category.OTHER,
            GameData.Category.RESOURCES,
            GameData.Category.TREASURE
        };

        for (int i = 0; i < 5; i++)
        {
            var item = MUtilities.CreateInstance(shopMainItemPrefab.gameObject, scrollRect.content.gameObject, true).GetComponent<ShopMainItem>();
            item.SetCategory(categories[i]);

        }
    }
}
