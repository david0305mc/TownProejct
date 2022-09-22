using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T3
{
    //public class T3TestRecvData 
    public class T3TestPacket : RequestBase
    {
        public override int Method => 4011;


        public TestRecvData GetData()
        {
            return GetData<TestRecvData>();
        }
    }

    public class TestRecvData : T3Response
    { 
    
    }

}
