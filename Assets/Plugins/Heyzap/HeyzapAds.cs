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

        [Obsolete("Use FLAG_AMAZON instead - we refactored the flags to be consistently named.")]
        public const int AMAZON = FLAG_AMAZON;
        [Obsolete("Use FLAG_DISABLE_MEDIATION instead - we refactored the flags to be consistently named.")]
        public const int DISABLE_MEDIATION = FLAG_DISABLE_MEDIATION;
        #endregion

        #region Public API
        /// <summary>
        /// Starts the Heyzap SDK. Call this method as soon as possible in your app to ensure Fyber has time to initialize before you want to show an ad.
        /// </summary>
        /// <param name="appID"> Your Fyber App ID. See the transition docs or the Fyber Dashboard for this value.</param>
        /// <param name="securityToken"> Your Fyber Security Token. See the transition docs or the Fyber Dashboard for this value.</param>
        /// <param name="options">A bitmask of options you can pass to this call to change the way Heyzap will work.</param>
        public static void Start(string appID, string securityToken, int options) {
            #if !UNITY_EDITOR

            #if UNITY_ANDROID
			HeyzapAdsAndroid.Start(appID, securityToken, options);
            #endif

            #if UNITY_IPHONE
            HeyzapAdsIOS.Start(appID, securityToken, options);
            #endif

            HeyzapAds.InitReceiver();
            HZInterstitialAd.InitReceiver();
            HZIncentivizedAd.InitReceiver();

            #endif
        }

       
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
        private static extern void hz_ads_start_app(string appID, string securityToken, int flags);

        public static void Start(string appID, string securityToken, int options=0) {
            hz_ads_start_app(appID, securityToken, options);
        }


    }
    #endif

    #if UNITY_ANDROID
    public class HeyzapAdsAndroid : MonoBehaviour {
		public static void Start(string appId, string securityToken, int options=0) {
            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
				jc.CallStatic("start", appId, securityToken, options);
            }
        }

        public static Boolean IsNetworkInitialized(string network) {
            if (Application.platform != RuntimePlatform.Android) return false;

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                return jc.CallStatic<Boolean>("isNetworkInitialized", network);
            }
        }

        public static Boolean OnBackPressed(){
            if(Application.platform != RuntimePlatform.Android) return false;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
                return jc.CallStatic<Boolean>("onBackPressed");
            }
        }

        public static void ShowMediationTestSuite() {
            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
                jc.CallStatic("showNetworkActivity");
            }
        }

        public static string GetRemoteData() {
            if(Application.platform != RuntimePlatform.Android) return "{}";
            AndroidJNIHelper.debug = false;

            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
                return jc.CallStatic<String>("getCustomPublisherData");
            }
        }

        public static void ShowDebugLogs() {
            if(Application.platform != RuntimePlatform.Android) return;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
                jc.CallStatic("showDebugLogs");
            }
        }

        public static void HideDebugLogs() {
            if(Application.platform != RuntimePlatform.Android) return;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
                jc.CallStatic("hideDebugLogs");
            }
        }
    }
    #endif
    #endregion
}
