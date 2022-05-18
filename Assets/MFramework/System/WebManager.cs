using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Encryption;
using System.Security.Cryptography;

public enum ResultCode
{
    PARSE_ERROR = -32700,
    INVALID_REQUEST = -32600,
    METHOD_NOT_FOUND = -32601,
    INVALID_PARAMS = -32602,
    INTERNAL_ERROR = -32603,
    SERVER_ERROR = -32000,
    DATABASE_ERROR = -32009,
    BANNED_IP = -32444,
    DECRYPT_VALIDATE_FAIL = -32007,
    SUCCESS = 0,
    FAIL = 1,
    CLIENT_ASSERT = 10000000,
}

public class WebManager : Singleton<WebManager>
{
    public enum RequestType
    {
        POST,
        GET,
    }

    public class WEB_MSG
    {
        public string Url;
        public Action<byte[]> SuccessAction;
        public Action<UnityWebRequest> FailAction;
        public byte[] SendData;
        public RequestType type;
        public bool Encrypt;
    }

    private int retryCnt;
    private List<WEB_MSG> waitList = new List<WEB_MSG>();
    private Coroutine coroutine;
    private readonly int autoRetryCount = 3;

    public const string UrlEntryDev = "http://172.20.102.88:9001/server.php";
    private const int LenIV = 16;
    private const int LenHash = 32;
    static readonly System.Random rand = new System.Random();
    static byte[] volatilitySaltedKey = new byte[32];
    static readonly byte[] saltedKey = Encoding.UTF8.GetBytes(GetSalt());

    static string GetKeyLeft() => "f749054868424f2d";
    static string GetKeyRight() => "9e382ee359da31eb";

    static string GetSalt()
    {
        return GetKeyLeft() + GetKeyRight();
    }


    // -------------- Log
    public delegate void LogDelegate(string line);
    public static LogDelegate OnNewLog;
    public static void AddLog(string text)
    {
#if UNITY_EDITOR
        if (OnNewLog != null) OnNewLog(text);
#endif
    }
    // --------------

    public ResultCode Validation<T>(byte[] data, string pid, out T content)
    {
        content = default;
        var json = Encoding.UTF8.GetString(data);
        OnNewLog?.Invoke($"RS : {json}");

        // Error check
        if (-1 < json.IndexOf("\"error\""))
        {
            if (!TryDeserialize(json, out ErrorPacket packError))
                return ResultCode.PARSE_ERROR;

            if (packError.error.code != ResultCode.SUCCESS)
            {
                return packError.error.code;
            }
        }

        if (!TryDeserialize(json, out ResponsePacket packet))
        {
            return ResultCode.PARSE_ERROR;
        }

        // ID check
        if (string.Compare(packet.id, pid, false) != 0)
        {
            return ResultCode.CLIENT_ASSERT;
        }

        if (!TryDeserialize<T>(packet.result, out content))
        {
            return ResultCode.PARSE_ERROR;
        }
        return ResultCode.SUCCESS;

    }

    public bool TryDeserialize<T>(JToken jObj, out T result)
    {
        try { result = jObj.ToObject<T>(); }
        catch (Exception error)
        {
            Debug.LogFormat($"{nameof(TryDeserialize)}<{typeof(T).Name}>", error);
            result = default;
            return false;
        }
        return true;
    }


    public bool TryDeserialize<T>(string json, out T result)
    {
        try { result = JsonConvert.DeserializeObject<T>(json); }
        catch (Exception error)
        {
            Debug.LogFormat($"{nameof(TryDeserialize)}<{typeof(T).Name}>", error);
            result = default;
            return false;
        }
        return true;
    }

    private void RequestFailed(WEB_MSG msg, UnityWebRequest request)
    {
        if (request.error == "Request timeout")
        {
            if (retryCnt >= autoRetryCount)
            {
                Debug.Log("Timeout Retry");
            }
            else
            {
                retryCnt++;
                TryRequest(msg);
            }
        }
        else if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("NETWORK_NOTCONNECT");
            CompleteRequest();
        }
        else if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("ProtocolError");
            CompleteRequest();
        }
        else
        {
            Debug.Log("Unknown Network Error");
            CompleteRequest();
        }
    }

    public void WebRequestPost<T>(string url, byte[] sendData, string pid, Action<T> successAction, Action<UnityWebRequest> failAction = null, bool encrypt = true)
    {
        var msg = new WEB_MSG() { Url = url, SendData = sendData, type = RequestType.POST, Encrypt = encrypt };
        WebRequest(msg, response =>
        {
            var resultType = Validation(response, pid, out T content);
            switch (resultType)
            {
                case ResultCode.SUCCESS:
                    {
                        successAction(content);
                    }
                    break;
                default:
                    {
                        Debug.Log(resultType);
                    }
                    break;
            }
        }, failAction);
    }

    public void WebRequestGet(string url, Action<byte[]> successAction, System.Action<UnityWebRequest> failAction = null)
    {
        var msg = new WEB_MSG() { Url = url, type = RequestType.GET, Encrypt = false };
        WebRequest(msg, successAction, failAction);
    }

    private void WebRequest(WEB_MSG msg, System.Action<byte[]> successAction, System.Action<UnityWebRequest> failAction = null)
    {
        msg.SuccessAction = (param) => {
            try
            {
                successAction(param);
            }
            catch (Exception error)
            {
                Debug.LogError(error.Message);
            }
        };
        msg.FailAction = (requst) =>
        {
            try
            {
                failAction(requst);
                RequestFailed(msg, requst);
            }
            catch (Exception error)
            {
                Debug.LogError(error.Message);
            }
        };

        if (waitList.Count == 0)
        {
            StartWebRequest(msg);
        }
        else
        {
            waitList.Add(msg);
        }
    }
    private void StartWebRequest(WEB_MSG msg)
    {
        retryCnt = 0;
        TryRequest(msg);
    }

    private void TryRequest(WEB_MSG msg)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        if (msg.type == RequestType.GET)
        {
            coroutine = StartCoroutine(Get(msg));
        }
        else
        {
            coroutine = StartCoroutine(Post(msg));
        }
    }

    private void CompleteRequest()
    {
        if (waitList.Count > 0)
        {
            StartWebRequest(waitList[0]);
            waitList.RemoveAt(0);
        }
    }
    public static string GetPacketID() => UnityEngine.Random.Range(int.MinValue, int.MaxValue).ToString("x");


    // Encryption
    public static byte[] Encrypt(byte[] data)
    {
        var iv = new byte[LenIV];
        rand.NextBytes(iv);

        var payload = AES.Encrypt(AES.KeySizeType.L256, volatilitySaltedKey, iv, data);

        byte[] hash;
        using (var sah256 = new HMACSHA256(volatilitySaltedKey))
            hash = sah256.ComputeHash(payload);

        var bin = new byte[LenIV + LenHash + payload.Length];
        {
            var offset = iv.BlockCopy(bin, 0);
            offset += hash.BlockCopy(bin, offset);
            payload.BlockCopy(bin, offset);
        }

        return bin;
    }
    public static byte[] Decrypt(string base64encoded, bool isStaticSalt = true)
    {
        byte[] ret = null;

        try
        {
            var plainBin = Convert.FromBase64String(base64encoded);
            ret = Decrypt(plainBin, isStaticSalt);
            //WebAPI.AddLog(Encoding.UTF8.GetString(ret));
        }
        catch (FormatException e)
        {
            //WebAPI.AddLog("decrypt fail : base64encoded str = " + base64encoded);
        }

        return ret;
    }
    public static byte[] Decrypt(byte[] data, bool isStaticSalt = true)
    {
        byte[] iv = new byte[LenIV];
        byte[] hash = new byte[LenHash];
        byte[] payload = new byte[data.Length - LenIV - LenHash];

        var offset = data.BlockCopy(0, iv, 0, LenIV);
        offset += data.BlockCopy(offset, hash, 0, LenHash);
        data.BlockCopy(offset, payload, 0, payload.Length);
        // TODO : Check hash
        var result = AES.Decrypt(AES.KeySizeType.L256, isStaticSalt ? saltedKey : volatilitySaltedKey, iv, payload);

        return result;
    }

    private IEnumerator Post(WEB_MSG msg)
    {
        var pid = GetPacketID();

        var base64Bytes = msg.SendData;
        if (msg.Encrypt)
        {
            var sendDataEncrypted = Encrypt(base64Bytes);
            var base64 = Convert.ToBase64String(sendDataEncrypted);
            base64Bytes = Encoding.UTF8.GetBytes(base64);
        }

        using (var request = new UnityWebRequest(UrlEntryDev,
                                  UnityWebRequest.kHttpVerbPOST,
                                  new DownloadHandlerBuffer(),
                                  new UploadHandlerRaw(base64Bytes) { contentType = "application/json" }))
        {

            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                yield return null;
            }

            var responseCode = (int)request.responseCode;
            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string resString = Encoding.Default.GetString(request.downloadHandler.data);
                    if (msg.Encrypt)
                    {
                        msg.SuccessAction?.Invoke(Decrypt(resString, false));
                    }
                    else
                    {
                        msg.SuccessAction?.Invoke(request.downloadHandler.data);
                    }
                }
                catch (Exception error)
                {
                    Debug.LogError("Response Logic Error");
                }
                finally
                {
                    CompleteRequest();
                }
            }
            else
            {
                Debug.LogError("Response Error");
                msg.FailAction?.Invoke(request);
            }
        }
    }

    private IEnumerator Get(WEB_MSG msg)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(msg.Url))
        {
            request.timeout = 30;
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                yield return null;
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                msg.SuccessAction?.Invoke(request.downloadHandler.data);
            }
            else
            {
                Debug.LogError("Error");
                msg.FailAction?.Invoke(request);
            }
        }
    }
}
