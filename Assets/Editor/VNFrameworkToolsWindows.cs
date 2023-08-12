using UnityEditor;
using UnityEngine;
using System.IO;

public class VNFrameworkToolsSetting : EditorWindow
{
    public static string ExportVNFrameworkResPath;

    private static readonly string settingDirPath = Path.Combine(Application.dataPath, "EditorConfig");
    private static readonly string settingFilePath = Path.Combine(settingDirPath, "VNFrameworkToolsSetting.txt");

    private static readonly string DefaultExportPath = Path.Combine(Application.dataPath, "ExportRes");

    [MenuItem("VNFrameworkTools/Setting")]
    public static void ShowWindow()
    {
        // 当配置文件夹或配置文件不存在时，创建配置文件夹和配置文件
        if (!Directory.Exists(settingDirPath)) Directory.CreateDirectory(settingDirPath);
        if (!File.Exists(settingFilePath)) File.WriteAllText(settingFilePath, DefaultExportPath);

        ExportVNFrameworkResPath = File.ReadAllText(settingFilePath);
        VNFrameworkToolsSetting window = GetWindow<VNFrameworkToolsSetting>();
        // 设置窗口最小尺寸
        window.minSize = new Vector2(300, 200);
    }

    void OnGUI()
    {
        GUILayout.Label("Resources Export Path", EditorStyles.boldLabel);
        ExportVNFrameworkResPath = EditorGUILayout.TextField("Export Folder Path:", ExportVNFrameworkResPath);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Restore Default"))
        {
            ExportVNFrameworkResPath = DefaultExportPath;
            File.WriteAllText(settingFilePath, ExportVNFrameworkResPath);
        }

        if (GUILayout.Button("Save"))
        {
            File.WriteAllText(settingFilePath, ExportVNFrameworkResPath);
        }

        GUILayout.EndHorizontal();
    }
}