using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites : MonoBehaviour
{
    public static Dictionary<int, SpriteCollection.SpriteData> sprites;
    public static Dictionary<Texture2D, Material> textureMaterialMap;

    public static void LoadSprites()
    {
        sprites = new Dictionary<int, SpriteCollection.SpriteData>();

        SpriteCollection spriteCollection = Resources.Load("SpriteCollection", typeof(SpriteCollection)) as SpriteCollection;
        if (spriteCollection != null)
        {
            for (int i = 0; i < spriteCollection.list.Count; i++)
            {
                var spriteData = spriteCollection.list[i];
                sprites.Add(spriteData.id, spriteData);
            }
        }
    }

    public static SpriteCollection.SpriteData GetSprite(int spriteID)
    {
        SpriteCollection.SpriteData sprite = null;
        sprites.TryGetValue(spriteID, out sprite);
        return sprite;
    }

    public static Material GetTextureMaterial(Texture2D texture, GameData.RenderingLayer layer, int order)
    {
        if (textureMaterialMap == null)
        {
            textureMaterialMap = new Dictionary<Texture2D, Material>();
        }

        Material material = null;
        if (!textureMaterialMap.TryGetValue(texture, out material))
        {
            material = Instantiate(Game.SceneManager.instance.RenderQuadMaterial) as Material;
            material.mainTexture = texture;
            textureMaterialMap.Add(texture, material);
        }
        material.renderQueue = 3000 * 100 * (int)layer + order;
        return material;
    }
}
