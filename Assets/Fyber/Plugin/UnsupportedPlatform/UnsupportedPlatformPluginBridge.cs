using UnityEngine;

namespace FyberPlugin
{
#if UNITY_EDITOR || (!UNITY_IPHONE && !UNITY_IOS && !UNITY_ANDROID)
    internal class PluginBridgeComponent : IPluginBridge
	{
		
		public void StartSDK(string json)
		{
			Debug.Log("Version - " + Application.unityVersion);
			Utils.printWarningMessage();
		}

        public void Cache(string action)	
		{	
			Utils.printWarningMessage();
		}
		
		public void Request(string json)
		{
			Utils.printWarningMessage();
		}
		
		public void StartAd(string json)
		{
			Utils.printWarningMessage();
		}

		public bool Banner(string json)
		{
			Utils.printWarningMessage();
			return false;
		}

		public void Report(string json)	
		{	
			Utils.printWarningMessage();
		}
		
		public string Settings(string json)
		{
			Utils.printWarningMessage();
			return "no yet implemented";
        }

        public void EnableLogging(bool shouldLog)
        {
            Utils.printWarningMessage();
		}

		public void GameObjectStarted()	
		{
			Utils.printWarningMessage();
		}

    }
#endif
}
