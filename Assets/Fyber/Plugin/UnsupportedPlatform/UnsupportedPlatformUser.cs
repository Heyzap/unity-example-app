namespace FyberPlugin
{
#if UNITY_EDITOR || (!UNITY_IPHONE && !UNITY_IOS && !UNITY_ANDROID)
    public partial class User
	{

		static protected void NativePut(string json)
		{
			Utils.printWarningMessage();
		}

		static protected string GetJsonMessage(string key)
		{
			Utils.printWarningMessage();
			return "{\"success\":false,\"error\":\"Unsupported platform\":\"key\":" + key + "}";
		}

	}
#endif
}
