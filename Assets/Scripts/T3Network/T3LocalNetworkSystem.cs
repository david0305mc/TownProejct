using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace T3
{
    public class T3LocalNetworkSystem : GameNetworkSystem
    {
        protected override void Update()
        {
            if (currentPacket != null)
            {
                var packet = (DummyWebRequest)currentPacket.request;
                int resultCode = (int)packet.result;
                if (resultCode >= 2)
                {
                    Debug.Log(currentPacket.request.result);
                    Debug.Log(currentPacket.request.error);
                    currentPacket.Task.SetCanceled();
                    currentPacket.request.Dispose();
                    currentPacket = null;
                    return;
                }

                if (packet.result == UnityWebRequest.Result.Success)
                {
                    // LocalServer
                    Debug.Log("LocalServer");

                    if (currentPacket.Complete != null)
                    {
                        currentPacket.Complete(currentPacket);
                        currentPacket.Task.SetResult(currentPacket);
                    }
                    currentPacket.request.Dispose();
                    currentPacket = null;
                }

            }
            else
            {
                if (packetQueue.Count <= 0)
                {
                    return;
                }

                currentPacket = packetQueue.Dequeue();
                currentPacket.request = new DummyWebRequest()
                {
                    endTime = Time.realtimeSinceStartup
                };
                currentPacket.request.downloadHandler = new CustomDownloadHandler();
            }
        }
    }

    public class CustomDownloadHandler : DownloadHandlerScript
    {
        new public string text { get; set; }
    }
    public class DummyWebRequest : UnityWebRequest
    {
        public float endTime;
        new public Result result
        {
            get 
            {
                return Time.realtimeSinceStartup >= endTime ? Result.Success : Result.InProgress;
            }
        }
    }

}
