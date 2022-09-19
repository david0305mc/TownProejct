using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T3PacketParamBuilder
{
    public ParamData paramData { get; protected set; } = new ParamData();
    public ObjectParamData objParamData { get; protected set; } = new ObjectParamData();

    public class ParamData
    {
        public string id;
        public int method;

        public Dictionary<string, string> param = new Dictionary<string, string>();
        public string sess;
    }

    public class ObjectParamData
    {
        public string id;
        public int method;

        public JObject param = new JObject();
        public string sess;
    }

    public T3PacketParamBuilder(string id, string sess)
    {
        paramData.id = id;
        paramData.sess = sess;

        objParamData.id = id;
        objParamData.sess = sess;
    }
    public T3PacketParamBuilder(IDefaultParam data)
    {
        paramData.id = data.UniqueueID;
        paramData.sess = data.Session;

        objParamData.id = data.UniqueueID;
        objParamData.sess = data.Session;
    }

    public void SetMethod(int method)
    {
        paramData.method = method;
        objParamData.method = method;
    }

    public void SetParam(Dictionary<string, string> data) 
    {
        paramData.param = data; 
    }

    public void SetObjectParam(JObject data)
    {
        objParamData.param = data;
    }

    public void AddParam(string key, string val)
    {
        if (paramData.param.ContainsKey(key) == false)
        {
            paramData.param.Add(key, val);
        }
        else
        {
            paramData.param[key] = val;
        }
    }

    public string MakeParam()
    {
        if (paramData != null)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(paramData);
        }
        else
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(objParamData);
        }
    }

}


public interface IDefaultParam
{
    string UniqueueID { get; }
    string Session { get; }
}
