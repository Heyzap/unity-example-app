using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FyberPlugin.LitJson;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FyberPlugin
{

#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public class FyberSettings : ScriptableObject
	{

		const string fyberSettingsAssetName = "FyberSettings";
		const string fyberSettingsPath = "Fyber/Resources";
		const string fyberSettingsAssetExtension = ".asset";
		
		private static FyberSettings instance;
		
#if UNITY_EDITOR		
		static FyberSettings()		
		{		
			GetInstance();		
		}		
#else	
		void OnEnable() 		 		
		{		 		
			GetInstance();		 		
		}		 		
#endif

		private static FyberSettings GetInstance()
		{
			if (instance == null)
			{
				PluginBridge.bridge = new PluginBridgeComponent();
				instance = Resources.Load(fyberSettingsAssetName) as FyberSettings;
				if (instance == null)
				{
					// If not found, autocreate the asset object.
					instance = CreateInstance<FyberSettings>();
#if UNITY_EDITOR
					string properPath = Path.Combine(Application.dataPath, fyberSettingsPath);
					if (!Directory.Exists(properPath))
					{
						AssetDatabase.CreateFolder("Assets/Fyber", "Resources");
					}
					
					string fullPath = Path.Combine(Path.Combine("Assets", fyberSettingsPath),
					                               fyberSettingsAssetName + fyberSettingsAssetExtension
					                               );
					instance.hideFlags = HideFlags.HideInInspector;
					AssetDatabase.CreateAsset(instance, fullPath);
#endif
				}
			}
			return instance;
		}

		public static FyberSettings Instance
		{
			get
			{
				return GetInstance();
			}
		}

		[SerializeField]
		[HideInInspector]
		private string bundlesJson;

		[SerializeField]
		[HideInInspector]
		private string configJson;

		[SerializeField]
		[HideInInspector]
		private int bundlesCount;

		internal string BundlesInfoJson()
		{
			return bundlesJson;
		}

		internal string BundlesConfigJson()
		{
			return configJson;
		}
		
		internal int BundlesCount()
		{
			return bundlesCount;
		}
	}

}
