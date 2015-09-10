//
//  HZIncentivizedAd.cs
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

public class HZIncentivizedAd : MonoBehaviour {
  
  public delegate void AdDisplayListener( string state, string tag );
  private static AdDisplayListener adDisplayListener;
  private static HZIncentivizedAd _instance = null;

  public static void show(string tag="default") {
    #if UNITY_ANDROID
    HZIncentivizedAdAndroid.show(tag);
    #endif

    #if UNITY_IPHONE && !UNITY_EDITOR
    HZIncentivizedAdIOS.show(tag);
    #endif
  }
  
  public static void fetch(string tag="default") {
    #if UNITY_ANDROID
    HZIncentivizedAdAndroid.fetch(tag);
    #endif

    #if UNITY_IPHONE && !UNITY_EDITOR
    HZIncentivizedAdIOS.fetch(tag);
    #endif
  }
  
  public static bool isAvailable(string tag="default") {
    #if UNITY_ANDROID
    return HZIncentivizedAdAndroid.isAvailable(tag);
    #elif UNITY_IPHONE && !UNITY_EDITOR
    return HZIncentivizedAdIOS.isAvailable(tag);
    #else
    return false;
    #endif
  }

  public static void setUserIdentifier(string identifier) {
    #if UNITY_ANDROID
    HZIncentivizedAdAndroid.setUserIdentifier(identifier);
    #endif

    #if UNITY_IPHONE && !UNITY_EDITOR
    HZIncentivizedAdIOS.setUserIdentifier(identifier);
    #endif
  }

  public static void initReceiver(){
    if (_instance == null) {
      GameObject receiverObject = new GameObject("HZIncentivizedAd");
      DontDestroyOnLoad(receiverObject);
      _instance = receiverObject.AddComponent<HZIncentivizedAd>();
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
public class HZIncentivizedAdIOS : MonoBehaviour {

  public static void show(string tag) {
    hz_ads_show_incentivized(tag);
  }

  [DllImport ("__Internal")]
  private static extern void hz_ads_show_incentivized(string tag);

  public static void fetch(string tag) {
    hz_ads_fetch_incentivized(tag);
  }

  [DllImport ("__Internal")]
  private static extern void hz_ads_fetch_incentivized(string tag);

  public static bool isAvailable(string tag) {
    return hz_ads_incentivized_is_available(tag);
  }

  [DllImport ("__Internal")]
  private static extern bool hz_ads_incentivized_is_available(string tag);

  public static void setUserIdentifier(string identifier) {
    hz_ads_incentivized_set_user_identifier(identifier);
  }

  [DllImport ("__Internal")]
  private static extern bool hz_ads_incentivized_set_user_identifier(string identifier);
}
#endif

#if UNITY_ANDROID
public class HZIncentivizedAdAndroid : MonoBehaviour {
  
  public static void show(string tag) {
    if(Application.platform != RuntimePlatform.Android) return;

      AndroidJNIHelper.debug = false;
      using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
        jc.CallStatic("showIncentivized", tag); 
      }
  }

  public static void fetch(string tag) {
    if(Application.platform != RuntimePlatform.Android) return;

      AndroidJNIHelper.debug = false;
      using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
        jc.CallStatic("fetchIncentivized", tag); 
      }
  }
  
  public static Boolean isAvailable(string tag) {
    if(Application.platform != RuntimePlatform.Android) return false;

      AndroidJNIHelper.debug = false;
      using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
        return jc.CallStatic<Boolean>("isIncentivizedAvailable", tag);
      }
  }

  public static void setUserIdentifier(string identifier) {
    if(Application.platform != RuntimePlatform.Android) return;
    AndroidJNIHelper.debug = false;
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
      jc.CallStatic("setIncentivizedUserIdentifier", identifier);
    }
  }
}
#endif
