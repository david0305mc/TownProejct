using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BasicSystemTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator ProcessLoadTable()
    {
        var types = typeof(ST_Price).Assembly.GetTypes();
        foreach (var type in types)
        {
            var method = type.GetMethod("LoadFromCSV");
            Debug.Log($"LoadTable {type.FullName}");
            string tablefilename = type.FullName.Replace("ST_", "");
            var req = UnityWebRequest.Get($"{Application.streamingAssetsPath}/LocalTable/{tablefilename}.csv");
            yield return req.SendWebRequest();
            method.Invoke(null, new object[] { req.downloadHandler.text });
        }

        yield return null;

    }
}
