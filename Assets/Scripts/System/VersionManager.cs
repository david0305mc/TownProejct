using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleSaveItem
{
    public string fileName;
    public string hash;
    public long capacity;
    public bool IsLocal = false;
}

public class VersionManager : MonoBehaviour
{
    ////////////////////////////////////////////////////////////
    /// ¸â¹ö º¯¼ö
    private static VersionManager m_Inst = null;

    public static VersionManager Inst
    {
        get
        {
            if (m_Inst == null)
                m_Inst = new VersionManager();

            return m_Inst;
        }
    }
}
