using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebManager : Singleton<WebManager>
{
    public static string ClientPath { get; set; }
    
    protected override void OnSingletonAwake()
    {
        ClientPath = Path.Combine(Application.persistentDataPath, "Client");
    }

    public void WebRequestGet(string url, System.Action<UnityWebRequest> resultAction)
    {
        StartCoroutine(Get(url, resultAction));
    }

    public IEnumerator Get(string url, System.Action<UnityWebRequest> resultAction)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.timeout = 6;
            var p = request.SendWebRequest();
            Debug.Log("Server responded: " + request.downloadHandler.text);
            while (!p.isDone)
            {
                yield return null;
            }

            var responseCode = (int)request.responseCode;
            var responseHeaders = request.GetResponseHeaders();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("request Success");

                FileUtil.CreateDirectory(ClientPath);
                File.WriteAllBytes(ClientPath, request.downloadHandler.data);
            }
            else
            {
                Debug.Log($"request.error {request.error}");
                if (request.error == "Request timeout")
                {   //е╦юс ╬ф©Т
                    //fNetTimeout(fRetryRequest, _sendMessage);
                }
            }
            resultAction?.Invoke(request);
        }
    }
}
