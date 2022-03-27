using System.Collections;
using System.Collections.Generic;
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
}
