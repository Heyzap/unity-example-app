using System.Runtime.InteropServices;
using UnityEngine;
using System;

namespace FyberPlugin
{
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
	
	internal class PluginBridgeComponent : IPluginBridge
	{
		[DllImport ("__Internal")]
		static extern void _SetPluginParameters(string pluginVersion, string frameWorkVersion);

		[DllImport ("__Internal")]
		static extern void _Start(string json);
		
		[DllImport ("__Internal")]
		static extern bool _Cache(string action);
		
		[DllImport ("__Internal")]
		static extern void _Request(string json);
		
		[DllImport ("__Internal")]
		static extern void _Report(string json);
		
		[DllImport ("__Internal")]
		static extern string _Settings(string json);
		
		[DllImport ("__Internal")]
		static extern void _EnableLogging(bool shouldLog);
		
		[DllImport ("__Internal")]
		static extern void _SetLogLevel(int logLevel);
		
		[DllImport ("__Internal")]
		static extern void _StartAd(string json);

		[DllImport ("__Internal")]
		static extern Boolean _Banner(string json);

		static PluginBridgeComponent()
		{
			FyberGameObject.Init();
		}

		public void StartSDK(string json)	
		{	
			_SetPluginParameters(Fyber.Version, Application.unityVersion);
			_Start (json);
		}
		
		public bool Cache(string action)
		{	
			return _Cache(action);
		}
		
		public void Request(string json)
		{
			_Request(json);
		}
		
		public void Report(string json)	
		{	
			_Report(json);
		}
		
		public string Settings(string json)
		{
			return _Settings(json);
		}
		
		public void EnableLogging(bool shouldLog)
		{
			_EnableLogging(shouldLog);
		}

		public Boolean Banner(string json)
		{
			return _Banner(json);
		}

		public void StartAd(string json)
		{
			//_StartAd(json);
			FyberGameObject gameObject = GameObject.Find ("FyberGameObject").GetComponent<FyberGameObject>();
			gameObject.SkipFrameWithBlock(action =>  _StartAd(json));
		}

		public void GameObjectStarted()	
		{
			//DO NOTHING
		}

		public void ApplicationQuit()
		{
			//DO NOTHING
		}
	}
#endif	
}