using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace T3
{
    public class RecvParse
    {
        public virtual T GetData<T>(string json) //where T : class, new()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }

    public class T3Request
    {
        public string Url;
        public int Tileout = 5;
        public string RawJson;


        public TaskCompletionSource<T3Request> Task { get; protected set; }
        public Task<T3Request> GetTask() { return Task.Task; }
        public virtual void CreateTask() { Task = new TaskCompletionSource<T3Request>(); }

        // Parse
        public RecvParse recvParse { get; set; } = new RecvParse();
        public UnityWebRequest request { get; set; }
    }

    public class RequestBase : T3Request
    {
        public string UniqueID;
        public virtual int Method { get; } = 0;
        public virtual bool InputBlock { get; set; } = true;

        protected virtual T GetData<T>()
        {
            return recvParse.GetData<T>(RawJson);
        }

        public JObject GetRawDataA()
        {
            return GetData<JObject>();
        }
    }

}
