//  HZRewardedAd.cs
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
    /// Use this class to show rewarded ads.
    /// </summary>
    public class HZRewardedAd : MonoBehaviour {
        
        public delegate void AdDisplayListener(string state, string placement);
        private static AdDisplayListener adDisplayListener;
        private static HZRewardedAd _instance;

        //provided since JS can't use default parameters
        /// <summary>
        /// Requests an ad for the default placement name.
        /// </summary>
        public static void Request() {
            HZRewardedAd.Request(null);
        }
        /// <summary>
        /// Fetches an ad for the given placement name.
        /// </summary>
        /// <param name="placement">The placement name to request an ad for.</param>
        public static void Request(string placement) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HZRewardedAdAndroid.Fetch(placement);
                #elif UNITY_IPHONE
                    HZRewardedAdIOS.Fetch(placement);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to fetch an HZRewardedAd, but the SDK does not function in the editor. You must use a device/emulator to fetch/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(HeyzapAds.NetworkCallback.FETCH_FAILED, placement));
            #endif
        }

        //provided since JS can't use default parameters
        /// <summary>
        /// Shows an ad with the default options.
        /// </summary>
        public static void Show() {
            HZRewardedAd.ShowWithOptions(null);
        }
        /// <summary>
        /// Shows an ad with the given options.
        /// </summary>
        /// <param name="showOptions"> The options to show the ad with, or the default options if <c>null</c></param>
        public static void ShowWithOptions(HZRewardedShowOptions showOptions) {
            if (showOptions == null) {
                showOptions = new HZRewardedShowOptions();
            }

            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HZRewardedAdAndroid.ShowWithOptions(showOptions);
                #elif UNITY_IPHONE
                    HZRewardedAdIOS.ShowWithOptions(showOptions);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to show an HZRewardedAd, but the SDK does not function in the editor. You must use a device/emulator to fetch/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(HeyzapAds.NetworkCallback.SHOW_FAILED, showOptions.Placement));
            #endif
        }

        //provided since JS can't use default parameters
        /// <summary>
        /// Returns whether or not an ad is available for the default placement name.
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable() {
            return HZRewardedAd.IsAvailable(null);
        }
        /// <summary>
        /// Returns whether or not an ad is available for the given placement name.
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable(string placement) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return HZRewardedAdAndroid.IsAvailable(placement);
                #elif UNITY_IPHONE
                    return HZRewardedAdIOS.IsAvailable(placement);
                #endif
            #else
                return false;
            #endif
        }
        
        /// <summary>
        /// Sets the AdDisplayListener for rewarded ads, which will receive callbacks regarding the state of rewarded ads.
        /// </summary>
        public static void SetDisplayListener(AdDisplayListener listener) {
            HZRewardedAd.adDisplayListener = listener;
        }

        #region Internal methods
        public static void InitReceiver(){
            if (_instance == null) {
                GameObject receiverObject = new GameObject("HZRewardedAd");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<HZRewardedAd>();
            }
        }

        public void SetCallback(string message) {
            string[] displayStateParams = message.Split(',');
            HZRewardedAd.SetCallbackStateAndPlacement(displayStateParams[0], displayStateParams[1]); 
        }
        
        protected static void SetCallbackStateAndPlacement(string state, string placement) {
            if (HZRewardedAd.adDisplayListener != null) {
                HZRewardedAd.adDisplayListener(state, placement);
            }
        }

        protected static IEnumerator InvokeCallbackNextFrame(string state, string placement) {
            yield return null; // wait a frame
            HZRewardedAd.SetCallbackStateAndPlacement(state, placement);
        }
        #endregion
    }

    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class HZRewardedAdIOS : MonoBehaviour {
        [DllImport ("__Internal")]
        private static extern void hz_ads_show_rewarded_with_custom_info(string placement, string rewardedInfo);
        [DllImport ("__Internal")]
        private static extern void hz_ads_fetch_rewarded(string placement);
        [DllImport ("__Internal")]
        private static extern bool hz_ads_rewarded_is_available(string placement);
        

        public static void ShowWithOptions(HZRewardedShowOptions showOptions) {
            hz_ads_show_rewarded_with_custom_info(showOptions.Placement, showOptions.RewardedInfo);
        }
        
        public static void Fetch(string placement) {
            hz_ads_fetch_rewarded(placement);
        }
        
        public static bool IsAvailable(string placement) {
            return hz_ads_rewarded_is_available(placement);
        }
    }
    #endif

    #if UNITY_ANDROID && !UNITY_EDITOR
    public class HZRewardedAdAndroid : MonoBehaviour {
        
        public static void ShowWithOptions(HZRewardedShowOptions showOptions) {
            if(Application.platform != RuntimePlatform.Android) return;
            
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("showRewarded", showOptions.Placement, showOptions.RewardedInfo); 
            }
        }
        
        public static void Fetch(string placement) {
            if(Application.platform != RuntimePlatform.Android) return;
            
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("fetchRewarded", placement); 
            }
        }
        
        public static Boolean IsAvailable(string placement) {
            if(Application.platform != RuntimePlatform.Android) return false;
            
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                return jc.CallStatic<Boolean>("isRewardedAvailable", placement);
            }
        }
    }
    #endif
    #endregion
}
