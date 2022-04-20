using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public static Dictionary<int, ItemsCollection.ItemData> items;

    public static void LoadItems()
    {
        items = new Dictionary<int, ItemsCollection.ItemData>();

        ItemsCollection itemCollection = Resources.Load("ItemsCollection", typeof(ItemsCollection)) as ItemsCollection;
        if (itemCollection != null)
        {
            for (int index = 0; index < itemCollection.list.Count; index++)
            {
                ItemsCollection.ItemData itemData = itemCollection.list[index];
                items.Add(itemData.id, itemData);
            }
        }
        else
        {
            Debug.LogError("ItemsCollection is missing! please go to 'Windows/Item Editor'");
        }
    }

    public static ItemsCollection.ItemData GetItem(int itemID)
    {
        ItemsCollection.ItemData item = null;
        items.TryGetValue(itemID, out item);
        return item;
    }
}
