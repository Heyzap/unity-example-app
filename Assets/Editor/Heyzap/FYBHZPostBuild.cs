using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.IO;
using System.Diagnostics;

public class FYBHZPostBuild : MonoBehaviour
{
    [PostProcessBuild(102)]
    private static void onPostProcessBuildPlayer( BuildTarget target, string pathToBuiltProject )
    {
#if UNITY_5 || UNITY_5_3_OR_NEWER
        if (target == BuildTarget.iOS)
#else
        if (target == BuildTarget.iPhone)
#endif
        {
            UnityEngine.Debug.Log ("FYBHZ: started post-build script");

            // grab the path to the postProcessor.py file
            var scriptPath = Path.Combine( Application.dataPath, "Editor/Heyzap/FYBHZPostprocessBuildPlayer.py" );
            var pathToEmbeddedFrameworkResources = Path.Combine( Application.dataPath, "Plugins/iOS/Heyzap/FYBHZMediationTestSuiteEF.embeddedframework/Resources" );
            
            // sanity check
            if( !File.Exists( scriptPath ) ) {
                UnityEngine.Debug.LogError( "FYBHZ post builder couldn't find python file. Did you accidentally delete it?" );
                return;
            } else if ( !Directory.Exists( pathToEmbeddedFrameworkResources ) ) {
                UnityEngine.Debug.LogError( "FYBHZ post builder couldn't find the .embeddedframework's Resources directory. Did you accidentally delete it?" );
            } else {
                var args = string.Format( "\"{0}\" \"{1}\" \"{2}\"", scriptPath, pathToBuiltProject, pathToEmbeddedFrameworkResources );
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "python2.6",
                        Arguments = args,
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        CreateNoWindow = true
                    }
                };

                UnityEngine.Debug.Log(string.Format("FYBHZ: starting FYBHZPostprocessBuildPlayer with args: {0}", args));
                proc.Start();
                proc.WaitForExit();
                if (proc.ExitCode > 0) {
                    UnityEngine.Debug.LogError("FYBHZ post-build script had an error(code=" + proc.ExitCode + "). See the editor log for more info & email a copy of it to support@heyzap.com for more help.");
                }

                UnityEngine.Debug.Log( "FYBHZ: Finished post-build script." );
            }
        }
    }
}