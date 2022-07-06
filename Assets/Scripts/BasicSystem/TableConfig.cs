using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableConfig : SaveAct
{
    protected override string SaveData { 
        get { 
            return Newtonsoft.Json.JsonConvert.SerializeObject(tableDatas); 
        } 
    }

    protected override string SaveKey => "TableConfig";

    public Dictionary<string, TableConfigData> tableDatas = new Dictionary<string, TableConfigData>();
    protected override string Load()
    {
        string loaddata = base.Load();
        
        if (string.IsNullOrEmpty(loaddata) == false)
        { 
            tableDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, TableConfigData>>(loaddata); 
        }
        else
        { 
            tableDatas.Clear(); 
        }
        return null;
    }

    protected override void Save()
    {
        base.Save();
        //PlayerPrefs.SetString(SaveKey, Newtonsoft.Json.JsonConvert.SerializeObject(tableDatas));
    }

    public class TableConfigData
    {
        public string fileName;
        public string[] data;
        public bool CompareData(string[] tabledatas)
        {
            if (data.Length != tabledatas.Length) { return true; }

            for (int it = 0; it < data.Length; it++)
            {
                if (data[it] != tabledatas[it]) { return true; }
            }

            return false;
        }
    }
}
