using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T3
{
    public class GameNetworkSystem : NetworkSingleton<RequestBase, GameNetworkSystem>, IDefaultParam
    {
        public string UniqueueID { get; set; }
        public string Session { get; set; }
    }

    public class NetworkSingleton<T0, T> : T3NetworkSystem<T0> , ISigleton<T>
        where T0 : T3Request
        where T : class, new()
    {
        public static T Instance { get; protected set; }
        public void Remove()
        {
            Destroy(this.gameObject);
        }

        public void StartUp(T _instance)
        {
            Instance = _instance;
        }
    }
}
