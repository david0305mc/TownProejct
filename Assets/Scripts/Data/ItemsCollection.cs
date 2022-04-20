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
    }

    [System.Serializable]
    public class ItemData
    {
        public int id;
        public int name;
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
    }

    public List<ItemData> list = new List<ItemData>();
}
