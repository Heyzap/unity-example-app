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

namespace FairBid
{
    /// <summary>
    /// Use this class to show interstitial ads. Depending on the network and your settings, these can be static or video ads.
    /// </summary>
    public class Interstitial : MonoBehaviour
    {

        public delegate void AdDisplayListener(string state, string placement);
        private static AdDisplayListener adDisplayListener;
        private static Interstitial _instance;

        /// <summary>
        /// Requests an ad for the given placement name.
        /// </summary>
        /// <param name="placement">The placement name to request an ad for.</param>
        public static void Request(string placement)
        {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    InterstitialAndroid.Request(placement);
                #elif UNITY_IPHONE
                    InterstitialIOS.Request(placement);
                #endif
            #else
                Debug.LogWarning("Call received to request an Interstitial, but the SDK does not function in the editor. You must use a device/emulator to request/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(FairBidSDK.NetworkCallback.REQUEST_FAILED, placement));
            #endif
        }

        /// <summary>
        /// Shows an ad with the given options.
        /// </summary>
        /// <param name="showOptions"> The options to show the ad with, or the default options if <c>null</c></param> 
        public static void ShowWithOptions(ShowOptions showOptions)
        {
            if (showOptions == null)
            {
                showOptions = new ShowOptions();
            }
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    InterstitialAndroid.ShowWithOptions(showOptions);
                #elif UNITY_IPHONE
                    InterstitialIOS.ShowWithOptions(showOptions);
                #endif
            #else
                Debug.LogWarning("Call received to show an Interstitial, but the SDK does not function in the editor. You must use a device/emulator to request/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(FairBidSDK.NetworkCallback.SHOW_FAILED, showOptions.Placement));
            #endif
        }

        /// <summary>
        /// Returns whether or not an ad is available for the given placement name.
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable(string placement)
        {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return InterstitialAndroid.IsAvailable(placement);
                #elif UNITY_IPHONE
                    return InterstitialIOS.IsAvailable(placement);
                #endif
            #else
                return false;
            #endif
        }

        /// <summary>
        /// Sets the AdDisplayListener for interstitial ads, which will receive callbacks regarding the state of interstitial ads.
        /// </summary>
        public static void SetDisplayListener(AdDisplayListener listener)
        {
            adDisplayListener = listener;
        }

        #region Chartboost-specific methods
        /// <summary>
        /// Requests an ad from Chartboost for the given CBLocation.
        /// </summary>
        public static void ChartboostFetchForLocation(string location)
        {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    InterstitialAndroid.chartboostFetchForLocation(location);
                #elif UNITY_IPHONE
                    InterstitialIOS.chartboostFetchForLocation(location);
                #endif
            #else
            #endif
        }

        /// <summary>
        /// Returns whether an ad is available at the given CBLocation.
        /// </summary>
        public static bool ChartboostIsAvailableForLocation(string location)
        {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return InterstitialAndroid.chartboostIsAvailableForLocation(location);
                #elif UNITY_IPHONE
                    return InterstitialIOS.chartboostIsAvailableForLocation(location);
                #endif
            #else
                return false;
            #endif
        }

        /// <summary>
        /// Attempts to show an ad from Chartboost for the given CBLocation.
        /// </summary>
        public static void ChartboostShowForLocation(string location)
        {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    InterstitialAndroid.chartboostShowForLocation(location);
                #elif UNITY_IPHONE
                    InterstitialIOS.chartboostShowForLocation(location);
                #endif
                #else
            #endif
        }
        #endregion

        #region Internal methods
        public static void InitReceiver()
        {
            if (_instance == null)
            {
                GameObject receiverObject = new GameObject("Interstitial");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<Interstitial>();
            }
        }

        public void SetCallback(string message)
        {
            string[] displayStateParams = message.Split(',');
            SetCallbackStateAndPlacement(displayStateParams[0], displayStateParams[1]);
        }

        protected static void SetCallbackStateAndPlacement(string state, string placement)
        {
            if (adDisplayListener != null)
            {
                adDisplayListener(state, placement);
            }
        }

        protected static IEnumerator InvokeCallbackNextFrame(string state, string placement)
        {
            yield return null; // wait a frame
            SetCallbackStateAndPlacement(state, placement);
        }
        #endregion
    }

    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class InterstitialIOS {
        [DllImport ("__Internal")]
        private static extern void fyb_sdk_request_interstitial(string placement);
        [DllImport ("__Internal")]
        private static extern void fyb_sdk_show_interstitial(string placement);
        [DllImport ("__Internal")]         
        private static extern bool fyb_sdk_interstitial_is_available(string placement);
        [DllImport ("__Internal")]
        private static extern void fyb_fetch_chartboost_for_location(string location);
        [DllImport ("__Internal")]
        private static extern bool fyb_chartboost_is_available_for_location(string location);
        [DllImport ("__Internal")]
        private static extern void fyb_show_chartboost_for_location(string location);

        public static void ShowWithOptions(ShowOptions showOptions) {
            fyb_sdk_show_interstitial(showOptions.Placement);
        }

        public static void Request(string placement) {
            fyb_sdk_request_interstitial(placement);
        }

        public static bool IsAvailable(string placement) {
            return fyb_sdk_interstitial_is_available(placement);
        }

        // Chartboost functions

        public static void chartboostFetchForLocation(string location) {
            fyb_fetch_chartboost_for_location(location);
        }

        public static bool chartboostIsAvailableForLocation(string location) {
            return fyb_chartboost_is_available_for_location(location);
        }

        public static void chartboostShowForLocation(string location) {
            fyb_show_chartboost_for_location(location);
        }
    }
    #endif

    #if UNITY_ANDROID && !UNITY_EDITOR
    public class InterstitialAndroid {

        public static void Request(string placement) {
            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("requestInterstitial", placement); 
            }
        }
      
        public static void ShowWithOptions(ShowOptions showOptions) {
            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("showInterstitial", showOptions.Placement); 
            }
        }

        public static Boolean IsAvailable(string placement) {
            if (Application.platform != RuntimePlatform.Android) return false;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                return jc.CallStatic<Boolean>("isInterstitialAvailable", placement);
            }
        }

        public static void chartboostShowForLocation(string location) {
            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("chartboostLocationShow", location);
            }
        }

        public static Boolean chartboostIsAvailableForLocation(string location) {
            if (Application.platform != RuntimePlatform.Android) return false;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                return jc.CallStatic<Boolean>("chartboostLocationIsAvailable", location);
            }
        }

        public static void chartboostFetchForLocation(string location) {
            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("chartboostLocationFetch", location);
            }
        }
    }
    #endif
    #endregion
}