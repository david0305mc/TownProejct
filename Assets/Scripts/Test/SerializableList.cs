using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public class SerializableList<T> : Collection<T>, ISerializationCallbackReceiver
{
    [SerializeField]
    List<T> items;

    public void OnBeforeSerialize()
    {
        items = (List<T>)Items;
    }

    public void OnAfterDeserialize()
    {
        Clear();
        foreach (var item in items)
            Add(item);
    }

    public string ToJson(bool prettyPrint = false)
    {
        var result = "[]";
        var json = JsonUtility.ToJson(this, prettyPrint);
        var pattern = prettyPrint ? "^\\{\n\\s+\"items\":\\s(?<array>.*)\n\\s+\\]\n}$" : "^{\"items\":(?<array>.*)}$";
        var regex = new Regex(pattern, RegexOptions.Singleline);
        var match = regex.Match(json);
        if (match.Success)
        {
            result = match.Groups["array"].Value;
            if (prettyPrint)
                result += "\n]";
        }
        return result;
    }

    public static SerializableList<T> FromJson(string arrayString)
    {
        var json = "{\"items\":" + arrayString + "}";

        return JsonUtility.FromJson<SerializableList<T>>(json);
    }
}