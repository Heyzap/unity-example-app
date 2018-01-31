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
        public delegate void NetworkCallbackListener(string network, string callback);

        private static NetworkCallbackListener networkCallbackListener;
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
        /// Currently, only AdMob, FAN, and Heyzap Exchange use this option's value. This value will be `OR`ed with the per-network server-side setting we provide on your Mediation Settings dashboard.
        /// </summary>
        public const int FLAG_CHILD_DIRECTED_ADS = 1 << 6; // 64
        #endregion

        public const string DEFAULT_TAG = "default";
        
        #region String constants to expect in Ad Listener & network callbacks
        // `NetworkCallback` is a bit of a misnomer. The callback constants here are both for "network callbacks" and for ad listener callbacks. We should refactor these into two classes in the next major SDK.
        public static class NetworkCallback {
            public const string INITIALIZED = "initialized";
            public const string SHOW = "show";
            public const string SHOW_FAILED = "failed";
            public const string AVAILABLE = "available";
            public const string HIDE = "hide";
            public const string FETCH_FAILED = "fetch_failed";
            public const string CLICK = "click";
            public const string DISMISS = "dismiss";
            public const string INCENTIVIZED_RESULT_COMPLETE = "incentivized_result_complete";
            public const string INCENTIVIZED_RESULT_INCOMPLETE = "incentivized_result_incomplete";
            public const string AUDIO_STARTING = "audio_starting";
            public const string AUDIO_FINISHED = "audio_finished";

            // these banner state callbacks are currently sent in Android, but they were removed for iOS
            public const string BANNER_LOADED = "banner-loaded";
            public const string BANNER_CLICK = "banner-click";
            public const string BANNER_HIDE = "banner-hide";
            public const string BANNER_DISMISS = "banner-dismiss";
            public const string BANNER_FETCH_FAILED = "banner-fetch_failed";

            public const string LEAVE_APPLICATION = "leave_application";

            // Facebook Specific
            public const string FACEBOOK_LOGGING_IMPRESSION = "logging_impression";
        }
        #endregion

        #region Network names
        public static class Network {
            public const string HEYZAP = "heyzap";
            public const string HEYZAP_CROSS_PROMO = "heyzap_cross_promo";
            public const string HEYZAP_EXCHANGE = "heyzap_exchange";
            public const string FACEBOOK = "facebook";
            public const string UNITYADS = "unityads";
            public const string APPLOVIN = "applovin";
            public const string VUNGLE = "vungle";
            public const string CHARTBOOST = "chartboost";
            public const string ADCOLONY = "adcolony";
            public const string ADMOB = "admob";
            public const string IAD = "iad";
            public const string LEADBOLT = "leadbolt";
            public const string INMOBI = "inmobi";
            public const string DOMOB = "domob";
            public const string MOPUB = "mopub";
            public const string FYBER_EXCHANGE = "fyber_exchange";
        }
        #endregion

        #region Public API
        /// <summary>
        /// Starts the Heyzap SDK. Call this method as soon as possible in your app to ensure Heyzap has time to initialize before you want to show an ad.
        /// </summary>
        /// <param name="publisher_id">Your publisher ID. This can be found on your Heyzap dashboards - see https://developers.heyzap.com/docs/unity_sdk_setup_and_requirements for more information.</param>
        /// <param name="options">A bitmask of options you can pass to this call to change the way Heyzap will work.</param>
        public static void Start(string publisher_id, int options) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HeyzapAdsAndroid.Start(publisher_id, options);
                #elif UNITY_IPHONE
                    HeyzapAdsIOS.Start(publisher_id, options);
                #endif
            #else
                UnityEngine.Debug.LogError("Call received to start the Heyzap SDK, but the SDK does not function in the editor. You must use a device/emulator to receive/test ads.");
            #endif

            HeyzapAds.InitReceiver();
            HZInterstitialAd.InitReceiver();
            HZVideoAd.InitReceiver();
            HZIncentivizedAd.InitReceiver();
            HZBannerAd.InitReceiver();
            HZOfferWallAd.InitReceiver();
            HZDemographics.InitReceiver();
        }
        
        /// <summary>
        /// Returns the remote data you've set on the Heyzap Dashboards, which will be a JSON dictionary in string format.
        /// </summary>
        public static string GetRemoteData(){
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return HeyzapAdsAndroid.GetRemoteData();
                #elif UNITY_IPHONE
                    return HeyzapAdsIOS.GetRemoteData();
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to retrieve remote data from the Heyzap SDK, but the SDK does not function in the editor. You must use a device/emulator to use the remote data feature.");
                return "{}";
            #endif
        }

        /// <summary>
        /// Shows the mediation test suite.
        /// </summary>
        public static void ShowMediationTestSuite() {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HeyzapAdsAndroid.ShowMediationTestSuite();
                #elif UNITY_IPHONE
                    HeyzapAdsIOS.ShowMediationTestSuite();
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to show the Heyzap SDK test suite, but the SDK does not function in the editor. You must use a device/emulator to use the test suite.");
            #endif
        }
        
        /// <summary>
        /// (Android only) Call this method in your back button pressed handler to make sure the back button does what the user should expect when ads are showing.
        /// </summary>
        /// <returns><c>true</c>, if Heyzap handled the back button press (in which case your code should not do anything else), and <c>false</c> if Heyzap did not handle the back button press (in which case your app may want to do something).</returns>
        public static Boolean OnBackPressed() {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return HeyzapAdsAndroid.OnBackPressed();
                #elif UNITY_IPHONE
                    return HeyzapAdsIOS.OnBackPressed();
                #endif
            #else
                return false;
            #endif
        }

        /// <summary>
        /// Returns whether or not the given network has been initialized by Heyzap yet.
        /// </summary>
        /// <returns><c>true</c> if is network initialized the specified network; otherwise, <c>false</c>.</returns>
        /// <param name="network">The name of the network in question. Use the strings in HeyzapAds.Network to ensure the name matches what we expect.</param>
        public static Boolean IsNetworkInitialized(string network) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return HeyzapAdsAndroid.IsNetworkInitialized(network);
                #elif UNITY_IPHONE
                    return HeyzapAdsIOS.IsNetworkInitialized(network);
                #endif
            #else
                return false;
            #endif
        }
        
        /// <summary>
        /// Sets the NetworkCallbackListener, which receives messages about specific networks, such as when a specific network fetches an ad.
        /// </summary>
        public static void SetNetworkCallbackListener(NetworkCallbackListener listener) {
            networkCallbackListener = listener;
        }
        
        /// <summary>
        /// Pauses expensive work, like ad fetches, until ResumeExpensiveWork() is called. Note that calling this method will affect ad availability.
        /// </summary>
        public static void PauseExpensiveWork() {
            #if UNITY_IPHONE && !UNITY_EDITOR
                HeyzapAdsIOS.PauseExpensiveWork();
            #elif UNITY_ANDROID && !UNITY_EDITOR
                HeyzapAdsAndroid.PauseExpensiveWork();
            #endif
        }

        /// <summary>
        /// Unpauses expensive work, like ad fetches. Only relevant after a call to PauseExpensiveWork().
        /// </summary>
        public static void ResumeExpensiveWork() {
            #if UNITY_IPHONE && !UNITY_EDITOR
                HeyzapAdsIOS.ResumeExpensiveWork();
            #elif UNITY_ANDROID && !UNITY_EDITOR
                HeyzapAdsAndroid.ResumeExpensiveWork();
            #endif
        }

        /// <summary>
        /// Enables verbose debug logging for the Heyzap SDK. For third party logging, <see cref="ShowThirdPartyDebugLogs()"/>.
        /// </summary>
        public static void ShowDebugLogs() {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HeyzapAdsAndroid.ShowDebugLogs();
                #elif UNITY_IPHONE
                    HeyzapAdsIOS.ShowDebugLogs();
                #endif
            #else
            #endif
        }

        /// <summary>
        /// Hides all debug logs coming from the Heyzap SDK. For third party logging, <see cref="HideThirdPartyDebugLogs()"/>.
        /// </summary>
        public static void HideDebugLogs() {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HeyzapAdsAndroid.HideDebugLogs();
                #elif UNITY_IPHONE
                    HeyzapAdsIOS.HideDebugLogs();
                #endif
            #else
            #endif
        }

        /// <summary>
        /// (iOS only currently) Enables verbose debug logging for all the mediated SDKs we can.
        /// You should call this method before starting the Heyzap SDK if you wish to enable these logs, since some SDK implementations only consider this parameter when initialized.
        /// </summary>
        public static void ShowThirdPartyDebugLogs() {
            #if UNITY_IPHONE && !UNITY_EDITOR
            HeyzapAdsIOS.ShowThirdPartyDebugLogs();
            #endif
        }

        /// <summary>
        /// (iOS only currently) Disables verbose debug logging for all the mediated SDKs we can.
        /// Only some networks' logs will be turned off, since some SDK implementations only consider this parameter when initialized.
        /// </summary>
        public static void HideThirdPartyDebugLogs() {
            #if UNITY_IPHONE && !UNITY_EDITOR
            HeyzapAdsIOS.HideThirdPartyDebugLogs();
            #endif
        }

        /// <summary>
        /// Set the bundle ID (i.e.: `com.name.of.company.and.app`) on Android and iOS to something other than what you have set in the Unity Editor's Player Settings menu.
        /// Note: This method MUST be called BEFORE calling `Start` on the SDK.
        /// This method is mostly useful for debugging purposes; for instance, you might use this to use a test account & bundle ID on debug builds.
        /// </summary>
        /// <param name="bundleID">The new bundle ID you want to use. This is how we link this app to your dashboard account on developers.heyzap.com .</param>
        public static void SetBundleIdentifier(string bundleID) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HeyzapAdsAndroid.SetBundleIdentifier(bundleID);
                #elif UNITY_IPHONE
                    HeyzapAdsIOS.SetBundleIdentifier(bundleID);
                #endif
            #else
            #endif
        }

        /// <summary>
        /// Adds the device as a Facebook Audience Network test device, allowing you to receive test ads when FAN would otherwise not give you fill. This API is only supported on iOS.
        /// </summary>
        /// <param name="device_id">The Device ID that FAN prints to the iOS console</param>
        public static void AddFacebookTestDevice(string device_id) {
            #if UNITY_IPHONE && !UNITY_EDITOR
            HeyzapAdsIOS.AddFacebookTestDevice(device_id);
            #endif
        }
        #endregion

        #region Internal methods
        public void SetNetworkCallbackMessage(string message) {
            string[] networkStateParams = message.Split(',');
            SetNetworkCallback(networkStateParams[0], networkStateParams[1]); 
        }

        protected static void SetNetworkCallback(string network, string callback) {
            if (networkCallbackListener != null) {
                networkCallbackListener(network, callback);
            }
        }

        public static void InitReceiver(){
            if (_instance == null) {
                GameObject receiverObject = new GameObject("HeyzapAds");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<HeyzapAds>();
            }
        }

        public static string TagForString(string tag) {
            if (tag == null) {
                tag = HeyzapAds.DEFAULT_TAG;
            }
            
            return tag;
        }
        #endregion
    }

    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class HeyzapAdsIOS : MonoBehaviour {

        [DllImport ("__Internal")]
        private static extern void hz_ads_start_app(string publisher_id, int flags);

        [DllImport ("__Internal")]
        private static extern void hz_ads_show_mediation_debug_view_controller();

        [DllImport ("__Internal")]
        private static extern string hz_ads_get_remote_data();

        [DllImport ("__Internal")]
        private static extern bool hz_ads_is_network_initialized(string network);

        [DllImport ("__Internal")]
        private static extern void hz_pause_expensive_work();

        [DllImport ("__Internal")]
        private static extern void hz_resume_expensive_work();

        [DllImport ("__Internal")]
        private static extern void hz_ads_hide_debug_logs();

        [DllImport ("__Internal")]
        private static extern void hz_ads_show_debug_logs();

        [DllImport ("__Internal")]
        private static extern void hz_ads_hide_third_party_debug_logs();

        [DllImport ("__Internal")]
        private static extern void hz_ads_show_third_party_debug_logs();

        [DllImport ("__Internal")]
        private static extern void hz_ads_set_bundle_identifier(string bundleID);

        [DllImport ("__Internal")]
        private static extern void hz_add_facebook_test_device(string device_id);

        public static void Start(string publisher_id, int options=0) {
            hz_ads_start_app(publisher_id, options);
        }
        
        public static void ShowMediationTestSuite() {
            hz_ads_show_mediation_debug_view_controller();
        }

        public static Boolean OnBackPressed(){
            return false;
        }

        public static bool IsNetworkInitialized(string network) {
            return hz_ads_is_network_initialized(network);
        }

        public static string GetRemoteData(){
            return hz_ads_get_remote_data();
        }

        public static void PauseExpensiveWork() {
            hz_pause_expensive_work();
        }

        public static void ResumeExpensiveWork() {
            hz_resume_expensive_work();
        }

        public static void ShowDebugLogs() {
            hz_ads_show_debug_logs();
        }
        
        public static void HideDebugLogs() {
            hz_ads_hide_debug_logs();
        }

        public static void ShowThirdPartyDebugLogs() {
            hz_ads_show_third_party_debug_logs();
        }
        
        public static void HideThirdPartyDebugLogs() {
            hz_ads_hide_third_party_debug_logs();
        }

        public static void SetBundleIdentifier(string bundleID) {
            hz_ads_set_bundle_identifier(bundleID);
        }

        public static void AddFacebookTestDevice(string device_id) {
            hz_add_facebook_test_device(device_id);
        }
    }
    #endif

    #if UNITY_ANDROID && !UNITY_EDITOR
    public class HeyzapAdsAndroid : MonoBehaviour {
        public static void Start(string publisher_id, int options=0) {
            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("start", publisher_id, options);
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

        public static void ResumeExpensiveWork() {
            if(Application.platform != RuntimePlatform.Android) return;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
                jc.CallStatic("resumeExpensiveWork");
            }
        }

        public static void PauseExpensiveWork() {
            if(Application.platform != RuntimePlatform.Android) return;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
                jc.CallStatic("pauseExpensiveWork");
            }
        }

        public static void SetBundleIdentifier(string bundleID) {
            if(Application.platform != RuntimePlatform.Android) return;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.ads.HeyzapAds")) {
                jc.CallStatic("setBundleId", bundleID);
            }
        }
    }
    #endif
    #endregion
}
