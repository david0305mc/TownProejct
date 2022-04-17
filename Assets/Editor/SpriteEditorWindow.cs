using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpriteEditorWindow : EditorWindow
{

    public static SpriteCollection spriteCollection;
    [MenuItem("Window/Sprite Editor")]
    private static void Init()
    {
        EnsureSpriteCollection();
        // Get existing open window or if none, make a new one:
        SpriteEditorWindow window = (SpriteEditorWindow)EditorWindow.GetWindow(typeof(SpriteEditorWindow));
        window.minSize = new Vector2(800, 500);
        window.Show();
    }

    private static void EnsureSpriteCollection()
    {
        if (spriteCollection != null)
            return;

        spriteCollection = Resources.Load("SpriteCollection", typeof(SpriteCollection)) as SpriteCollection;
        if (spriteCollection == null)
        {
            SpriteCollection asset = ScriptableObject.CreateInstance<SpriteCollection>();
            string path = "Assets/Resources/SpriteCollection.asset";
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
