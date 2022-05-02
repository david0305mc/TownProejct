using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncFileWriteTest : MonoBehaviour
{
    private static SD saveData;

    public static async Task<bool> SaveAsync()
    {
        if (saveData == null)
            saveData = await SerializationManager<SD>.LoadAsync();
        saveData.data = "test";
        return await SerializationManager<SD>.SaveAsync(saveData);
    }
}

public class SerializationManager<T> where T : SD
{

    public static string FILE_PATH { get; private set; }
    public static T Data { get; private set; }

    public static async Task<bool> SaveAsync(T data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log("File saved at: " + FILE_PATH);
        Data = data;
        return await SerializeDataAsync(bf);
    }

    internal static Task<SD> LoadAsync()
    {
        throw new NotImplementedException();
    }

    private static async Task<bool> SerializeDataAsync(BinaryFormatter bf)
    {
        await Task.Run(() =>
        {
            using (FileStream stream = File.Open(FILE_PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                Data.isSaved = true;
                bf.Serialize(stream, Data);
            }
        });
        Debug.Log("Saved Async");
        return true;
    }
}

public class SD
{
    public bool isSaved { get; internal set; }
    public string data { get; set; }
}
