using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class ErrorPacket
{
    [JsonProperty("id")] public string id;
    [JsonProperty("error")] public ResponseError error;

    public override string ToString()
    {
        return $"{nameof(id)}.{id}{Environment.NewLine}"
             + $"{nameof(error)}.{error}";
    }
}

[Serializable]
public class ResponseError
{
    static readonly StringBuilder sb = new StringBuilder();

    [JsonProperty("code")] public ResultCode code;
    [JsonProperty("file")] public string file;
    [JsonProperty("line")] public int line;
    [JsonProperty("message")] public string message;

    public override string ToString()
    {
        sb.Clear();
        {
            sb.Append($"{nameof(code)}.{code} ({(int)code}){Environment.NewLine}"
                    + $"{nameof(message)}.{message}{Environment.NewLine}"
                    + $"{nameof(file)}.{file} ({line}){Environment.NewLine}"
                    + $"{{{Environment.NewLine}");

            sb.Append("}");
        }
        var result = sb.ToString();
        sb.Clear();
        return result;
    }
}

[Serializable]
public class ResponsePacket
{
    [JsonProperty("id")] public string id;
    [JsonProperty("result")] public JToken result;
    [JsonProperty("runtime")] public double runtime;

    public override string ToString()
    {
        return $"{nameof(id)}.{id}{Environment.NewLine}"
             + $"{nameof(result)}.{result}{Environment.NewLine}"
             + $"{nameof(runtime)}.{runtime}";
    }
}