using UnityEngine;
using UnityEditor;
using System.IO;

namespace VNFramework
{
    public class VNResourcesManager : MonoBehaviour
    {
        [MenuItem("VNFrameworkTools/Load VN Resources")]
        public static void LoadVNResources()
        {
            string resourcesPath = File.ReadAllText(Path.Combine(Application.dataPath, "EditorConfig", "VNFrameworkToolsSetting.txt"));
            if (!Directory.Exists(resourcesPath))
            {
                Debug.LogError($"Resources Folder Not Exist : {resourcesPath}");
                return;
            }

            string targetFolderPath = Path.Combine(Application.dataPath, "VNFramework", "Resources");

            (string, string)[] resFolderPaths = {
                (Path.Combine(resourcesPath, "Fonts"),Path.Combine(Application.dataPath, "VNFramework", "Fonts")),
                (Path.Combine(resourcesPath, "Resources", "ProjectData"),Path.Combine(targetFolderPath, "ProjectData")),
                (Path.Combine(resourcesPath, "Resources", "Sounds"), Path.Combine(targetFolderPath, "Sounds")),
                (Path.Combine(resourcesPath, "Resources", "Sprites"),Path.Combine(targetFolderPath, "Sprites")),
                (Path.Combine(resourcesPath, "Resources", "VNScripts"),Path.Combine(targetFolderPath, "VNScripts"))
            };

            foreach (var coupleFolder in resFolderPaths)
            {
                CopyFolderFilesToTargetFolder(coupleFolder.Item1, coupleFolder.Item2);
            }

            Debug.Log("Resources Loaded!");
        }

        private static void CopyFolderFilesToTargetFolder(string sourceFolderPath, string targetFolderPath)
        {
            string[] allResourcePaths = Directory.GetFiles(sourceFolderPath, "*", SearchOption.AllDirectories);

            foreach (string resourcePath in allResourcePaths)
            {
                string relativePath = GetRelativePath(resourcePath, sourceFolderPath);
                string exportPath = Path.Combine(targetFolderPath, relativePath);

                string exportDirectory = Path.GetDirectoryName(exportPath);
                Directory.CreateDirectory(exportDirectory);

                File.Copy(resourcePath, exportPath, true);
            }
        }

        [MenuItem("VNFrameworkTools/Export VN Resources")]
        public static void ExportVNResources()
        {
            string exportFolderPath = VNFrameworkToolsSetting.ExportVNFrameworkResPath;
            if (!Directory.Exists(exportFolderPath))
            {
                Debug.LogError("Export Folder Not Exist");
                return;
            }

            var resFolderPath = Path.Combine(Application.dataPath, "VNFramework", "Resources");
            var fontFolderPath = Path.Combine(Application.dataPath, "VNFramework", "Fonts");

            string[] resExportDir =
            {
                "Sounds",
                "VNScripts",
                "Sprites",
                "ProjectData"
            };

            foreach (var dir in resExportDir)
            {
                ExportResources(Path.Combine(resFolderPath, dir), Path.Combine(exportFolderPath, "Resources", dir));
            }

            ExportResources(fontFolderPath, Path.Combine(exportFolderPath, "Fonts"));

            Debug.Log("Resources Exported!");
        }

        private static void ExportResources(string sourceFolderPath, string exportFolderPath)
        {
            string[] allResourcePaths = Directory.GetFiles(sourceFolderPath, "*", SearchOption.AllDirectories);

            foreach (string resourcePath in allResourcePaths)
            {
                string relativePath = GetRelativePath(resourcePath, sourceFolderPath);
                string exportPath = Path.Combine(exportFolderPath, relativePath);

                string exportDirectory = Path.GetDirectoryName(exportPath);
                Directory.CreateDirectory(exportDirectory);

                File.Copy(resourcePath, exportPath, true);
            }
        }

        private static string GetRelativePath(string path, string basePath)
        {
            return Path.GetFullPath(path).Substring(Path.GetFullPath(basePath).Length + 1);
        }
    }
}