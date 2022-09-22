using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using T3;
public class T3NetworkTest : MonoBehaviour
{
    private void Awake()
    {

        var networkSystem = GameUtil.MakeGameObject<GameNetworkSystem>("GameNetworkSystem", transform);
        networkSystem.StartUp(networkSystem);
    }

    public void OnClickBtnTest()
    {
        RequestAreaUpgrade(100, (request) =>
        {
            Debug.Log("Success");
        }, 
        (request)=> {
            Debug.Log("Failed");
        });
    }
    public void RequestAreaUpgrade(int _tID, System.Action<T3Request> complete = null, System.Action<T3Request> failed = null)
    {
        var packet = new T3TestPacket();
        T3PacketParamBuilder builder = new T3PacketParamBuilder(GameNetworkSystem.Instance);
        builder.AddParam("tID", _tID.ToString());
        builder.SetMethod(packet.Method);

        packet.paramBuilder = builder;
        packet.Complete = complete;
        packet.Failed = failed;

        GameNetworkSystem.Instance.AddPacket(packet);
    }
}
