//
//  HZIncentivizedAd.cs
//
//  Copyright 2013 Smart Balloon, Inc. All Rights Reserved
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
  
  public static void show() {
    #if UNITY_ANDROID
    HZIncentivizedAdAndroid.show();
    #endif

    #if UNITY_IPHONE
    HZIncentivizedAdIOS.show();
    #endif
  }

  public static void hide() {
    #if UNITY_ANDROID
    HZIncentivizedAdAndroid.hide();
    #endif

    #if UNITY_IPHONE
    HZIncentivizedAdIOS.hide();
    #endif
  }
  
  public static void fetch() {
    #if UNITY_ANDROID
    HZIncentivizedAdAndroid.fetch();
    #endif

    #if UNITY_IPHONE
    HZIncentivizedAdIOS.fetch();
    #endif
  }
  
  public static bool isAvailable() {
    #if UNITY_ANDROID
    return HZIncentivizedAdAndroid.isAvailable();
    #endif

    #if UNITY_IPHONE
    return HZIncentivizedAdIOS.isAvailable();
    #endif
    
    return false;
  }

  public static void setUserIdentifier(string identifier) {
    #if UNITY_ANDROID
    HZIncentivizedAdAndroid.setUserIdentifier(identifier);
    #endif

    #if UNITY_IPHONE
    HZIncentivizedAdIOS.setUserIdentifier(identifier);
    #endif
  }
}

#if UNITY_IPHONE
public class HZIncentivizedAdIOS : MonoBehaviour {

  public static void show() {
    hz_ads_show_incentivized();
  }

  [DllImport ("__Internal")]
  private static extern void hz_ads_show_incentivized();

  public static void hide() {
    hz_ads_hide_incentivized();
  }

  [DllImport ("__Internal")]
  private static extern void hz_ads_hide_incentivized();

  public static void fetch() {
    hz_ads_fetch_incentivized();
  }

  [DllImport ("__Internal")]
  private static extern void hz_ads_fetch_incentivized();

  public static bool isAvailable() {
    return hz_ads_incentivized_is_available();
  }

  [DllImport ("__Internal")]
  private static extern bool hz_ads_incentivized_is_available();

  public static void setUserIdentifier(string identifier) {
    hz_ads_incentivized_set_user_identifier(identifier);
  }

  [DllImport ("__Internal")]
  private static extern bool hz_ads_incentivized_set_user_identifier(string identifier);
}
#endif

#if UNITY_ANDROID
public class HZIncentivizedAdAndroid : MonoBehaviour {
  
  public static void show() {
    if(Application.platform != RuntimePlatform.Android) return;

      AndroidJNIHelper.debug = false;
      using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.ads.UnityHelper")) { 
        jc.CallStatic("showIncentivized"); 
      }
  }

  public static void hide() {
    if(Application.platform != RuntimePlatform.Android) return;

      AndroidJNIHelper.debug = false;
      using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.ads.UnityHelper")) { 
        jc.CallStatic("hideIncentivized");
      }
  }

  public static void fetch() {
    if(Application.platform != RuntimePlatform.Android) return;

      AndroidJNIHelper.debug = false;
      using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.ads.UnityHelper")) { 
        jc.CallStatic("fetchIncentivized"); 
      }
  }
  
  public static Boolean isAvailable() {
    if(Application.platform != RuntimePlatform.Android) return false;

      AndroidJNIHelper.debug = false;
      using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.ads.UnityHelper")) { 
        return jc.CallStatic<Boolean>("isIncentivizedAvailable");
      }

      return false;
  }

  public static void setUserIdentifier(string identifier) {
    if(Application.platform != RuntimePlatform.Android) return;
    AndroidJNIHelper.debug = false;
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.ads.UnityHelper")) { 
      jc.CallStatic("setIncentivizedUserIdentifier", identifier);
    }
  }
}
#endif
