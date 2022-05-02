using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CultureInfoTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //CultureInfo myCIintl = new CultureInfo("es-ES", false);
        CultureInfo.CurrentCulture = new CultureInfo("es-ES", false);


        long test = DateTimeToTimeStamp(DateTime.Now);
        PlayerPrefs.SetString("unixTime", test.ToString());
        Debug.Log("dateTime 1 " + DateTime.Now);

        //CultureInfo.CurrentCulture = new CultureInfo("ko-KR", false);

        string timeStamp = PlayerPrefs.GetString("unixTime");
        DateTime test2 = TimeStampToDateTime(long.Parse(timeStamp));
        Debug.Log("dateTime 2 " + test2);
        
        //CultureInfo[] cinfos = CultureInfo.GetCultures(CultureTypes.AllCultures);
        //foreach (var info in cinfos)
        //{
        //    try
        //    {
        //        Debug.LogFormat("EnglishName:{0}, DisplayName:{1}", info.EnglishName, info.DisplayName);
        //        CultureInfo.CurrentCulture = info;  // 임시로 현재 문화권 설정

        //        //string d = System.DateTime.Now.ToString("yyyy/MM/dd");  //TEST API
        //        //Debug.Log(timeSpan.TotalMilliseconds.ToString("n0"));


        //        Debug.Log(DateTime.Now.ToString());


        //    }
        //    catch (System.Exception ex)
        //    {
        //        Debug.LogErrorFormat("ex={0}", ex.Message);
        //    }
        //}
    }
    public long GetCurrentTimeStamp()
    {
        return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
    }

    public long DateTimeToTimeStamp(DateTime value)
    {
        return ((DateTimeOffset)value).ToUnixTimeSeconds();
    }
    public DateTime TimeStampToDateTime(long value)
    {
        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dt = dt.AddSeconds(value).ToLocalTime();
        return dt;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
