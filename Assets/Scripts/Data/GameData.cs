using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData 
{
    public static string tableRootPath = Application.streamingAssetsPath + "/LocalTable/";


    public enum ShopCategory
    {
        ARMY,
        DEFENCE,
        RESOURCES,
        OTHER,
        TREASURE,
        DECORATIONS
    }

    public enum ShopSubCategory
    {
        BARRACK,
        BOAT,
        BUILDER_HUT,
        CAMP,
        CANNON,
        ELIXIR_COLLECTOR,
        ELIXIR_STORAGE,
        GEMS,
        GOLD_MINE,
        GOLD_STORAGE,
        TOWER,
        TOWN_CENTER,
        TREE1,
        TREE2,
        TREE3,
        WINDMILL,
        WALL

    }

    public enum RenderingLayer
    {
        GROUND = 0,
        SHADOW = 1,
        SPRITE = 2
    }
    public enum State
    {
        IDLE,
        WALK,
        ATTACK,
        DESTROYED
    }

    public enum Direction
    {
        BOTTOM,
        BOTTOM_RIGHT,
        RIGHT,
        TOP_RIGHT,
        TOP,
        TOP_LEFT,
        LEFT,
        BOTTOM_LEFT
    }
}
