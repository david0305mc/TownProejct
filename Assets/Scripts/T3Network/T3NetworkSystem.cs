using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T3
{
    public class T3NetworkSystem<T> : MonoBehaviour where T : T3Request
    {
        protected Queue<T> packetQueue = new Queue<T>();
        protected T currentPacket = null;

        public NetworkEventReceiver errorHandler { get; protected set; }
        public virtual void ChengeErrorHandler(NetworkEventReceiver handler)
        {
            errorHandler = handler;
        }

        public void AddPacket(T packet)
        {
            packet.CreateTask();
            packetQueue.Enqueue(packet);
        }

        protected virtual void Update()
        {
            if (currentPacket != null)
            {
                int resultCode = (int)currentPacket.request.result;
                if (resultCode >= 2)
                {
                    Debug.Log(currentPacket.request.result);
                    Debug.Log(currentPacket.request.error);

                    if (errorHandler != null)
                    {
                        errorHandler.OnError(currentPacket);
                    }

                    currentPacket.request.Dispose();
                    currentPacket = null;
                    return;
                }

                if (currentPacket.request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
                {
                    //ParsingRawData(currentPacket.request.downloadHandler.text);
                }
            }
            else
            {
                if (packetQueue.Count <= 0)
                {
                    return;
                }
                currentPacket = packetQueue.Dequeue();
                currentPacket.MakeRequest().SendWebRequest();
            }
        }
    }

    public abstract class NetworkEventReceiver : MonoBehaviour
    {
        public abstract void OnError(T3Request packet);
        public abstract void OnTimeOut();
    }
}
