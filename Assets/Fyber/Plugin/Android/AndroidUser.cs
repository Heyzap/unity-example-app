using UnityEngine;

namespace FyberPlugin
{
#if UNITY_ANDROID && !UNITY_EDITOR
	public partial class User
	{

		static protected void NativePut(string json)
		{
			using (AndroidJavaObject UserObject = new AndroidJavaObject("com.fyber.unity.user.UserWrapper")) 
				UserObject.CallStatic("put", json);
				
		}

		static protected string GetJsonMessage(string key)
		{
			using (AndroidJavaObject UserObject = new AndroidJavaObject("com.fyber.unity.user.UserWrapper"))
				return UserObject.CallStatic<string>("get", key);
		}

	}
#endif
}

