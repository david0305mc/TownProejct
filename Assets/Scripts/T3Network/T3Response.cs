using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace T3
{
    public class T3Response
    {
        public string id;
        public GoodsInfoData publickGoods;
        public ResponseError error = new ResponseError();
    }


    public class GoodsInfoData
    {
        public string Coin = "0";
        public string Heart = "0";
    }


    public class ResponseError
    {
        public int code = 0;
    }
}
