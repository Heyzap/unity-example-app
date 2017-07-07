//
//  HZOfferwallAd.cs
//
//  Copyright 2017 Heyzap, Inc. All Rights Reserved
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
    /// Use this class to show offerwall ads.
    /// </summary>
    public class HZOfferwallAd : MonoBehaviour {
      
        public delegate void AdDisplayListener(string state, string tag);
        private static AdDisplayListener adDisplayListener;

        public delegate void VirtualCurrencyResponseListener(VirtualCurrencyResponse response);
        private static VirtualCurrencyResponseListener virtualCurrencyResponseListener;

        public delegate void VirtualCurrencyErrorListener(string errorMsg);
        private static VirtualCurrencyErrorListener virtualCurrencyErrorListener;

        private static HZOfferwallAd _instance = null;
      
        //provided since JS can't use default parameters
        /// <summary>
        /// Shows an ad with the default options.
        /// </summary>
        public static void Show() {
            HZOfferwallAd.ShowWithOptions(null);
        }
        /// <summary>
        /// Shows an ad with the given options.
        /// </summary>
        /// <param name="showOptions"> The options to show the ad with, or the default options if <c>null</c></param> 
        public static void ShowWithOptions(HZOfferwallShowOptions showOptions) {
            if (showOptions == null) {
                showOptions = new HZOfferwallShowOptions();
            }

            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HZOfferwallAdAndroid.ShowWithOptions(showOptions);
                #elif UNITY_IPHONE
                    HZOfferwallAdIOS.ShowWithOptions(showOptions);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to show an HZOfferwallAd, but the SDK does not function in the editor. You must use a device/emulator to fetch/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(HeyzapAds.NetworkCallback.SHOW_FAILED, showOptions.Tag));
            #endif
        }
      
        //provided since JS can't use default parameters
        /// <summary>
        /// Fetches an ad for the default ad tag.
        /// </summary>
        public static void Fetch() {
            HZOfferwallAd.Fetch(null);
        }
        /// <summary>
        /// Fetches an ad for the given ad tag.
        /// </summary>
        /// <param name="tag">The ad tag to fetch an ad for.</param>
        public static void Fetch(string tag) {
            tag = HeyzapAds.TagForString(tag);

            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HZOfferwallAdAndroid.Fetch(tag);
                #elif UNITY_IPHONE
                    HZOfferwallAdIOS.Fetch(tag);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to fetch an HZOfferwallAd, but the SDK does not function in the editor. You must use a device/emulator to fetch/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(HeyzapAds.NetworkCallback.FETCH_FAILED, tag));
            #endif
        }
      
        //provided since JS can't use default parameters
        /// <summary>
        /// Returns whether or not an ad is available for the default ad tag.
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable() {
            return HZOfferwallAd.IsAvailable(null);
        }
        /// <summary>
        /// Returns whether or not an ad is available for the given ad tag.
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable(string tag) {
            tag = HeyzapAds.TagForString(tag);

            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return HZOfferwallAdAndroid.IsAvailable(tag);
                #elif UNITY_IPHONE
                    return HZOfferwallAdIOS.IsAvailable(tag);
                #endif
            #else
                return false;
            #endif
        }

        /// <summary>
        /// Sends a request to the Virtual Currency Server to see if the user has earned virtual currency since the last request.
        /// </summary>
        /// <param name="currencyId">The ID of the currency to request information about. Setting this to NULL will request the default currency.</param>
        public static void RequestDeltaOfCurrency(string currencyId) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HZOfferwallAdAndroid.RequestDeltaOfCurrency(currencyId);
                #elif UNITY_IPHONE
                    HZOfferwallAdIOS.RequestDeltaOfCurrency(currencyId);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to request a VCS update on HZOfferwallAd, but the SDK does not function in the editor. You must use a device/emulator to do this.");
                _instance.StartCoroutine(InvokeVCSErrorNextFrame("only_works_on_device"));
            #endif
        }

        /// <summary>
        /// Sets the AdDisplayListener for offerwall ads, which will receive callbacks regarding the state of offerwall ads.
        /// </summary>
        public static void SetDisplayListener(AdDisplayListener listener) {
            HZOfferwallAd.adDisplayListener = listener;
        }

        /// <summary>
        /// Sets the VirtualCurrencyResponseListener for offerwall ads, which will receive callbacks in response to a request for a VCS update.
        /// </summary>
        public static void SetVirtualCurrencyResponseListener(VirtualCurrencyResponseListener listener) {
            HZOfferwallAd.virtualCurrencyResponseListener = listener;
        }

        /// <summary>
        /// Sets the VirtualCurrencyErrorListener for offerwall ads, which will receive callbacks regarding errors received after requesting virtual currency updates from the server.
        /// </summary>
        public static void SetVirtualCurrencyErrorListener(VirtualCurrencyErrorListener listener) {
            HZOfferwallAd.virtualCurrencyErrorListener = listener;
        }

        #region Internal methods
        public static void InitReceiver(){
            if (_instance == null) {
                GameObject receiverObject = new GameObject("HZOfferwallAd");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<HZOfferwallAd>();
            }
        }

        // received from native SDK
        public void SetCallback(string message) {
            string[] displayStateParams = message.Split(',');
            HZOfferwallAd.SetCallbackStateAndTag(displayStateParams[0], displayStateParams[1]); 
        }
        protected static void SetCallbackStateAndTag(string state, string tag) {
            if (HZOfferwallAd.adDisplayListener != null) {
                HZOfferwallAd.adDisplayListener(state, tag);
            }
        }

        // used for in-editor functionality
        protected static IEnumerator InvokeCallbackNextFrame(string state, string tag) {
            yield return null; // wait a frame
            HZOfferwallAd.SetCallbackStateAndTag(state, tag);
        }

        // received from native SDK
        public void VCSResponse(string jsonString) {
            HZOfferwallAd.SendVCSResponse(jsonString);
        }
        protected static void SendVCSResponse(string jsonString) {
            if (HZOfferwallAd.virtualCurrencyResponseListener != null) {
                VirtualCurrencyResponse response = (VirtualCurrencyResponse)JsonUtility.FromJson<VirtualCurrencyResponse>(jsonString);
                HZOfferwallAd.virtualCurrencyResponseListener(response);
            }
        }

        // received from native SDK
        public void VCSError(string errorMsg) {
            HZOfferwallAd.SendVCSError(errorMsg);
        }
        protected static void SendVCSError(string errorMsg) {
            if (HZOfferwallAd.virtualCurrencyErrorListener != null) {
                HZOfferwallAd.virtualCurrencyErrorListener(errorMsg);
            }
        }

        // used for in-editor functionality
        protected static IEnumerator InvokeVCSErrorNextFrame(string errorMsg) {
            yield return null; // wait a frame
            HZOfferwallAd.SendVCSError(errorMsg);
        }
        #endregion
    }

    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class HZOfferwallAdIOS : MonoBehaviour {
        [DllImport ("__Internal")]
        private static extern void hz_ads_show_offerwall(string tag, bool shouldCloseAfterFirstClick);
        [DllImport ("__Internal")]
        private static extern void hz_ads_fetch_offerwall(string tag);
        [DllImport ("__Internal")]
        private static extern bool hz_ads_offerwall_is_available(string tag);
        [DllImport ("__Internal")]
        private static extern bool hz_ads_virtual_currency_request(string currencyId);


        public static void ShowWithOptions(HZOfferwallShowOptions showOptions) {
            hz_ads_show_offerwall(showOptions.Tag, showOptions.ShouldCloseAfterFirstClick);
        }

        public static void Fetch(string tag) {
            hz_ads_fetch_offerwall(tag);
        }

        public static bool IsAvailable(string tag) {
            return hz_ads_offerwall_is_available(tag);
        }

        public static void RequestDeltaOfCurrency(string currencyId) {
            hz_ads_virtual_currency_request(currencyId);
        }
    }
    #endif

    #if UNITY_ANDROID && !UNITY_EDITOR
    public class HZOfferwallAdAndroid : MonoBehaviour {
      
        public static void ShowWithOptions(HZOfferwallShowOptions showOptions) {
        if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("showOfferwall", showOptions.Tag);
            }
        }

        public static void Fetch(string tag) {
            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("fetchOfferwall", tag); 
            }
        }
          
        public static Boolean IsAvailable(string tag) {
            if(Application.platform != RuntimePlatform.Android) return false;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                return jc.CallStatic<Boolean>("isOfferwallAvailable", tag);
            }
        }

        public static void RequestDeltaOfCurrency(string currencyId) {
            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                return jc.CallStatic<Boolean>("TODO", currencyId);
            }
        }
    }
    #endif
    #endregion

    [Serializable]
    public class VirtualCurrencyResponse {
        public string LatestTransactionID;
        public string CurrencyID;
        public string CurrencyName;
        public float DeltaOfCurrency;
    }
}
