using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileUtil : MonoBehaviour
{

    public static void CreateDirectory(string path)
    {
        var dirPath = Path.GetDirectoryName(path);
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
    }


}
