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

            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HZInterstitialAdAndroid.ShowWithOptions(showOptions);
                #elif UNITY_IPHONE
                    HZInterstitialAdIOS.ShowWithOptions(showOptions);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to show an HZInterstitalAd, but the SDK does not function in the editor. You must use a device/emulator to fetch/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(HeyzapAds.NetworkCallback.SHOW_FAILED, showOptions.Tag));
            #endif
        }

        //provided since JS can't use default parameters
        /// <summary>
        /// Fetches an ad for the default ad tag.
        /// </summary>
        public static void Fetch() {
            HZInterstitialAd.Fetch(null);
        }
        /// <summary>
        /// Fetches an ad for the given ad tag.
        /// </summary>
        /// <param name="tag">The ad tag to fetch an ad for.</param>
        public static void Fetch(string tag) {
            tag = HeyzapAds.TagForString(tag);

            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HZInterstitialAdAndroid.Fetch(tag);
                #elif UNITY_IPHONE
                    HZInterstitialAdIOS.Fetch(tag);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to fetch an HZInterstitialAd, but the SDK does not function in the editor. You must use a device/emulator to fetch/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(HeyzapAds.NetworkCallback.FETCH_FAILED, tag));
            #endif
        }
      
        //provided since JS can't use default parameters
        /// <summary>
        /// Returns whether or not an ad is available for the default ad tag.
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable() {
            return HZInterstitialAd.IsAvailable(null);
        }
        /// <summary>
        /// Returns whether or not an ad is available for the given ad tag.
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable(string tag) {
            tag = HeyzapAds.TagForString(tag);

            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return HZInterstitialAdAndroid.IsAvailable(tag);
                #elif UNITY_IPHONE
                    return HZInterstitialAdIOS.IsAvailable(tag);
                #endif
            #else
                return false;
            #endif
        }

        /// <summary>
        /// Sets the AdDisplayListener for interstitial ads, which will receive callbacks regarding the state of interstitial ads.
        /// </summary>
        public static void SetDisplayListener(AdDisplayListener listener) {
            HZInterstitialAd.adDisplayListener = listener;
        }

        #region Chartboost-specific methods
        /// <summary>
        /// Fetches an ad from Chartboost for the given CBLocation.
        /// </summary>
        public static void ChartboostFetchForLocation(string location) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HZInterstitialAdAndroid.chartboostFetchForLocation(location);
                #elif UNITY_IPHONE
                    HZInterstitialAdIOS.chartboostFetchForLocation(location);
                #endif
            #else
            #endif
        }

        /// <summary>
        /// Returns whether an ad is available at the given CBLocation.
        /// </summary>
        public static bool ChartboostIsAvailableForLocation(string location) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return HZInterstitialAdAndroid.chartboostIsAvailableForLocation(location);
                #elif UNITY_IPHONE
                    return HZInterstitialAdIOS.chartboostIsAvailableForLocation(location);
                #endif
            #else
                return false;
            #endif
        }

        /// <summary>
        /// Attempts to show an ad from Chartboost for the given CBLocation.
        /// </summary>
        public static void ChartboostShowForLocation(string location) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HZInterstitialAdAndroid.chartboostShowForLocation(location);
                #elif UNITY_IPHONE
                    HZInterstitialAdIOS.chartboostShowForLocation(location);
                #endif
            #else
            #endif
        }
        #endregion

        #region Internal methods
        public static void InitReceiver(){
            if (_instance == null) {
                GameObject receiverObject = new GameObject("HZInterstitialAd");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<HZInterstitialAd>();
            }
        }

        public void SetCallback(string message) {
            string[] displayStateParams = message.Split(',');
            HZInterstitialAd.SetCallbackStateAndTag(displayStateParams[0], displayStateParams[1]); 
        }

        protected static void SetCallbackStateAndTag(string state, string tag) {
            if (HZInterstitialAd.adDisplayListener != null) {
                HZInterstitialAd.adDisplayListener(state, tag);
            }
        }

        protected static IEnumerator InvokeCallbackNextFrame(string state, string tag) {
            yield return null; // wait a frame
            HZInterstitialAd.SetCallbackStateAndTag(state, tag);
        }
        #endregion
    }

    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class HZInterstitialAdIOS {
        [DllImport ("__Internal")]
        private static extern void hz_ads_show_interstitial(string tag);
        [DllImport ("__Internal")]
        private static extern void hz_ads_fetch_interstitial(string tag);
        [DllImport ("__Internal")]
        private static extern bool hz_ads_interstitial_is_available(string tag);
        [DllImport ("__Internal")]
        private static extern void hz_fetch_chartboost_for_location(string location);
        [DllImport ("__Internal")]
        private static extern bool hz_chartboost_is_available_for_location(string location);
        [DllImport ("__Internal")]
        private static extern void hz_show_chartboost_for_location(string location);


        public static void ShowWithOptions(HZShowOptions showOptions) {
            hz_ads_show_interstitial(showOptions.Tag);
        }

        public static void Fetch(string tag) {
            hz_ads_fetch_interstitial(tag);
        }

        public static bool IsAvailable(string tag) {
            return hz_ads_interstitial_is_available(tag);
        }


        // Chartboost functions

        public static void chartboostFetchForLocation(string location) {
            hz_fetch_chartboost_for_location(location);
        }

        public static bool chartboostIsAvailableForLocation(string location) {
            return hz_chartboost_is_available_for_location(location);
        }

        public static void chartboostShowForLocation(string location) {
            hz_show_chartboost_for_location(location);
        }
    }
    #endif

    #if UNITY_ANDROID && !UNITY_EDITOR
    public class HZInterstitialAdAndroid {
      
        public static void ShowWithOptions(HZShowOptions showOptions) {
            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("showInterstitial", showOptions.Tag); 
            }
        }

        public static void Fetch(string tag="default") {
            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("fetchInterstitial", tag); 
            }
        }

        public static Boolean IsAvailable(string tag="default") {
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
    #endregion
}
