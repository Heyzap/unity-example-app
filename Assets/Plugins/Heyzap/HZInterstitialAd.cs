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

namespace Heyzap {
    /// <summary>
    /// Use this class to show interstitial ads. Depending on the network and your settings, these can be static or video ads.
    /// </summary>
    public class HZInterstitialAd : MonoBehaviour {

        public delegate void AdDisplayListener(string state, string tag);
        private static AdDisplayListener adDisplayListener;
        private static HZInterstitialAd _instance = null;

        //provided since JS can't use default parameters
        /// <summary>
        /// Shows an ad with the default options.
        /// </summary>
        public static void Show() {
            HZInterstitialAd.ShowWithOptions(null);
        }

        /// <summary>
        /// Shows an ad with the given options.
        /// </summary>
        /// <param name="showOptions"> The options to show the ad with, or the default options if <c>null</c></param> 
        public static void ShowWithOptions(HZShowOptions showOptions) {
            if (showOptions == null) {
                showOptions = new HZShowOptions();
            }

            #if UNITY_ANDROID
            HZInterstitialAdAndroid.ShowWithOptions(showOptions);
            #endif
            
            #if UNITY_IPHONE && !UNITY_EDITOR
            HZInterstitialAdIOS.ShowWithOptions(showOptions);
            #endif
        }
            
        /// <summary>
        /// Fetches an ad
        /// </summary>
        public static void Fetch() {
            #if UNITY_ANDROID
            HZInterstitialAdAndroid.Fetch();
            #endif

            #if UNITY_IPHONE && !UNITY_EDITOR
            HZInterstitialAdIOS.Fetch();
            #endif
        }

        /// <summary>
        /// Fetches an ad for the given ad tag.
        /// </summary>
        /// <param name="tag">The ad tag to fetch an ad for.</param>
        public static void Fetch(string tag) {
            Fetch();
        }
      
        /// <summary>
        /// Returns whether or not an ad is available.
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable() {
            #if UNITY_ANDROID
            return HZInterstitialAdAndroid.IsAvailable();
            #elif UNITY_IPHONE && !UNITY_EDITOR
            return HZInterstitialAdIOS.IsAvailable();
            #else
            return false;
            #endif
        }
        /// <summary>
        /// Returns whether or not an ad is available for the given ad tag.
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable(string tag) {
            return IsAvailable();
        }

        /// <summary>
        /// Sets the AdDisplayListener for interstitial ads, which will receive callbacks regarding the state of interstitial ads.
        /// </summary>
        public static void SetDisplayListener(AdDisplayListener listener) {
            HZInterstitialAd.adDisplayListener = listener;
        }

        #region Internal methods
        public static void InitReceiver(){
            if (_instance == null) {
                GameObject receiverObject = new GameObject("HZInterstitialAd");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<HZInterstitialAd>();
            }
        }

        public void SetCallback(string message) {
            if (HZInterstitialAd.adDisplayListener != null) {
                HZInterstitialAd.adDisplayListener(message, "");
            }
        }

        #endregion
    }

    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class HZInterstitialAdIOS {
        [DllImport ("__Internal")]
        private static extern void hz_ads_show_interstitial(string tag);
        [DllImport ("__Internal")]
        private static extern void hz_ads_fetch_interstitial();
        [DllImport ("__Internal")]
        private static extern bool hz_ads_interstitial_is_available();

        public static void ShowWithOptions(HZShowOptions showOptions) {
            hz_ads_show_interstitial(showOptions.Tag);
        }

        public static void Fetch() {
            hz_ads_fetch_interstitial();
        }

        public static bool IsAvailable() {
            return hz_ads_interstitial_is_available();
        }
    }
    #endif

    #if UNITY_ANDROID
    public class HZInterstitialAdAndroid {
     
        public static void ShowWithOptions(HZShowOptions showOptions) {
        if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("showInterstitial", showOptions.Tag); 
            }
        }


        public static void Fetch() {
            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("fetchInterstitial"); 
            }
        }

        public static Boolean IsAvailable() {
            if(Application.platform != RuntimePlatform.Android) return false;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                return jc.CallStatic<Boolean>("isInterstitialAvailable");
            }
        }
    }
    #endif
    #endregion
}