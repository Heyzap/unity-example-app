//  HeyzapAds.cs
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
    /// Heyzap wrapper for iOS and Android via Unity. For more information, see https://developers.heyzap.com/docs/unity_sdk_setup_and_requirements .
    /// </summary>
    public class HeyzapAds : MonoBehaviour {

        private static HeyzapAds _instance = null;
        
        #region Flags for the call to HeyzapAds.StartWithOptions()
        /// <summary>
        /// Use this flag to start the Heyzap SDK with no extra configuration options. This is the default behavior if no options are passed when the SDK is started.
        /// </summary>
        public const int FLAG_NO_OPTIONS = 0 << 0; // 0
        /// <summary>
        /// Use this flag to disable automatic prefetching of ads. You must call a `Fetch` method for every ad unit before a matching call to a `Show` method.
        /// </summary>
        public const int FLAG_DISABLE_AUTOMATIC_FETCHING = 1 << 0; // 1
        /// <summary>
        /// Use this flag to disable all advertising functionality of the Heyzap SDK. This should only be used if you're integrating the SDK solely as an install tracker.
        /// </summary>
        public const int FLAG_INSTALL_TRACKING_ONLY = 1 << 1; // 2
        /// <summary>
        /// (Android only) Use this flag to tell the Heyzap SDK that this app is being distributed on the Amazon App Store.
        /// </summary>
        public const int FLAG_AMAZON = 1 << 2; // 4
        /// <summary>
        /// Use this flag to disable the mediation features of the Heyzap SDK. Only Heyzap ads will be available.
        /// You should set this flag if you are using Heyzap through another mediation tool to avoid potential conflicts.
        /// </summary>
        public const int FLAG_DISABLE_MEDIATION = 1 << 3; // 8
        /// <summary>
        /// (iOS only) Use this flag to stop the Heyzap SDK from automatically recording in-app purchases.
        /// </summary>
        public const int FLAG_DISABLE_AUTOMATIC_IAP_RECORDING = 1 << 4; // 16
        /// <summary>
        /// (Android only) Use this flag to disable all non-native ads & ad networks that don't support native ads.
        /// </summary>
        public const int FLAG_NATIVE_ADS_ONLY = 1 << 5; // 32
        /// <summary>
        /// Use this flag to to mark mediated ads as "child-directed". This value will be passed on to networks that support sending such an option (for purposes of the Children's Online Privacy Protection Act (COPPA)).
        /// Currently, only AdMob uses this option's value. The AdMob setting will be left alone if this flag is not passed when the Heyzap SDK is started.
        /// </summary>
        public const int FLAG_CHILD_DIRECTED_ADS = 1 << 6; // 64
        #endregion

        public const string DEFAULT_TAG = "default";

        #region String constants to expect in callbacks
        public static class CallbackConstants {
            // for interstitial and incentivized ads
            public const string SHOW = "show"; // ad shown successfully
            public const string AVAILABLE = "available"; // ad fetch success
            public const string HIDE = "hide"; // ad closed
            public const string SHOW_FAILED = "failed";
            public const string FETCH_FAILED = "fetch_failed";
            public const string AUDIO_STARTING = "audio_starting";
            public const string AUDIO_FINISHED = "audio_finished";

            // for incentivized ads only
            public const string INCENTIVIZED_RESULT_COMPLETE = "incentivized_result_complete";
            public const string INCENTIVIZED_RESULT_INCOMPLETE = "incentivized_result_incomplete";

            // for banner ads only
            public const string BANNER_LOADED = "loaded";
            public const string BANNER_ERROR = "error";
            public const string LEAVE_APPLICATION = "leave_application";
        }
        #endregion

        #region Public API
        /// <summary>
        /// Starts the Heyzap SDK. Call this method as soon as possible in your app to ensure Heyzap has time to initialize before you want to show an ad.
        /// </summary>
        /// <param name="publisher_id">Your publisher ID. This can be found on your Heyzap dashboards - see https://developers.heyzap.com/docs/unity_sdk_setup_and_requirements for more information.</param>
        /// <param name="options">A bitmask of options you can pass to this call to change the way Heyzap will work.</param>
        public static void Start(string publisher_id, int options) {
            #if !UNITY_EDITOR
            
            #if UNITY_ANDROID
            HeyzapAdsAndroid.Start(publisher_id, options);
            #endif
            
            #if UNITY_IPHONE
            HeyzapAdsIOS.Start(publisher_id, options);
            #endif
            
            HeyzapAds.InitReceiver();
            HZInterstitialAd.InitReceiver();
            HZIncentivizedAd.InitReceiver();
            HZBannerAd.InitReceiver();
            
            #endif
        }

        /// <summary>
        /// Shows the mediation test suite.
        /// </summary>
        public static void ShowMediationTestSuite() {
            #if UNITY_ANDROID
            HeyzapAdsAndroid.ShowMediationTestSuite();
            #endif

            #if UNITY_IPHONE && !UNITY_EDITOR
            HeyzapAdsIOS.ShowMediationTestSuite();
            #endif
        }

        /// Enables verbose debug logging for the Heyzap SDK. For third party logging, <see cref="ShowThirdPartyDebugLogs()"/>.
        /// </summary>
        public static void ShowDebugLogs() {
            #if UNITY_IPHONE && !UNITY_EDITOR
            HeyzapAdsIOS.ShowDebugLogs();
            #endif
        }
        
        /// <summary>
        /// Hides all debug logs coming from the Heyzap SDK. For third party logging, <see cref="HideThirdPartyDebugLogs()"/>.
        /// </summary>
        public static void HideDebugLogs() {
            #if UNITY_IPHONE && !UNITY_EDITOR
            HeyzapAdsIOS.HideDebugLogs();
            #endif
        }

        /// <summary>
        /// This method is not yet available in SDK 10.
        /// </summary>
        public static void PauseExpensiveWork() {}
        /// <summary>
        /// This method is not yet available in SDK 10.
        /// </summary>
        public static void ResumeExpensiveWork() {}
        /// <summary>
        /// This method is not yet available in SDK 10.
        /// </summary>
        public static void ShowThirdPartyDebugLogs() {}
        /// <summary>
        /// This method is not yet available in SDK 10.
        /// </summary>
        public static void HideThirdPartyDebugLogs() {}
        #endregion


        #region Internal methods

        public static void InitReceiver(){
            if (_instance == null) {
                GameObject receiverObject = new GameObject("HeyzapAds");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<HeyzapAds>();
            }
        }
        #endregion


    }

    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class HeyzapAdsIOS : MonoBehaviour {

        [DllImport ("__Internal")]
        private static extern void hz_ads_start_app(string publisher_id, int flags);

        public static void Start(string publisher_id, int options=0) {
            hz_ads_start_app(publisher_id, options);
        }

        [DllImport ("__Internal")]
        private static extern void hz_ads_show_debug_logs();

        public static void ShowDebugLogs() {
            hz_ads_show_debug_logs();
        }

        [DllImport ("__Internal")]
        private static extern void hz_ads_hide_debug_logs();

        public static void HideDebugLogs() {
            hz_ads_hide_debug_logs();
        }

        [DllImport ("__Internal")]
        private static extern void hz_ads_show_mediation_debug_view_controller();
        
        public static void ShowMediationTestSuite() {
            hz_ads_show_mediation_debug_view_controller();
        }

    }
    #endif

    #if UNITY_ANDROID
    public class HeyzapAdsAndroid : MonoBehaviour {
        public static void Start(string publisher_id, int options=0) {
            if(Application.platform != RuntimePlatform.Android) return;
            
            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("start", publisher_id, options);
            }
        }

        public static void ShowMediationTestSuite() {
            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
                jc.CallStatic("showNetworkActivity");
            }
        }
    }
    #endif
    #endregion
}
