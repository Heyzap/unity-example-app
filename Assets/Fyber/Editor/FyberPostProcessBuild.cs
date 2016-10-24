using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEditor.XCodeEditor.FyberPlugin;
using System.Text;
using System.IO;
using FyberPlugin.Editor;

public class FyberPostProcessBuild
{

    [PostProcessBuild(500)]
    public static void OnPostProcessBuild( BuildTarget target, string path )
    {
#if UNITY_IPHONE || UNITY_IOS
#if UNITY_5
        if (target == BuildTarget.iOS)
#else
        if (target == BuildTarget.iPhone)
#endif // UNITY_5
        {
            string folderPath = Path.Combine (Application.dataPath, "Fyber/iOS/fyber-sdk-compat-lib");
            if (Directory.Exists (folderPath)) {
                Directory.Delete (folderPath, true);
            }

            XCProject project = new XCProject(path);

            string libPath = Path.Combine (project.projectRootPath, "Libraries/Fyber/iOS/fyber-sdk-compat-lib");
            
            if (Directory.Exists (libPath)) {
                Directory.Delete (libPath, true);
            }

            // Find and run through all projmods files to patch the project
            string projModPath = System.IO.Path.Combine(Application.dataPath, "Fyber/iOS");
            string[] files = System.IO.Directory.GetFiles(projModPath, "*.projmods", System.IO.SearchOption.AllDirectories);
            foreach( var file in files ) 
            {
                project.ApplyMod(Application.dataPath, file);

                if (file.Contains("NativeX"))
                {
                    string unityVersionPlist = "<plist><key>name</key><string>Nativex</string><key>settings</key><dict><key>FYBNativeXUnityBuildFlag</key><true /></dict></plist>";
                    PlistUpdater.UpdatePlist(project.projectRootPath, unityVersionPlist);
                }
            }
            project.Save();
            
#if UNITY_5
            var compatGroup = "Libraries/Fyber/iOS/fyber-sdk-compat-lib";
            string[] filesToRemove = {"FYBBannerSize.h","FYBBannerView.h","libFyberSDKCompat.a"};

            UnityEditor.iOS.Xcode.PBXProject pbxProject = new UnityEditor.iOS.Xcode.PBXProject ();
            string pbxprojPath = UnityEditor.iOS.Xcode.PBXProject.GetPBXProjectPath(path);
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
#endif  //UNITY_IPHONE || UNITY_IOS
    }

#if UNITY_IPHONE || UNITY_IOS
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

