using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections.Generic;

namespace FyberEditor
{
	[InitializeOnLoad]
	public class FyberReloadScript : AssetPostprocessor 
	{

		private static int counter = 0 ;
		private static Queue<string> bundlesToRemove = new Queue<string>();

		static FyberReloadScript()
		{
			EditorApplication.update += Update;
			FyberEditorSettings.AndroidBundlesChanged += RestartCounter;
		}
		
		static void Update()
		{
			if (Application.isPlaying)
			{
				EditorApplication.update -= Update;
				FyberEditorSettings.AndroidBundlesChanged -= RestartCounter;
			}
			else if (FyberEditorSettings.Instance.shouldAutogenerateAssets)
			{
				if ( counter > 0 )
				{
					counter--;
				} 
				else if (counter == 0)
				{
					FyberEditorSettings.Instance.ProcessAndroidAdapters();

					if (bundlesToRemove.Count > 0)
					{
						while (bundlesToRemove.Count > 0)
				        {
							string bundle = bundlesToRemove.Dequeue();
							AndroidBundlesProcessor.RemoveBundleFiles(bundle);
						}
						AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
					}
					counter = -10;
				}
			}
		}

		static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) 
		{
			var needsUpdate = false;

			foreach (var str in importedAssets)
			{
				if (str.Contains("Fyber") && str.Contains("Mediation"))
				{
					needsUpdate = true;
					break;
				}
			}

			foreach (var str in deletedAssets)
			{
				if (str.Contains("Fyber") && str.Contains("Mediation"))
				{
					needsUpdate = true;
					if (str.EndsWith("natives"))
					{
						var path = new DirectoryInfo(str);
						bundlesToRemove.Enqueue(path.Parent.Name);
					}

				}

			}

			if (needsUpdate)
				RestartCounter();
		}

		internal static void RestartCounter()
		{
			counter = 500;
		}

	}
}
