using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MUtilities : MonoBehaviour
{
    public static GameObject CreateInstance(GameObject original, GameObject parent, bool isActive)
    {
        GameObject inst = Instantiate(original, parent.transform, false);
        inst.SetActive(isActive);
        return inst;
    }

    public static T CreatePopup<T>(GameObject original, GameObject parent, bool isActive) where T : MPopupBase
    {
        T inst = Instantiate(original, parent.transform, false).GetComponent<T>();
        if (inst == null)
        {
            Debug.LogError("inst == null");
            return null;
        }
        inst.gameObject.SetActive(isActive);
        return inst;
    }
    public static string CleanStringForFloat(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        if (Regex.Match(input, @"^-?[0-9]*(?:\.[0-9]*)?$").Success)
            return input;
        else
        {
            //Debug.Log("Error, Bad Float: " + input);
            return "0";
        }
    }

    public static string CleanStringForInt(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        if (Regex.Match(input, "([-+]?[0-9]+)").Success)
            return input;
        else
        {
            //Debug.Log("Error, Bad Int: " + input);
            return "0";
        }
    }
}
