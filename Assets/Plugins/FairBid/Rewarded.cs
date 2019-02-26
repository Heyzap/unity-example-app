//  Copyright 2019 Fyber. All Rights Reserved
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

namespace FairBid {
    /// <summary>
    /// Use this class to show rewarded ads.
    /// </summary>
    public class Rewarded : MonoBehaviour {
        
        public delegate void AdDisplayListener(string state, string placement);
        private static AdDisplayListener adDisplayListener;
        private static Rewarded _instance;

        /// <summary>
        /// Requests an ad for the given placement name.
        /// </summary>
        /// <param name="placement">The placement name to request an ad for.</param>
        public static void Request(string placement) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    RewardedAndroid.Request(placement);
                #elif UNITY_IPHONE
                    RewardedIOS.Request(placement);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to request an Rewarded, but the SDK does not function in the editor. You must use a device/emulator to request/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(FairBidSDK.NetworkCallback.REQUEST_FAILED, placement));
            #endif
        }

        /// <summary>
        /// Shows an ad with the given options.
        /// </summary>
        /// <param name="showOptions"> The options to show the ad with, or the default options if <c>null</c></param>
        public static void ShowWithOptions(RewardedShowOptions showOptions) {
            if (showOptions == null) {
                showOptions = new RewardedShowOptions();
            }

            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    RewardedAndroid.ShowWithOptions(showOptions);
                #elif UNITY_IPHONE
                    RewardedIOS.ShowWithOptions(showOptions);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to show an Rewarded, but the SDK does not function in the editor. You must use a device/emulator to request/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(FairBidSDK.NetworkCallback.SHOW_FAILED, showOptions.Placement));
            #endif
        }

        /// <summary>
        /// Returns whether or not an ad is available for the given placement name.
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable(string placement) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return RewardedAndroid.IsAvailable(placement);
                #elif UNITY_IPHONE
                    return RewardedIOS.IsAvailable(placement);
                #endif
            #else
                return false;
            #endif
        }
        
        /// <summary>
        /// Sets the AdDisplayListener for rewarded ads, which will receive callbacks regarding the state of rewarded ads.
        /// </summary>
        public static void SetDisplayListener(AdDisplayListener listener) {
            adDisplayListener = listener;
        }

        #region Internal methods
        public static void InitReceiver(){
            if (_instance == null) {
                GameObject receiverObject = new GameObject("Rewarded");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<Rewarded>();
            }
        }

        public void SetCallback(string message) {
            string[] displayStateParams = message.Split(',');
            SetCallbackStateAndPlacement(displayStateParams[0], displayStateParams[1]); 
        }
        
        protected static void SetCallbackStateAndPlacement(string state, string placement) {
            if (adDisplayListener != null) {
                adDisplayListener(state, placement);
            }
        }

        protected static IEnumerator InvokeCallbackNextFrame(string state, string placement) {
            yield return null; // wait a frame
            SetCallbackStateAndPlacement(state, placement);
        }
        #endregion
    }

    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class RewardedIOS : MonoBehaviour {
        [DllImport ("__Internal")]
        private static extern void fyb_sdk_request_rewarded(string placement);
        [DllImport ("__Internal")]
        private static extern void fyb_sdk_show_rewarded_with_custom_info(string placement, string rewardedInfo);
        [DllImport ("__Internal")]
        private static extern bool fyb_sdk_rewarded_is_available(string placement);
        

        public static void ShowWithOptions(RewardedShowOptions showOptions) {
            fyb_sdk_show_rewarded_with_custom_info(showOptions.Placement, showOptions.RewardedInfo);
        }
        
        public static void Request(string placement) {
            fyb_sdk_request_rewarded(placement);
        }
        
        public static bool IsAvailable(string placement) {
            return fyb_sdk_rewarded_is_available(placement);
        }
    }
    #endif

    #if UNITY_ANDROID && !UNITY_EDITOR
    public class RewardedAndroid : MonoBehaviour {
        
        public static void Request(string placement) {
            if (Application.platform != RuntimePlatform.Android) return;
            
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("requestRewarded", placement); 
            }
        }
        
        public static void ShowWithOptions(RewardedShowOptions showOptions) {
            if (Application.platform != RuntimePlatform.Android) return;
            
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("showRewarded", showOptions.Placement, showOptions.RewardedInfo); 
            }
        }
        
        public static Boolean IsAvailable(string placement) {
            if (Application.platform != RuntimePlatform.Android) return false;
            
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                return jc.CallStatic<Boolean>("isRewardedAvailable", placement);
            }
        }
    }
    #endif
    #endregion
}
