using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int instanceId;
    public int itemId;
    public int posX;
    public int posZ;
}

[System.Serializable]
public class SceneData
{
    string testData;
    public List<ItemData> items;
    public SceneData()
    {
        items = new List<ItemData>();
    }

    public void AddOrUpdateItem(int instanceID, int itemID, int posX, int posZ)
    {
        var itemInst = items.Find(item => item.instanceId == instanceID);
        if (itemInst == null)
        {
            itemInst = new ItemData();
            itemInst.instanceId = instanceID;
            itemInst.itemId = itemID;
            items.Add(itemInst);
        }
        itemInst.posX = posX;
        itemInst.posZ = posZ;
    }
}

public class DatabaseManager : Singleton<DatabaseManager>
{
    private string gameDataFilePath = "/db.json";
    private SceneData sceneData;

    private string filePath;
    protected override void OnSingletonAwake()
    {
        EnsureGameDataFileExists();
    }

    public void UpdateItemData(BaseItemScript item)
    {
        sceneData.AddOrUpdateItem(item.instanceId, item.itemData.id, (int)item.GetPosition().x, (int)item.GetPosition().z);
        SaveDataBase();
    }

    private void EnsureGameDataFileExists()
    {
        sceneData = new SceneData();
           
        Debug.Log(Application.streamingAssetsPath);
        filePath = Application.streamingAssetsPath + gameDataFilePath;
        //var directoryPath = Directory.GetParent(filePath);
        //Debug.Log(directoryPath.FullName);

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            sceneData = JsonUtility.FromJson<SceneData>(jsonData);
        }
        else
        {
            SaveDataBase();
        }
    }

    public SceneData GetScene()
    {
        return sceneData;
    }
    public void SaveDataBase()
    {
        string jsonData = JsonUtility.ToJson(sceneData);
        File.WriteAllText(filePath, jsonData);
    }
}
