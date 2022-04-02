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


    public void SetData(GameData.ShopCategory category)
    {
        GameData.ShopSubCategory[] subCategory = new GameData.ShopSubCategory[0];

        switch (category)
        {
            case GameData.ShopCategory.ARMY:
                {
                    subCategory = new GameData.ShopSubCategory[] { GameData.ShopSubCategory.BARRACK, GameData.ShopSubCategory.CAMP, GameData.ShopSubCategory.BOAT };
                }
                break;
            case GameData.ShopCategory.DECORATIONS:
                {
                    subCategory = new GameData.ShopSubCategory[] { GameData.ShopSubCategory.TREE1, GameData.ShopSubCategory.TREE2, GameData.ShopSubCategory.TREE3 };
                }
                break;
            case GameData.ShopCategory.DEFENCE:
                {
                    subCategory = new GameData.ShopSubCategory[] { GameData.ShopSubCategory.CANNON, GameData.ShopSubCategory.TOWER};
                }
                break;
            case GameData.ShopCategory.OTHER:
                {
                    subCategory = new GameData.ShopSubCategory[] { GameData.ShopSubCategory.TOWN_CENTER, GameData.ShopSubCategory.BUILDER_HUT, GameData.ShopSubCategory.WALL };
                }
                break;
            case GameData.ShopCategory.RESOURCES:
                {
                    subCategory = new GameData.ShopSubCategory[] { GameData.ShopSubCategory.ELIXIR_COLLECTOR, GameData.ShopSubCategory.ELIXIR_STORAGE, GameData.ShopSubCategory.GOLD_MINE, GameData.ShopSubCategory.WINDMILL};
                }
                break;
            case GameData.ShopCategory.TREASURE:
                {
                    subCategory = new GameData.ShopSubCategory[] { GameData.ShopSubCategory.GEMS };
                }
                break;

        }

        for (int i = 0; i < subCategory.Length; i++)
        {
            var item = MUtilities.CreateInstance(shopSubItemPrefab.gameObject, scrollRect.content.gameObject, true).GetComponent<ShopSubItem>();
            item.SetSubCategory(subCategory[i]);
        }

    }
}
