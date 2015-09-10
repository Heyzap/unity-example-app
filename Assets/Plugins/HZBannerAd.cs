//
//  HZBannerAd.cs
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

public class HZBannerAd : MonoBehaviour {
    public delegate void AdDisplayListener( string state, string tag );
    private static AdDisplayListener adDisplayListener;
    private static HZBannerAd _instance = null;

    public static string POSITION_TOP = "top";
    public static string POSITION_BOTTOM = "bottom";

    public static void showWithTag(string position, string tag) {
      #if UNITY_ANDROID
      HZBannerAdAndroid.show(position, tag);
      #endif
      
      #if UNITY_IPHONE && !UNITY_EDITOR
      HZBannerAdIOS.show(position, tag);
      #endif
    }

    public static void show(string position) {
      #if UNITY_ANDROID
      HZBannerAdAndroid.show(position, "");
      #endif

      #if UNITY_IPHONE && !UNITY_EDITOR
      HZBannerAdIOS.show(position, "");
      #endif
    }

    public static bool getCurrentBannerDimensions(out Rect banner){
      #if UNITY_ANDROID
      return HZBannerAdAndroid.getCurrentBannerDimensions(out banner);

      #elif UNITY_IPHONE && !UNITY_EDITOR
      return HZBannerAdIOS.getCurrentBannerDimensions(out banner);

      #else
      banner = new Rect(0,0,0,0);
      return false;
      #endif
    }

    public static void hide() {
      #if UNITY_ANDROID
      HZBannerAdAndroid.hide();
      #endif

      #if UNITY_IPHONE && !UNITY_EDITOR
      HZBannerAdIOS.hide();
      #endif
    }

    public static void destroy() {
      #if UNITY_ANDROID
      HZBannerAdAndroid.destroy();
      #endif

      #if UNITY_IPHONE && !UNITY_EDITOR
      HZBannerAdIOS.destroy();
      #endif
    }

    public static void initReceiver(){
      if (_instance == null) {
        GameObject receiverObject = new GameObject("HZBannerAd");
        DontDestroyOnLoad(receiverObject);
        _instance = receiverObject.AddComponent<HZBannerAd>();
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
public class HZBannerAdIOS : MonoBehaviour {

  public static void show(string position, string tag) {
    hz_ads_show_banner(position, tag);
  }

  [DllImport ("__Internal")]
  private static extern void hz_ads_show_banner(string position, string tag);

  public static bool hide() {
    return hz_ads_hide_banner();
  }

  [DllImport ("__Internal")]
  private static extern bool hz_ads_hide_banner();

  public static void destroy() {
    hz_ads_destroy_banner();
  }

  [DllImport ("__Internal")]
  private static extern bool hz_ads_destroy_banner();

  public static bool getCurrentBannerDimensions(out Rect banner){
    banner = new Rect(0,0,0,0); // default value in error cases

    string returnValue = hz_ads_banner_dimensions();
    if(returnValue == null || returnValue.Length == 0){
      return false;
    }

    string[] split = returnValue.Split(' ');
    if(split.Length != 4){
      return false;
    }

    banner = new Rect(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
    return true;
  }

  [DllImport ("__Internal")]
  private static extern string hz_ads_banner_dimensions();

}
#endif

#if UNITY_ANDROID
public class HZBannerAdAndroid : MonoBehaviour {

  public static bool getCurrentBannerDimensions(out Rect banner){
    banner = new Rect(0,0,0,0); // default value in error cases

    if (Application.platform != RuntimePlatform.Android) {
      return false;
    }

    AndroidJNIHelper.debug = false; 
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
      string returnValue = jc.CallStatic<string>("getBannerDimensions");
      if(returnValue == null || returnValue.Length == 0){
        return false;
      }
      
      string[] split = returnValue.Split(' ');
      if(split.Length != 4){
        return false;
      }

      banner = new Rect(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]), int.Parse(split[3]));
      return true;
    }
  }

  
  public static void show(string position, string tag) {
    if(Application.platform != RuntimePlatform.Android) return;

    AndroidJNIHelper.debug = false;
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
      jc.CallStatic("showBanner", tag, position);
    }
  }

  public static void hide() {
    if(Application.platform != RuntimePlatform.Android) return;

      AndroidJNIHelper.debug = false;
      using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
        jc.CallStatic("hideBanner"); 
      }
  }

  public static void destroy() {
    if(Application.platform != RuntimePlatform.Android) return;

    AndroidJNIHelper.debug = false;
    using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
      jc.CallStatic("destroyBanner"); 
    }
  }

}
#endif