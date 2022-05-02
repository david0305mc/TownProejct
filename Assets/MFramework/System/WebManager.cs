using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public struct WEB_MSG
{
    public string Url;
    public System.Action<UnityWebRequest> SuccessAction;
    public System.Action<UnityWebRequest> FailAction;
}

public class WebManager : Singleton<WebManager>
{
    public static string ClientPath { get; set; }

    private readonly int maxRetryCnt = 5;
    private int retryCnt;
    private List<WEB_MSG> requestList = new List<WEB_MSG>();

    private Coroutine coroutine;
    protected override void OnSingletonAwake()
    {
        ClientPath = Path.Combine(Application.persistentDataPath, "Client");
    }

    public void WebRequestGet(string url, System.Action<UnityWebRequest> resultAction, System.Action<UnityWebRequest> failAction = null)
    {
        Debug.Log($"requestList.Count  {requestList.Count }");
        var msg = new WEB_MSG() { Url = url, SuccessAction = resultAction, FailAction = failAction };
        requestList.Add(msg);
        if (requestList.Count == 1)
        {
            WebRequestGet(msg);
        }
    }
    private void WebRequestGet(WEB_MSG msg)
    {
        retryCnt = 0;
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Get(msg));
    }

    private void RetryGet(WEB_MSG msg)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Get(msg));
    }

    private void CompleteRequest()
    {
        requestList.RemoveAt(0);
        if (requestList.Count > 0)
        {
            WebRequestGet(requestList[0]);
        }
    }
    private IEnumerator Get(WEB_MSG msg)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(msg.Url))
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
                msg.SuccessAction?.Invoke(request);
                CompleteRequest();
            }
            else
            {
                Debug.Log($"request.error {request.error}");
                if (request.error == "Request timeout")
                {
                    if (retryCnt >= maxRetryCnt)
                    {
                        msg.FailAction?.Invoke(request);
                        CompleteRequest();
                    }
                    else
                    {
                        retryCnt++;
                        RetryGet(msg);
                    }
                }
                else
                {
                    msg.FailAction?.Invoke(request);
                    CompleteRequest();
                }
            }
        }
    }
}
