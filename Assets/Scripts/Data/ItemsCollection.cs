using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsCollection", menuName = "Scriptable Object/ItemsCollection", order = int.MaxValue)]
public class ItemsCollection : ScriptableObject
{

    [System.Serializable]
    public class Configuration
    {
        public float buildTime = 0;
        public bool isCharacter;
        public float speed;
        public float attackRange = 0;
        public float defenceRange = 0;
        public float healthPoints;
        public float hitPoints;
        public float productionRate;
        public string product;
    }

    [System.Serializable]
    public class ItemData
    {
        public int id;
        public string name;
        public Texture2D thumb;
        public int gridSize = 4;
        public Configuration configuration = new Configuration();

        public List<int> idleSprites;
        public List<int> walkSprites;
        public List<int> attackSprites;
        public List<int> destroyedSprites;

        public ItemData()
        {
            this.idleSprites = new List<int>();
            this.walkSprites = new List<int>();
            this.attackSprites = new List<int>();
            this.destroyedSprites = new List<int>();
        }
        public void AddSprite(SpriteCollection.SpriteData sprite, GameData.State state)
        {
            List<int> sprites = idleSprites;
            switch (state)
            {
                case GameData.State.IDLE:
                    sprites = idleSprites;
                    break;
                case GameData.State.WALK:
                    sprites = walkSprites;
                    break;
                case GameData.State.ATTACK:
                    sprites = attackSprites;
                    break;
                case GameData.State.DESTROYED:
                    sprites = destroyedSprites;
                    break;
            }

            if (!sprites.Contains(sprite.id))
            {
                sprites.Add(sprite.id);
            }
        }

        public void RemoveSprite(SpriteCollection.SpriteData sprite, GameData.State state)
        {
            List<int> sprites = idleSprites;
            switch (state)
            {
                case GameData.State.IDLE:
                    sprites = idleSprites;
                    break;
                case GameData.State.WALK:
                    sprites = walkSprites;
                    break;
                case GameData.State.ATTACK:
                    sprites = attackSprites;
                    break;
                case GameData.State.DESTROYED:
                    sprites = destroyedSprites;
                    break;
            }

            if (sprites.Contains(sprite.id))
            {
                sprites.Remove(sprite.id);
            }
        }

        public List<int> GetSprites(GameData.State state)
        {
            switch (state)
            {
                case GameData.State.IDLE:
                    return idleSprites;
                case GameData.State.WALK:
                    return walkSprites;
                case GameData.State.ATTACK:
                    return attackSprites;
                case GameData.State.DESTROYED:
                    return destroyedSprites;
            }

            return idleSprites;
        }

    }

    public List<ItemData> list = new List<ItemData>();

    public void AddNewItem()
    {
        ItemData newItemData = new ItemData();
        newItemData.id = this._GetUnusedId();
        newItemData.name = "New Item";

        this.list.Add(newItemData);
    }

    public void RemoveItem(int index)
    {
        this.list.RemoveAt(index - 1);
    }

    private int _GetUnusedId()
    {
        int id = Random.Range(1000, 9999);
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
