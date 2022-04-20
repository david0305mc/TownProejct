using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpriteCollection", menuName = "Scriptable Object/SpriteCollection", order = int.MaxValue)]
public class SpriteCollection : ScriptableObject
{
    [System.Serializable]
    public class TextureData
    {
        public SpriteData parent;
        public Texture2D texture;
        public float offsetX = 0.0f;
        public float offsetY = 0.0f;
        public float scale = 100.0f;

        public int numberOfColumns = 1;
        public int numberOfRows = 1;
        public int framesCount = 1;
        public int fps = 1;

        public TextureData(SpriteData parent)
        {
            this.parent = parent;
        }
    }

    [System.Serializable]
    public class SpriteData
    {
        public int id;
        public string name;
        public int gridSize = 4;
        public GameData.RenderingLayer renderingLayer;
        public int renderingOrder;

        public TextureData bottomTexture;
        public TextureData bottomRightTexture;
        public TextureData rightTexture;
        public TextureData topRightTexture;
        public TextureData topTexture;
        public SpriteData()
        {
            bottomTexture = new TextureData(this);
            bottomRightTexture = new TextureData(this);
            rightTexture = new TextureData(this);
            topRightTexture = new TextureData(this);
            topTexture = new TextureData(this);
        }


        public TextureData GetTextureData(GameData.Direction direction)
        {
            TextureData textureData = null;
            switch (direction)
            {
                case GameData.Direction.BOTTOM:
                    textureData = bottomTexture;
                    break;
                case GameData.Direction.BOTTOM_RIGHT:
                    textureData = bottomRightTexture;
                    break;
                case GameData.Direction.RIGHT:
                    textureData = rightTexture;
                    break;
                case GameData.Direction.TOP_RIGHT:
                    textureData = topRightTexture;
                    break;
                case GameData.Direction.TOP:
                    textureData = topTexture;
                    break;

                case GameData.Direction.TOP_LEFT:
                    textureData = topRightTexture;
                    break;
                case GameData.Direction.LEFT:
                    textureData = rightTexture;
                    break;
                case GameData.Direction.BOTTOM_LEFT:
                    textureData = bottomRightTexture;
                    break;
            }

            if (textureData.texture == null)
            {
                textureData = bottomTexture;
            }

            return textureData;
        }
    }

    public List<SpriteData> list = new List<SpriteData>();

    public void AddNewSprite()
    {
        SpriteData newSpriteData = new SpriteData();
        newSpriteData.id = this._GetUnusedId();
        newSpriteData.name = "New Sprite";
        this.list.Add(newSpriteData);
    }

    public void RemoveSprite(int index)
    {
        list.RemoveAt(index);
    }

    public SpriteData GetSprite(int id)
    {
        for (int index = 0; index < this.list.Count; index++)
        {
            if (id == list[index].id)
            {
                return list[index];
            }
        }
        return null;
    }

    private int _GetUnusedId()
    {
        int id = Random.Range(100, 999);
        for (int index = 0; index < this.list.Count; index++)
        {
            if (id == list[index].id)
            {
                return _GetUnusedId();
            }
        }
        return id;
    }

}
