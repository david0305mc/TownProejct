using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorMenu
{
    [MenuItem("Assets/GenerateTableCode")]
    public static void GenerateTableCode()
    {
        if (EditorApplication.isPlaying) return;
        CodeGenerator.GenDatatable();
        Debug.Log("GenerateTableCode");
        CodeGenerator.GenConfigTable();
        //Debug.Log("GenConfigTable");
    }

    [MenuItem("Assets/GenerateTableEnum")]
    public static void GenerateTableEnum()
    {
        if (EditorApplication.isPlaying) return;
        CodeGenerator.GenTableEnum();
        Debug.Log("GenerateTableEnum");
    }
    
    [MenuItem("Util/CleanCache")]
    public static void CleanCache()
    {
        if (Caching.ClearCache())
        {
            EditorUtility.DisplayDialog("알림", "캐시가 삭제되었습니다.", "확인");
        }
        else
        {
            EditorUtility.DisplayDialog("오류", "캐시 삭제에 실패했습니다.", "확인");
        }
    }
    
}
