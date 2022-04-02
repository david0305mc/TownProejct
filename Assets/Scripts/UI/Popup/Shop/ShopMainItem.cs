using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMainItem : MonoBehaviour
{
    public Sprite ArmySprite;
    public Sprite DefenceSprite;
    public Sprite OtherSprite;
    public Sprite ResourcesSprite;
    public Sprite TreasureSprite;
    public Sprite DecorationsSprite;

    /* references */
    public Text Name;
    public Image Image;
    public Button slotButton;

    /* private variables */
    private GameData.ShopCategory _category;

    public void SetCategory(GameData.ShopCategory category, UnityEngine.Events.UnityAction<GameData.ShopCategory> _action)
    {
        this._category = category;
        switch (this._category)
        {
            case GameData.ShopCategory.ARMY:
                this.Name.text = "ARMY";
                this.Image.sprite = this.ArmySprite;
                break;
            case GameData.ShopCategory.DEFENCE:
                this.Name.text = "DEFENCE";
                this.Image.sprite = this.DefenceSprite;
                break;
            case GameData.ShopCategory.OTHER:
                this.Name.text = "OTHER";
                this.Image.sprite = this.OtherSprite;
                break;
            case GameData.ShopCategory.RESOURCES:
                this.Name.text = "RESOURCES";
                this.Image.sprite = this.ResourcesSprite;
                break;
            case GameData.ShopCategory.TREASURE:
                this.Name.text = "TREASURE";
                this.Image.sprite = this.TreasureSprite;
                break;
            case GameData.ShopCategory.DECORATIONS:
                this.Name.text = "DECORATIONS";
                this.Image.sprite = this.DecorationsSprite;
                break;
        }
        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(()=> { _action(category); });
    }
}
