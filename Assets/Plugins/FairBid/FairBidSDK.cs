//  FairBid.cs
//
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
using System.Linq;

namespace FairBid {
    /// <summary>
    /// FairBid wrapper for iOS and Android via Unity. For more information, see https://developers.heyzap.com/docs/unity_sdk_setup_and_requirements .
    /// </summary>
    public class FairBidSDK : MonoBehaviour {
        public const string Version = "2.0.0";
        public delegate void NetworkCallbackListener(string network, string callback);

        private static NetworkCallbackListener networkCallbackListener;
        private static FairBidSDK _instance;

        #region Flags for the call to FairBid.StartWithOptions()
        /// <summary>
        /// Use this flag to start the FairBid SDK with no extra configuration options. This is the default behavior if no options are passed when the SDK is started.
        /// </summary>
        public const int FLAG_NO_OPTIONS = 0 << 0; // 0
        /// <summary>
        /// Use this flag to disable automatic prefetching of ads. You must call a `Request` method for every ad type before a matching call to a `Show` method.
        /// </summary>
        public const int FLAG_DISABLE_AUTOMATIC_REQUESTING = 1 << 0; // 1
        /// <summary>
        /// Use this flag to disable all advertising functionality of the FairBid SDK. This should only be used if you're integrating the SDK solely as an install tracker.
        /// </summary>
        public const int FLAG_INSTALL_TRACKING_ONLY = 1 << 1; // 2
        /// <summary>
        /// Use this flag to disable the mediation features of the FairBid SDK. Only FairBid ads will be available.
        /// You should set this flag if you are using FairBid through another mediation tool to avoid potential conflicts.
        /// </summary>
        public const int FLAG_DISABLE_MEDIATION = 1 << 3; // 8
        /// <summary>
        /// Use this flag to to mark mediated ads as "child-directed". This value will be passed on to networks that support sending such an option (for purposes of the Children's Online Privacy Protection Act (COPPA)).
        /// Currently, only AdMob and FAN use this option's value. This value will be `OR`ed with the per-network server-side setting we provide on your Mediation Settings dashboard.
        /// </summary>
        public const int FLAG_CHILD_DIRECTED_ADS = 1 << 6; // 64
        #endregion
        
        #region String constants to expect in Ad Listener & network callbacks
        // `NetworkCallback` is a bit of a misnomer. The callback constants here are both for "network callbacks" and for ad listener callbacks. We should refactor these into two classes in the next major SDK.
        public static class NetworkCallback {
            public const string INITIALIZED = "initialized";
            public const string SHOW = "show";
            public const string SHOW_FAILED = "failed";
            public const string AVAILABLE = "available";
            public const string HIDE = "hide";
            public const string REQUEST_FAILED = "request_failed";
            public const string CLICK = "click";
            public const string DISMISS = "dismiss";
            public const string REWARDED_RESULT_COMPLETE = "rewarded_result_complete";
            public const string REWARDED_RESULT_INCOMPLETE = "rewarded_result_incomplete";
            public const string AUDIO_STARTING = "audio_starting";
            public const string AUDIO_FINISHED = "audio_finished";

            // these banner state callbacks are currently sent in Android, but they were removed for iOS
            public const string BANNER_LOADED = "banner-loaded";
            public const string BANNER_CLICK = "banner-click";
            public const string BANNER_HIDE = "banner-hide";
            public const string BANNER_DISMISS = "banner-dismiss";
            public const string BANNER_REQUEST_FAILED = "banner-request_failed";

            public const string LEAVE_APPLICATION = "leave_application";

            // Facebook Specific
            public const string FACEBOOK_LOGGING_IMPRESSION = "logging_impression";
        }
        #endregion

        #region Network names
        public static class Network {
            public const string ADCOLONY = "adcolony";
            public const string ADMOB = "admob";
            public const string APPLOVIN = "applovin";
            public const string CHARTBOOST = "chartboost";
            public const string FACEBOOK = "facebook";
            public const string HYPRMX = "hyprmx";
            public const string INMOBI = "inmobi";
            public const string IRONSOURCE = "iron_source";
            public const string MOPUB = "mopub";
            public const string TAPJOY = "tapjoy";
            public const string UNITYADS = "unityads";
            public const string VUNGLE = "vungle";
        }
        #endregion

        #region Public API
        /// <summary>
        /// Starts the FairBid SDK. Call this method as soon as possible in your app to ensure FairBid has time to initialize before you want to show an ad.
        /// </summary>
        /// <param name="publisher_id">Your publisher ID. This can be found on your FairBid dashboards - see https://developers.heyzap.com/docs/unity_sdk_setup_and_requirements for more information.</param>
        /// <param name="options">A bitmask of options you can pass to this call to change the way FairBid will work.</param>
        public static void Start(string publisher_id, int options) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    FairBidAndroid.SetPluginVersion(Version);
                    FairBidAndroid.Start(publisher_id, options);
                #elif UNITY_IPHONE
                    FairBidIOS.SetPluginVersion(Version);
                    FairBidIOS.Start(publisher_id, options);
                #endif
            #else
                Debug.LogError("Call received to start the FairBid SDK, but the SDK does not function in the editor. You must use a device/emulator to receive/test ads.");
            #endif

            InitReceiver();
            Interstitial.InitReceiver();
            Rewarded.InitReceiver();
            Banner.InitReceiver();
            Demographics.InitReceiver();
        }

        /// <summary>
        /// Sets User's consent under GDPR. Fyber will only be able to show targeted advertising if the user consented. Only call this method if the user explicitly gave or denied consent.
        /// </summary>
        /// <param name="isGdprConsentGiven">true if user gave consent to receive targeted advertisement, false otherwise</param>
        public static void SetGdprConsent(Boolean isGdprConsentGiven) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    FairBidAndroid.SetGdprConsent(isGdprConsentGiven);
                #elif UNITY_IPHONE
                    FairBidIOS.SetGdprConsent(isGdprConsentGiven);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to set the GDPR consent, but the SDK does not function in the editor. You must use a device/emulator to set the GDPR consent.");
            #endif
        }

        /// <summary>
        /// Sets User's consent data under GDPR. FairBid SDK will use this information to provide optimal targeted advertising without infringing GDPR.
        /// </summary>
        /// <param name="gdprConsentData">A Dictionary of key-value pairs containing GDPR related information</param>
        static public void SetGdprConsentData(Dictionary<string, string> gdprConsentData) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                string gdprConsentDataAsJsonString = null;
                if (gdprConsentData != null) {
                    Dictionary<string, string> validatedGdprConsentData = new Dictionary<string, string>();
                    foreach(KeyValuePair<string, string> entry in gdprConsentData) {
                        if (entry.Value != null) {
                            validatedGdprConsentData.Add(entry.Key, entry.Value);
                        }
                    }
                    gdprConsentDataAsJsonString = GetGdprConsentDataAsJsonString(validatedGdprConsentData);
                }

                #if UNITY_ANDROID
                    FairBidAndroid.SetGdprConsentData(gdprConsentDataAsJsonString);
                #elif UNITY_IPHONE
                    FairBidIOS.SetGdprConsentData(gdprConsentDataAsJsonString);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to set the GDPR consent data, but the SDK does not function in the editor. You must use a device/emulator to set the GDPR consent data.");
            #endif
        }

        /// <summary>
        /// Clears all GDPR related information. This means removing any GDPR consent Data and restoring the GDPR consent to "unknown"
        /// </summary>
        static public void ClearGdprConsentData() {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    FairBidAndroid.ClearGdprConsentData();
                #elif UNITY_IPHONE
                    FairBidIOS.ClearGdprConsentData();
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to clear the GDPR consent data, but the SDK does not function in the editor. You must use a device/emulator to set the GDPR consent data.");
            #endif
        }

        /// <summary>
        /// Shows the test suite.
        /// </summary>
        public static void ShowTestSuite() {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    FairBidAndroid.ShowTestSuite();
                #elif UNITY_IPHONE
                    FairBidIOS.ShowTestSuite();
                #endif
            #else
                Debug.LogWarning("Call received to show the FairBid SDK test suite, but the SDK does not function in the editor. You must use a device/emulator to use the test suite.");
            #endif
        }

        /// <summary>
        /// (Android only) Call this method in your back button pressed handler to make sure the back button does what the user should expect when ads are showing.
        /// </summary>
        /// <returns><c>true</c>, if FairBid handled the back button press (in which case your code should not do anything else), and <c>false</c> if FairBid did not handle the back button press (in which case your app may want to do something).</returns>
        public static Boolean OnBackPressed() {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return FairBidAndroid.OnBackPressed();
                #elif UNITY_IPHONE
                    return FairBidIOS.OnBackPressed();
                #endif
            #else
                return false;
            #endif
        }

        /// <summary>
        /// Returns whether or not the given network has been initialized by FairBid yet.
        /// </summary>
        /// <returns><c>true</c> if is network initialized the specified network; otherwise, <c>false</c>.</returns>
        /// <param name="network">The name of the network in question. Use the strings in FairBid.Network to ensure the name matches what we expect.</param>
        public static Boolean IsNetworkInitialized(string network) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return FairBidAndroid.IsNetworkInitialized(network);
                #elif UNITY_IPHONE
                    return FairBidIOS.IsNetworkInitialized(network);
                #endif
            #else
                return false;
            #endif
        }
        
        /// <summary>
        /// Sets the NetworkCallbackListener, which receives messages about specific networks, such as when a specific network requests an ad.
        /// </summary>
        public static void SetNetworkCallbackListener(NetworkCallbackListener listener) {
            networkCallbackListener = listener;
        }

        /// <summary>
        /// Enables verbose debug logging for the FairBid SDK. For third party logging, <see cref="ShowThirdPartyDebugLogs()"/>.
        /// </summary>
        public static void ShowDebugLogs() {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    FairBidAndroid.ShowDebugLogs();
                #elif UNITY_IPHONE
                    FairBidIOS.ShowDebugLogs();
                #endif
            #else
            #endif
        }

        /// <summary>
        /// Hides all debug logs coming from the FairBid SDK. For third party logging, <see cref="HideThirdPartyDebugLogs()"/>.
        /// </summary>
        public static void HideDebugLogs() {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    FairBidAndroid.HideDebugLogs();
                #elif UNITY_IPHONE
                    FairBidIOS.HideDebugLogs();
                #endif
            #else
            #endif
        }

        /// <summary>
        /// (iOS only currently) Enables verbose debug logging for all the mediated SDKs we can.
        /// You should call this method before starting the FairBid SDK if you wish to enable these logs, since some SDK implementations only consider this parameter when initialized.
        /// </summary>
        public static void ShowThirdPartyDebugLogs() {
            #if UNITY_IPHONE && !UNITY_EDITOR
                FairBidIOS.ShowThirdPartyDebugLogs();
            #endif
        }

        /// <summary>
        /// (iOS only currently) Disables verbose debug logging for all the mediated SDKs we can.
        /// Only some networks' logs will be turned off, since some SDK implementations only consider this parameter when initialized.
        /// </summary>
        public static void HideThirdPartyDebugLogs() {
            #if UNITY_IPHONE && !UNITY_EDITOR
                FairBidIOS.HideThirdPartyDebugLogs();
            #endif
        }

        /// <summary>
        /// Adds the device as a Facebook Audience Network test device, allowing you to receive test ads when FAN would otherwise not give you fill. This API is only supported on iOS.
        /// </summary>
        /// <param name="device_id">The Device ID that FAN prints to the iOS console</param>
        public static void AddFacebookTestDevice(string device_id) {
            #if UNITY_IPHONE && !UNITY_EDITOR
                FairBidIOS.AddFacebookTestDevice(device_id);
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

        public static void InitReceiver() {
            if (_instance == null) {
                GameObject receiverObject = new GameObject("FairBid");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<FairBidSDK>();
            }
        }

        // Within Unity's .NET framework we don't have a stock solution for converting objets to Json so we need to implement a custom solution 
        static private string GetGdprConsentDataAsJsonString(Dictionary<string, string> gdprConsentData) {
            var entries = gdprConsentData.Select(d =>
                string.Format("\"{0}\": \"{1}\"", d.Key, d.Value)
            );
            return "{" + string.Join(",", entries.ToArray()) + "}";
        }
        #endregion
    }

    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class FairBidIOS : MonoBehaviour
    {
        [DllImport ("__Internal")]
        private static extern void fyb_sdk_set_plugin_version(string pluginVersion);

        [DllImport ("__Internal")]
        private static extern void fyb_sdk_start_app(string publisher_id, int flags);

        [DllImport ("__Internal")]
        private static extern void fyb_sdk_show_test_suite();

        [DllImport ("__Internal")]
        private static extern void fyb_sdk_set_gdpr_consent(Boolean isGdprConsentGiven);

        [DllImport ("__Internal")]
        private static extern void fyb_sdk_set_gdpr_consent_data(string gdprConsentData);

        [DllImport ("__Internal")]
        private static extern void fyb_sdk_clear_gdpr_consent_data();

        [DllImport ("__Internal")]
        private static extern bool fyb_sdk_is_network_initialized(string network);

        [DllImport ("__Internal")]
        private static extern void fyb_sdk_hide_debug_logs();

        [DllImport ("__Internal")]
        private static extern void fyb_sdk_show_debug_logs();

        [DllImport ("__Internal")]
        private static extern void fyb_sdk_hide_third_party_debug_logs();

        [DllImport ("__Internal")]
        private static extern void fyb_sdk_show_third_party_debug_logs();

        [DllImport ("__Internal")]
        private static extern void fyb_add_facebook_test_device(string device_id);

        public static void SetPluginVersion(string pluginVersion) {
            fyb_sdk_set_plugin_version(pluginVersion);
        }

        public static void Start(string publisher_id, int options=0) {
            fyb_sdk_start_app(publisher_id, options);
        }

        public static void ShowTestSuite() {
            fyb_sdk_show_test_suite();
        }

        public static Boolean OnBackPressed() {
            return false;
        }

        public static bool IsNetworkInitialized(string network) {
            return fyb_sdk_is_network_initialized(network);
        }

        public static void SetGdprConsent(Boolean isGdprConsentGiven) {
            fyb_sdk_set_gdpr_consent(isGdprConsentGiven);
        }

        public static void SetGdprConsentData(string gdprConsentData) {
            fyb_sdk_set_gdpr_consent_data(gdprConsentData);
        }

        public static void ClearGdprConsentData() {
            fyb_sdk_clear_gdpr_consent_data();
        }

        public static void ShowDebugLogs() {
            fyb_sdk_show_debug_logs();
        }

        public static void HideDebugLogs() {
            fyb_sdk_hide_debug_logs();
        }

        public static void ShowThirdPartyDebugLogs() {
            fyb_sdk_show_third_party_debug_logs();
        }

        public static void HideThirdPartyDebugLogs() {
            fyb_sdk_hide_third_party_debug_logs();
        }

        public static void AddFacebookTestDevice(string device_id) {
            fyb_add_facebook_test_device(device_id);
        }
    }
    #endif

    #if UNITY_ANDROID && !UNITY_EDITOR
    public class FairBidAndroid : MonoBehaviour
    {
        public static void SetPluginVersion(string pluginVersion) {
            if (Application.platform != RuntimePlatform.Android) return;
             AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) {
                jc.CallStatic("addCustomParameter", "plugin_version", pluginVersion);
            }
        }
        
        public static void Start(string publisher_id, int options=0) {
            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("start", publisher_id, options);
            }
        }

        public static Boolean IsNetworkInitialized(string network) {
            if (Application.platform != RuntimePlatform.Android) return false;

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                return jc.CallStatic<Boolean>("isNetworkInitialized", network);
            }
        }

        public static Boolean OnBackPressed(){
            if (Application.platform != RuntimePlatform.Android) return false;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) {
                return jc.CallStatic<Boolean>("onBackPressed");
            }
        }

        public static void ShowTestSuite() {
            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) {
                jc.CallStatic("showTestSuite");
            }
        }

        public static void SetGdprConsent(Boolean isGdprConsentGiven) {
            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass javaClass = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) {
                javaClass.CallStatic("setGdprConsent", isGdprConsentGiven);
            }
        }

        public static void SetGdprConsentData(String gdprConsentData) {
            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass javaClass = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) {
                javaClass.CallStatic("setGdprConsentData", gdprConsentData);
            }
        }

        public static void ClearGdprConsentData() {
            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass javaClass = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) {
                javaClass.CallStatic("clearGdprConsentData");
            }
        }

        public static void ShowDebugLogs() {
            if(Application.platform != RuntimePlatform.Android) return;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) {
                jc.CallStatic("showDebugLogs");
            }
        }

        public static void HideDebugLogs() {
            if(Application.platform != RuntimePlatform.Android) return;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) {
                jc.CallStatic("hideDebugLogs");
            }
        }
    }
    #endif
    #endregion
}
