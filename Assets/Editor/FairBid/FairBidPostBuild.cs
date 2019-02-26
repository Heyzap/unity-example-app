using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.IO;
using System.Diagnostics;

public class FairBidPostBuild : MonoBehaviour {
    [PostProcessBuild(101)]
    private static void OnPostProcessBuildPlayer(BuildTarget target, string pathToBuiltProject) {
        if (target == BuildTarget.iOS) {
            UnityEngine.Debug.Log ("FairBid: started post-build script");

            // grab the path to the postProcessor.py file
            var scriptPath = Path.Combine(Application.dataPath, "Editor/FairBid/PostprocessBuildPlayer.py");
            
            // sanity check
            if (!File.Exists(scriptPath)) {
                UnityEngine.Debug.LogError("FairBid post builder couldn't find python file. Did you accidentally delete it?");
                return;
            } else {
                var args = string.Format( "\"{0}\" \"{1}\"", scriptPath, pathToBuiltProject);
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "python2.7",
                        Arguments = args,
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        CreateNoWindow = true
                    }
                };

                UnityEngine.Debug.Log(string.Format("FairBid: starting HeyzapPostprocessBuildPlayer with args: {0}", args));
                proc.Start();
                proc.WaitForExit();
                if (proc.ExitCode > 0) {
                    UnityEngine.Debug.LogError("FairBid post-build script had an error(code=" + proc.ExitCode + "). See the editor log for more info & email a copy of it to support@heyzap.com for more help.");
                }

                UnityEngine.Debug.Log("FairBid: Finished post-build script.");
            }
        }
    }
}
