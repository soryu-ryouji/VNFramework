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
                if (Directory.Exists(coupleFolder.Item1))
                {
                    CopyFolderFilesToTargetFolder(coupleFolder.Item1, coupleFolder.Item2);
                }
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

                if (File.Exists(exportPath))
                {
                    // 检查 resourcePath 和 exportPath 是否引用了相同的文件
                    if (!FileCompare(resourcePath, exportPath))
                    {
                        // 删除 exportPath 的文件
                        File.Delete(exportPath);

                        // 复制 resourcePath 的文件到 exportPath
                        File.Copy(resourcePath, exportPath);
                    }
                }
                else
                {
                    // 如果 exportPath 不存在，直接复制 resourcePath 的文件到 exportPath
                    File.Copy(resourcePath, exportPath);
                }
            }
        }

        private static bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;

            using (FileStream fs1 = new FileStream(file1, FileMode.Open))
            using (FileStream fs2 = new FileStream(file2, FileMode.Open))
            {
                if (fs1.Length != fs2.Length)
                {
                    return false;
                }

                do
                {
                    file1byte = fs1.ReadByte();
                    file2byte = fs2.ReadByte();
                } while ((file1byte == file2byte) && (file1byte != -1));

                return (file1byte - file2byte) == 0;
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