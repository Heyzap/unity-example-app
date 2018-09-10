using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEditor.XCodeEditor.FyberPlugin;
using FyberEditor;
using System.Text;
using System.IO;
using System.Linq;
using FyberPlugin.Editor;

public class FyberPostProcessBuild
{

    [PostProcessBuild(500)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
#if UNITY_IPHONE || UNITY_IOS
      OniOSPostProcessBuild(path);
#endif
    }

#if UNITY_IPHONE || UNITY_IOS
    private static void OniOSPostProcessBuild(string path)
    {
        XCProject project = new XCProject(path);

        removeCompatibilityLibraryFromProject(project);
        applyProjmodsToProject(project);

        project.Save();
    }

    private static void applyProjmodsToProject(XCProject project)
    {
        // Find and run through all projmods files to patch the project
        string mediationProjmodsRootDir = System.IO.Path.Combine(Application.dataPath, "Fyber/iOS");
        string SDKProjmodRootDir = System.IO.Path.Combine(Application.dataPath, "Plugins/iOS");

        string[] SDKProjmodsFiles = System.IO.Directory.GetFiles(SDKProjmodRootDir, "*.projmods", System.IO.SearchOption.AllDirectories);
        string[] mediationProjmodsFiles = System.IO.Directory.GetFiles(mediationProjmodsRootDir, "*.projmods", System.IO.SearchOption.AllDirectories);
        string[] projmodsFiles = SDKProjmodsFiles.Concat(mediationProjmodsFiles).ToArray();

        foreach(var projmodFile in projmodsFiles)
        {
            Debug.Log ("Applying Modification to Project from projmod file: " + projmodFile);
            project.ApplyMod(Application.dataPath, projmodFile);

            if (projmodFile.Contains("NativeX"))
            {
                string unityVersionPlist = "<plist><key>name</key><string>Nativex</string><key>settings</key><dict><key>FYBNativeXUnityBuildFlag</key><true /></dict></plist>";
                PlistUpdater.UpdatePlist(project.projectRootPath, unityVersionPlist);
            }
        }
    }

    private static void removeCompatibilityLibraryFromProject(XCProject project)
    {
        string folderPath = Path.Combine (Application.dataPath, "Fyber/iOS/fyber-sdk-compat-lib");
        if (Directory.Exists (folderPath)) {
            Debug.Log (folderPath +" exists. Deleting it");
            Directory.Delete (folderPath, true);
        }

        string libPath = Path.Combine (project.projectRootPath, "Libraries/Fyber/iOS/fyber-sdk-compat-lib");
        if (Directory.Exists (libPath)) {
            Debug.Log (libPath +" exists. Deleting it");
            Directory.Delete (libPath, true);
        }

#if UNITY_5
        var compatGroup = "Libraries/Fyber/iOS/fyber-sdk-compat-lib";
        string[] filesToRemove = { "FYBBannerSize.h","FYBBannerView.h","libFyberSDKCompat.a" };

        UnityEditor.iOS.Xcode.PBXProject pbxProject = new UnityEditor.iOS.Xcode.PBXProject ();
        string pbxprojPath = UnityEditor.iOS.Xcode.PBXProject.GetPBXProjectPath(project.projectRootPath);
        pbxProject.ReadFromFile(pbxprojPath);

        foreach(string file in filesToRemove) {
            var fileToRemove = compatGroup + "/" + file;
            if(pbxProject.ContainsFileByProjectPath(fileToRemove)) {
                string guid = pbxProject.FindFileGuidByProjectPath(fileToRemove);
                pbxProject.RemoveFile (guid);
            }
        }

        pbxProject.WriteToFile(pbxprojPath);
#endif // UNITY_5
    }

    [PostProcessBuild(600)]
    public static void OnPostProcessBuildOrientationFix(BuildTarget target, string pathToBuildProject)
    {

        if (PlayerPrefs.GetInt ("FYBPostProcessBuild") == 0)
            return;

        StringBuilder newFile = new StringBuilder();

        string[] file = File.ReadAllLines(pathToBuildProject + "/Classes/UnityAppController.mm");

        for (int idx = 0; idx < file.Length; idx++)
        {
            string line = file[idx];
            if (line.Contains("- (NSUInteger)application:(UIApplication*)application supportedInterfaceOrientationsForWindow:(UIWindow*)window"))
            {
                // Calculate the length of the method
                int subIdx = 0;
                for (subIdx = idx; subIdx < file.Length; subIdx++)
                {
                    string subLine = file[subIdx];
                    if (subLine.Contains("}"))
                    {
                        break;
                    }
                }

                // Replace methods content
                newFile.Append(line + "\r\n");
                newFile.Append("{" + "\r\n");
                newFile.Append("\t" + PlayerPrefs.GetString ("FYBOrientationReturnValueKey") + "\r\n");
                newFile.Append("}" + "\r\n");

                // Move to the next method
                idx += (subIdx - idx);

                continue;
            }

            newFile.Append(line + "\r\n");
        }

        File.WriteAllText(pathToBuildProject + "/Classes/UnityAppController.mm", newFile.ToString());
    }
#endif  //UNITY_IPHONE || UNITY_IOS

}
