
//
//  HeyzapAds.cs
//
//  Copyright 2015 Heyzap, Inc. All Rights Reserved
//
//  Permission is hereby granted, free of charge, to any person
//  obtaining a copy of this software and associated documentation
//  files (the "Software"), to deal in the Software without
//  restriction, including without limitation the rights to use,
//  copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following
//  conditions:
//
//  The above copyright notice and this permission notice shall be
//  included in all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//  OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
//  HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//  WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//  OTHER DEALINGS IN THE SOFTWARE.
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class HeyzapAds : MonoBehaviour {
  public delegate void NetworkCallbackListener( string network, string callback );
  private static NetworkCallbackListener networkCallbackListener;
  private static HeyzapAds _instance = null;
    
  public static int FLAG_NO_OPTIONS = 0 << 0;
  public static int FLAG_DISABLE_AUTOMATIC_FETCHING = 1 << 0;
  public static int FLAG_INSTALL_TRACKING_ONLY = 1 << 1;
  public static int AMAZON = 1 << 2;
  public static int DISABLE_FIRST_AUTOMATIC_FETCH = 1 << 3;
  public static int DISABLE_MEDIATION = 1 << 4;

  public class NetworkCallback {
    public static string INITIALIZED = "initialized";
    public static string SHOW = "show";
    public static string AVAILABLE = "available";
    public static string HIDE = "hide";
    public static string FETCH_FAILED = "fetch_failed";
    public static string CLICK = "click";
    public static string DISMISS = "dismiss";
    public static string INCENTIVIZED_RESULT_COMPLETE = "incentivized_result_complete";
    public static string INCENTIVIZED_RESULT_INCOMPLETE = "incentivized_result_incomplete";
    public static string AUDIO_STARTING = "audio_starting";
    public static string AUDIO_FINISHED = "audio_finished";
    public static string BANNER_LOADED = "banner-loaded";
    public static string BANNER_CLICK = "banner-click";
    public static string BANNER_HIDE = "banner-hide";
    public static string BANNER_DISMISS = "banner-dismiss";
    public static string BANNER_FETCH_FAILED = "banner-fetch_failed";
    public static string LEAVE_APPLICATION = "leave_application";

    // Facebook Specific
    public static string FACEBOOK_LOGGING_IMPRESSION = "logging_impression";
    
    // Chartboost Specific
    public static string CHARTBOOST_MOREAPPS_FETCH_FAILED = "moreapps-fetch_failed";
    public static string CHARTBOOST_MOREAPPS_HIDE = "moreapps-hide";
    public static string CHARTBOOST_MOREAPPS_DISMISS = "moreapps-dismiss";
    public static string CHARTBOOST_MOREAPPS_CLICK = "moreapps-click";
    public static string CHARTBOOST_MOREAPPS_SHOW = "moreapps-show";
    public static string CHARTBOOST_MOREAPPS_AVAILABLE = "moreapps-available";
    public static string CHARTBOOST_MOREAPPS_CLICK_FAILED = "moreapps-click_failed";
  }

  public class Network {
    public static string HEYZAP = "heyzap";
    public static string FACEBOOK = "facebook";
    public static string UNITYADS = "unityads";
    public static string APPLOVIN = "applovin";
    public static string VUNGLE = "vungle";
    public static string CHARTBOOST = "chartboost";
    public static string ADCOLONY = "adcolony";
    public static string ADMOB = "admob";
    public static string IAD = "iad";
  }

  public static void start(string publisher_id, int options) {
    #if !UNITY_EDITOR

    #if UNITY_ANDROID
    HeyzapAdsAndroid.start(publisher_id, options);
    #endif

    #if UNITY_IPHONE
    HeyzapAdsIOS.start(publisher_id, options);
    #endif

    HeyzapAds.initReceiver();
    HZInterstitialAd.initReceiver();
    HZVideoAd.initReceiver();
    HZIncentivizedAd.initReceiver();
    HZBannerAd.initReceiver();

    #endif
  }
  
  public static string getRemoteData(){
    #if UNITY_ANDROID
    return HeyzapAdsAndroid.getRemoteData();
    #elif UNITY_IPHONE && !UNITY_EDITOR
    return HeyzapAdsIOS.getRemoteData();
    #else 
    return "{}";
    #endif
  }

  public static void showMediationTestSuite() {
    #if UNITY_ANDROID
    HeyzapAdsAndroid.showMediationTestSuite();
    #endif

    #if UNITY_IPHONE && !UNITY_EDITOR
    HeyzapAdsIOS.showMediationTestSuite();
    #endif
  }
  
  
  public static Boolean onBackPressed() {
    #if UNITY_ANDROID
    return HeyzapAdsAndroid.onBackPressed();
    
    #elif UNITY_IPHONE && !UNITY_EDITOR
    return HeyzapAdsIOS.onBackPressed();
    
    #else
    return false;
    #endif  
}

  public static Boolean isNetworkInitialized(string network) {
    #if UNITY_ANDROID
    return HeyzapAdsAndroid.isNetworkInitialized(network);
    
    #elif UNITY_IPHONE && !UNITY_EDITOR
    return HeyzapAdsIOS.isNetworkInitialized(network);

    #else
    return false;
    #endif   
  }
  

  public static void setNetworkCallbackListener(NetworkCallbackListener listener) {
    networkCallbackListener = listener;
  }
    
  public void setNetworkCallbackMessage(string message) {
		string[] networkStateParams = message.Split(',');
		setNetworkCallback(networkStateParams[0], networkStateParams[1]); 
  }
	
  public static void setNetworkCallback(string network, string callback) {
  	if (networkCallbackListener != null) {
  		networkCallbackListener(network, callback);
  	}
  }
	
  public static void initReceiver(){
  	if (_instance == null) {
  		GameObject receiverObject = new GameObject("HeyzapAds");
  		DontDestroyOnLoad(receiverObject);
  		_instance = receiverObject.AddComponent<HeyzapAds>();
  	}
  }

  public static void pauseExpensiveWork() {
    #if UNITY_IPHONE && !UNITY_EDITOR
    HeyzapAdsIOS.pauseExpensiveWork();
    #endif
  }

  public static void resumeExpensiveWork() {
    #if UNITY_IPHONE && !UNITY_EDITOR
    HeyzapAdsIOS.resumeExpensiveWork();
    #endif
  }

  public static void showDebugLogs() {
    #if UNITY_ANDROID
    HeyzapAdsAndroid.showDebugLogs();
    #endif

    #if UNITY_IPHONE && !UNITY_EDITOR
    HeyzapAdsIOS.showDebugLogs();
    #endif
  }

  public static void hideDebugLogs() {
    #if UNITY_ANDROID
    HeyzapAdsAndroid.hideDebugLogs();
    #endif

    #if UNITY_IPHONE && !UNITY_EDITOR
    HeyzapAdsIOS.hideDebugLogs();
    #endif
  }
}

#if UNITY_IPHONE && !UNITY_EDITOR
public class HeyzapAdsIOS : MonoBehaviour {
  public static void start(string publisher_id, int options=0) {
    hz_ads_start_app(publisher_id, options);
  }

  [DllImport ("__Internal")]
  private static extern void hz_ads_start_app(string publisher_id, int flags);

  public static void showMediationTestSuite() {
    hz_ads_show_mediation_debug_view_controller();
  }
  
  public static Boolean onBackPressed(){
  	return false;
  }

  [DllImport ("__Internal")]
  private static extern void hz_ads_show_mediation_debug_view_controller();


  public static bool isNetworkInitialized(string network) {
    return hz_ads_is_network_initialized(network);
  }
  
  public static string getRemoteData(){
    return hz_ads_get_remote_data();
  }
  [DllImport ("__Internal")]
  private static extern string hz_ads_get_remote_data();

  [DllImport ("__Internal")]
  private static extern bool hz_ads_is_network_initialized(string network);

  public static void pauseExpensiveWork() {
    hz_pause_expensive_work();
  }

  public static void resumeExpensiveWork() {
    hz_resume_expensive_work();
  }

  [DllImport ("__Internal")]
  private static extern void hz_pause_expensive_work();

  [DllImport ("__Internal")]
  private static extern void hz_resume_expensive_work();

  public static void showDebugLogs() {
    hz_ads_show_debug_logs();
  }
    
  [DllImport ("__Internal")]
  private static extern void hz_ads_show_debug_logs();

  public static void hideDebugLogs() {
    hz_ads_hide_debug_logs();
  }
  
  [DllImport ("__Internal")]
  private static extern void hz_ads_hide_debug_logs();
}
#endif

#if UNITY_ANDROID
public class HeyzapAdsAndroid : MonoBehaviour {
  public static void start(string publisher_id, int options=0) {
    if(Application.platform != RuntimePlatform.Android) return;

    AndroidJNIHelper.debug = false; 
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
      jc.CallStatic("start", publisher_id, options);
    }
  }

  public static Boolean isNetworkInitialized(string network) {
    if (Application.platform != RuntimePlatform.Android) return false;

    AndroidJNIHelper.debug = false; 
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
      return jc.CallStatic<Boolean>("isNetworkInitialized", network);
    }
  }
  
  public static Boolean onBackPressed(){
    if(Application.platform != RuntimePlatform.Android) return false;

    AndroidJNIHelper.debug = false;
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
      return jc.CallStatic<Boolean>("onBackPressed");
    }
  }

  public static void showMediationTestSuite() {
    if(Application.platform != RuntimePlatform.Android) return;

    AndroidJNIHelper.debug = false;
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
      jc.CallStatic("showNetworkActivity");
    }
  }
  public static string getRemoteData() {
    if(Application.platform != RuntimePlatform.Android) return "{}";
    AndroidJNIHelper.debug = false;
    
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
      return jc.CallStatic<String>("getCustomPublisherData");
    }
  }

  public static void showDebugLogs() {
    if(Application.platform != RuntimePlatform.Android) return;
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
      jc.CallStatic("showDebugLogs");
    }
  }

  public static void hideDebugLogs() {
    if(Application.platform != RuntimePlatform.Android) return;
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
      jc.CallStatic("hideDebugLogs");
    }
  }
}
#endif
