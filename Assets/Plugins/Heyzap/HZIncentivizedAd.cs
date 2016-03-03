//  HZIncentivizedAd.cs
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
    /// Use this class to show incentivized (rewarded) video ads.
    /// </summary>
    public class HZIncentivizedAd : MonoBehaviour {
        
        public delegate void AdDisplayListener(string state, string tag);
        private static AdDisplayListener adDisplayListener;
        private static HZIncentivizedAd _instance = null;

        /// <summary>
        /// Fetches an ad
        /// </summary>
        public static void Fetch() {

            #if UNITY_ANDROID
            HZIncentivizedAdAndroid.Fetch(null);
            #endif
            
            #if UNITY_IPHONE && !UNITY_EDITOR
            HZIncentivizedAdIOS.Fetch();
            #endif
        }
            
        /// <summary>
        /// Shows an ad
        /// </summary>
        public static void Show() {
            #if UNITY_ANDROID
            HZIncentivizedAdAndroid.Show();
            #endif

            #if UNITY_IPHONE && !UNITY_EDITOR
            HZIncentivizedAdIOS.Show();
            #endif
        }
            
        /// <summary>
        /// Returns whether or not an ad is available
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable() {
            #if UNITY_ANDROID
            return HZIncentivizedAdAndroid.IsAvailable(null);
            #elif UNITY_IPHONE && !UNITY_EDITOR
            return HZIncentivizedAdIOS.IsAvailable();
            #else
            return false;
            #endif
        }
        
        /// <summary>
        /// Sets the AdDisplayListener for incentivized ads, which will receive callbacks regarding the state of incentivized ads.
        /// </summary>
        public static void SetDisplayListener(AdDisplayListener listener) {
            HZIncentivizedAd.adDisplayListener = listener;
        }

        #region Internal methods
        public static void InitReceiver(){
            if (_instance == null) {
                GameObject receiverObject = new GameObject("HZIncentivizedAd");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<HZIncentivizedAd>();
            }
        }

        public void SetCallback(string message) {
            string[] displayStateParams = message.Split(',');
            HZIncentivizedAd.SetCallbackStateAndTag(displayStateParams[0], displayStateParams[1]); 
        }
        
        protected static void SetCallbackStateAndTag(string state, string tag) {
            if (HZIncentivizedAd.adDisplayListener != null) {
                HZIncentivizedAd.adDisplayListener(state, tag);
            }
        }
        #endregion
    }

    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class HZIncentivizedAdIOS : MonoBehaviour {
        [DllImport ("__Internal")]
        private static extern void hz_ads_show_incentivized();
        [DllImport ("__Internal")]
        private static extern void hz_ads_fetch_incentivized();
        [DllImport ("__Internal")]
        private static extern bool hz_ads_incentivized_is_available();
        

        public static void Show() {
            hz_ads_show_incentivized();
        }
        
        public static void Fetch() {
            hz_ads_fetch_incentivized();
        }
        
        public static bool IsAvailable() {
            return hz_ads_incentivized_is_available();
        }
    }
    #endif

    #if UNITY_ANDROID
    public class HZIncentivizedAdAndroid : MonoBehaviour {
        
        public static void ShowWithOptions(HZIncentivizedShowOptions showOptions) {
            if(Application.platform != RuntimePlatform.Android) return;
            
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("showIncentivized", showOptions.Tag, showOptions.IncentivizedInfo); 
            }
        }
        
        public static void Fetch(string tag) {
            if(Application.platform != RuntimePlatform.Android) return;
            
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("fetchIncentivized", tag); 
            }
        }
        
        public static Boolean IsAvailable(string tag) {
            if(Application.platform != RuntimePlatform.Android) return false;
            
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                return jc.CallStatic<Boolean>("isIncentivizedAvailable", tag);
            }
        }
    }
    #endif
    #endregion
}