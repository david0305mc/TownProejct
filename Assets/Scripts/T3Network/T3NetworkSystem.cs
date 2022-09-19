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
    }

    public abstract class NetworkEventReceiver : MonoBehaviour
    {
        public abstract void OnError(T3Request packet);
        public abstract void OnTimeOut();
    }
}
