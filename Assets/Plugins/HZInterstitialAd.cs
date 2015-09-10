//
//  HZInterstitialAd.cs
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

public class HZInterstitialAd : MonoBehaviour {

  public delegate void AdDisplayListener( string state, string tag );
  private static AdDisplayListener adDisplayListener;
  private static HZInterstitialAd _instance = null;
  
  public static void show(string tag="default") {
    #if UNITY_ANDROID
    HZInterstitialAdAndroid.show(tag);
    #endif

    #if UNITY_IPHONE && !UNITY_EDITOR
    HZInterstitialAdIOS.show(tag);
    #endif
  }
  
  public static void fetch(string tag="default") {
    #if UNITY_ANDROID
    HZInterstitialAdAndroid.fetch(tag);
    #endif

    #if UNITY_IPHONE && !UNITY_EDITOR
    HZInterstitialAdIOS.fetch(tag);
    #endif
  }
  
  public static bool isAvailable(string tag="default") {
    #if UNITY_ANDROID
    return HZInterstitialAdAndroid.isAvailable(tag);
    #elif UNITY_IPHONE && !UNITY_EDITOR
    return HZInterstitialAdIOS.isAvailable(tag);
    #else
    return false;
    #endif
  }

	public static void chartboostFetchForLocation(string location) {
		#if UNITY_ANDROID
		HZInterstitialAdAndroid.chartboostFetchForLocation(location);
		#elif UNITY_IPHONE && !UNITY_EDITOR
		HZInterstitialAdIOS.chartboostFetchForLocation(location);
		#else
		#endif
	}

  public static bool chartboostIsAvailableForLocation(string location) {
		#if UNITY_ANDROID
		return HZInterstitialAdAndroid.chartboostIsAvailableForLocation(location);
		#elif UNITY_IPHONE && !UNITY_EDITOR
		return HZInterstitialAdIOS.chartboostIsAvailableForLocation(location);
		#else
		return false;
		#endif
  }

	public static void chartboostShowForLocation(string location) {
		#if UNITY_ANDROID
		HZInterstitialAdAndroid.chartboostShowForLocation(location);
		#elif UNITY_IPHONE && !UNITY_EDITOR
		HZInterstitialAdIOS.chartboostShowForLocation(location);
		#else
		#endif
	}

  public static void initReceiver(){
    if (_instance == null) {
      GameObject receiverObject = new GameObject("HZInterstitialAd");
      DontDestroyOnLoad(receiverObject);
      _instance = receiverObject.AddComponent<HZInterstitialAd>();
    }
  }

  public static void setDisplayListener(AdDisplayListener listener) {
    adDisplayListener = listener;
  }
    
  public void setDisplayState(string message) {
    string[] displayStateParams = message.Split(',');
    setDisplayStates(displayStateParams[0], displayStateParams[1]); 
  }
  
  public static void setDisplayStates(string state, string tag) {
    if (adDisplayListener != null) {
      adDisplayListener(state, tag);
    }
  }
}

#if UNITY_IPHONE && !UNITY_EDITOR
public class HZInterstitialAdIOS : MonoBehaviour {

  public static void show(string tag="default") {
    hz_ads_show_interstitial(tag);
  }

  [DllImport ("__Internal")]
  private static extern void hz_ads_show_interstitial(string tag);

  public static void fetch(string tag="default") {
    hz_ads_fetch_interstitial(tag);
  }

  [DllImport ("__Internal")]
  private static extern void hz_ads_fetch_interstitial(string tag);

  public static bool isAvailable(string tag="default") {
    return hz_ads_interstitial_is_available(tag);
  }

  [DllImport ("__Internal")]
  private static extern bool hz_ads_interstitial_is_available(string tag);

	// Chartboost functions

  public static void chartboostFetchForLocation(string location) {
    hz_fetch_chartboost_for_location(location);
  }

  [DllImport ("__Internal")]
  private static extern void hz_fetch_chartboost_for_location(string location);

  public static bool chartboostIsAvailableForLocation(string location) {
    return hz_chartboost_is_available_for_location(location);
  }

  [DllImport ("__Internal")]
  private static extern bool hz_chartboost_is_available_for_location(string location);

  public static void chartboostShowForLocation(string location) {
    hz_show_chartboost_for_location(location);
  }

  [DllImport ("__Internal")]
  private static extern void hz_show_chartboost_for_location(string location);

}
#endif

#if UNITY_ANDROID
public class HZInterstitialAdAndroid : MonoBehaviour {
  
  public static void show(string tag="default") {
    if(Application.platform != RuntimePlatform.Android) return;

      AndroidJNIHelper.debug = false;
      using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
        jc.CallStatic("showInterstitial", tag); 
      }
  }

  public static void fetch(string tag="default") {
    if(Application.platform != RuntimePlatform.Android) return;

      AndroidJNIHelper.debug = false;
      using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
        jc.CallStatic("fetchInterstitial", tag); 
      }
  }
  
  public static Boolean isAvailable(string tag="default") {
    if(Application.platform != RuntimePlatform.Android) return false;

    AndroidJNIHelper.debug = false;
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
      return jc.CallStatic<Boolean>("isInterstitialAvailable", tag);
    }
  }

  public static void chartboostShowForLocation(string location) {
    if(Application.platform != RuntimePlatform.Android) return;

    AndroidJNIHelper.debug = false;
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
      jc.CallStatic("chartboostLocationShow", location);
    }
  }

  public static Boolean chartboostIsAvailableForLocation(string location) {
    if(Application.platform != RuntimePlatform.Android) return false;

    AndroidJNIHelper.debug = false;
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
      return jc.CallStatic<Boolean>("chartboostLocationIsAvailable", location);
    }
  }

  public static void chartboostFetchForLocation(string location) {
    if(Application.platform != RuntimePlatform.Android) return;

    AndroidJNIHelper.debug = false;
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
      jc.CallStatic("chartboostLocationFetch", location);
    }
  }
}
#endif
