using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebManager : Singleton<WebManager>
{
    public static string ClientPath { get; set; }

    private readonly int maxRetryCnt = 3;
    private int retryCnt;

    private Coroutine coroutine;
    protected override void OnSingletonAwake()
    {
        ClientPath = Path.Combine(Application.persistentDataPath, "Client");
    }

    public void WebRequestGet(string url, System.Action<UnityWebRequest> resultAction)
    {
        retryCnt = 0;
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Get(url, resultAction));
    }
    public void RetryGet(string url, System.Action<UnityWebRequest> resultAction)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Get(url, resultAction));
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

            try
            {
                if (retryCnt <= maxRetryCnt)
                {
                    throw new System.Exception("test");
                }
                
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("request Success");

                    FileUtil.CreateDirectory(ClientPath);
                    File.WriteAllBytes(ClientPath, request.downloadHandler.data);
                    resultAction?.Invoke(request);
                }
                else
                {
                    Debug.Log($"request.error {request.error}");
                    if (request.error == "Request timeout")
                    {   //е╦юс ╬ф©Т
                        //fNetTimeout(fRetryRequest, _sendMessage);
                        retryCnt++;
                        RetryGet(url, resultAction);
                    }
                    else
                    {
                        resultAction?.Invoke(request);
                    }
                }
            }
            catch
            {
                retryCnt++;
                Debug.LogError($"request.error {request.error}");
                RetryGet(url, resultAction);
            }      
        }
    }
}
