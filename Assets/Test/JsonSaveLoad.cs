using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JTestClass
{
    public int i;
    public float f;
    public bool b;
    public string str;
    public int[] iArray;
    public List<int> iList = new List<int>();
    public Dictionary<string, float> fDictionary = new Dictionary<string, float>();

    public JTestClass() { }


    public JTestClass(bool isSet)
    {

        if (isSet)

        {
            i = 10;
            f = 99.9f;
            b = true;
            str = "JSON Test String";
            iArray = new int[] { 1, 1, 3, 5, 8, 13, 21, 34, 55 };

            for (int idx = 0; idx < 5; idx++)
            {
                iList.Add(2 * idx);
            }



            fDictionary.Add("PIE", Mathf.PI);
            fDictionary.Add("Epsilon", Mathf.Epsilon);
            fDictionary.Add("Sqrt(2)", Mathf.Sqrt(2));

        }
    }


    public void Print()
    {
        Debug.Log("i = " + i);
        Debug.Log("f = " + f);
        Debug.Log("b = " + b);
        Debug.Log("str = " + str);

        for (int idx = 0; idx < iArray.Length; idx++)
        {
            Debug.Log(string.Format("iArray[{0}] = {1}", idx, iArray[idx]));
        }

        for (int idx = 0; idx < iList.Count; idx++)
        {
            Debug.Log(string.Format("iList[{0}] = {1}", idx, iList[idx]));
        }

        foreach (var data in fDictionary)
        {
            Debug.Log(string.Format("iDictionary[{0}] = {1}", data.Key, data.Value));
        }
    }
}

public class JsonSaveLoad : MonoBehaviour
{

    public void SaveTest()
    {
        JTestClass jTest = new JTestClass(true);
        string saveString = JsonConvert.SerializeObject(jTest, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/saveData.json", saveString);
    }
    public void Save()
    {
        SaveTest();
        return;
        JObject saveData = new JObject();
        saveData["key-name"] = "value-data";
        saveData["anyName"] = 1f;
        saveData["is-save"] = true;

        JArray arrayData = new JArray();
        Enumerable.Range(0, 5).ToList().ForEach(i =>
        {
            arrayData.Add(Random.Range(0, 10));
        });
        saveData["arrayData"] = arrayData;

        saveData["newarr"] = new JArray();
        Enumerable.Range(0, 5).ToList().ForEach(i => {
            ((JArray)saveData["newarr"]).Add(Random.Range(0, 50));
        });

        saveData["parent"] = new JObject();
        saveData["parent"]["child1"] = 123;
        saveData["parent"]["child2"] = 456;

        SaveData s = new SaveData();
        s.id = 1111;
        s.nameList.Add("asdfsdf");
        s.nameList.Add("skdfioie");
        saveData["class-saveData"] = JToken.FromObject(s);

        string saveString = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/saveData.json", saveString);
    }

    public void LoadTest()
    {
        string loadString = File.ReadAllText(Application.dataPath + "/saveData.json");
        JTestClass jTest = JsonConvert.DeserializeObject<JTestClass>(loadString);
        jTest.Print();
    }

    public void Load()
    {
        LoadTest();
        return;
        string loadString = File.ReadAllText(Application.dataPath + "/saveData.json");
        //JsonConverter<>

        JObject loadData = JObject.Parse(loadString);

        Debug.Log(loadData["class-saveData"]);

        foreach (var item in loadData)
        {
            Debug.Log($"item {item}" );
        }
    }
}

public class SaveData
{
    public int id;
    [JsonProperty("name-list")]
    public List<string> nameList;
    public SaveData() {
        id = 0;
        nameList = new List<string>();
    }

}
