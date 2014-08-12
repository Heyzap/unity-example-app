
//
//  HeyzapAds.cs
//
//  Copyright 2014 Smart Balloon, Inc. All Rights Reserved
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
  public delegate void AdDisplayListener( string state, string tag );
  private static AdDisplayListener adDisplayListener;
  private static HeyzapAds _instance = null;
    
  public static int FLAG_NO_OPTIONS = 0 << 0;
  public static int FLAG_DISABLE_AUTOMATIC_FETCHING = 1 << 0;
  public static int FLAG_INSTALL_TRACKING_ONLY = 1 << 1;
  public static int AMAZON = 1 << 2;

  public static void start(string publisher_id, int options) {
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
	
  public static void initReceiver(){
  	if (_instance == null) {
  		GameObject receiverObject = new GameObject("HeyzapAds");
  		DontDestroyOnLoad(receiverObject);
  		_instance = receiverObject.AddComponent<HeyzapAds>();
  	}
  }
}

#if UNITY_IPHONE
public class HeyzapAdsIOS : MonoBehaviour {
    public static void start(string publisher_id, int options=0) {
      hz_ads_start_app(publisher_id, options);
    }

    [DllImport ("__Internal")]
    private static extern void hz_ads_start_app(string publisher_id, int flags);
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
}
#endif
