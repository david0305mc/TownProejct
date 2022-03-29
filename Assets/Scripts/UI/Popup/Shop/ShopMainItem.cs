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


    /* private variables */
    private GameData.Category _category;

    public void SetCategory(GameData.Category category)
    {
        this._category = category;

        switch (this._category)
        {
            case GameData.Category.ARMY:
                this.Name.text = "ARMY";
                this.Image.sprite = this.ArmySprite;
                break;
            case GameData.Category.DEFENCE:
                this.Name.text = "DEFENCE";
                this.Image.sprite = this.DefenceSprite;
                break;
            case GameData.Category.OTHER:
                this.Name.text = "OTHER";
                this.Image.sprite = this.OtherSprite;
                break;
            case GameData.Category.RESOURCES:
                this.Name.text = "RESOURCES";
                this.Image.sprite = this.ResourcesSprite;
                break;
            case GameData.Category.TREASURE:
                this.Name.text = "TREASURE";
                this.Image.sprite = this.TreasureSprite;
                break;
            case GameData.Category.DECORATIONS:
                this.Name.text = "DECORATIONS";
                this.Image.sprite = this.DecorationsSprite;
                break;
        }
    }
}
