using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AutoSetABLabel
{
    [MenuItem("VNFrameworkTools/Set AB Label")]
    public static void SetABLabel()
    {
        string resPath = Path.Combine(Application.dataPath, "VNFramework", "Resources");
        string prefabPath = Path.Combine(resPath, "Prefabs");
        string vnScriptPath = Path.Combine(resPath, "VNScripts");
        string spritePath = Path.Combine(resPath, "Sprites");
        string soundPath = Path.Combine(resPath, "Sounds");
        string projectDataPath = Path.Combine(resPath, "ProjectData");

        List<(string path, string label)> items = new()
        {
            (vnScriptPath, "vnscript"),
            (prefabPath, "prefab"),
            (spritePath, "sprite"),
            (soundPath, "sound"),
            (projectDataPath, "projectdata")
        };

        foreach (var item in items)
        {
            SetAssetBundleLabel(item.path, item.label);
        }

        Debug.Log("Finish AB Label Setting");
    }

    private static void SetAssetBundleLabel(string folderPath, string label)
    {
        string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            if (file.EndsWith(".meta")) continue;

            AssetImporter importer = AssetImporter.GetAtPath("Assets" + file.Substring(Application.dataPath.Length));
            if (importer != null)
            {
                importer.assetBundleName = label;
            }
        }
    }
}