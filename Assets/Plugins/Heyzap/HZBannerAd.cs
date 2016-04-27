//
//  HZBannerAd.cs
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
    /// Use this class to show banner ads.
    /// </summary>
    public class HZBannerAd : MonoBehaviour {
        public delegate void AdDisplayListener(string state);
        
        private static AdDisplayListener adDisplayListener;
        private static HZBannerAd _instance = null;
        
        // these are reproduced here for convenience since they were here in old SDK versions
        /// <summary>
        /// Set `HZBannerShowOptions.Position` to this value to show ads at the top of the screen.
        /// </summary>
        [Obsolete("This constant has been relocated to HZBannerShowOptions")]
        public const string POSITION_TOP = HZBannerShowOptions.POSITION_TOP;
        /// <summary>
        /// Set `HZBannerShowOptions.Position` to this value to show ads at the bottom of the screen.
        /// </summary>
        [Obsolete("This constant has been relocated to HZBannerShowOptions")]
        public const string POSITION_BOTTOM = HZBannerShowOptions.POSITION_BOTTOM;
        
        /// <summary>
        /// Shows a banner ad with the given options.
        /// </summary>
        /// <param name="showOptions">The options with which to show the banner ad, or the defaults if <c>null</c> </param>
        public static void ShowWithOptions(HZBannerShowOptions showOptions) {
            if (showOptions == null) {
                showOptions = new HZBannerShowOptions();
            }
            
            #if UNITY_ANDROID
            HZBannerAdAndroid.ShowWithOptions(showOptions);
            #endif
            
            #if UNITY_IPHONE && !UNITY_EDITOR
            HZBannerAdIOS.ShowWithOptions(showOptions);
            #endif
        }
        
        /// <summary>
        /// Hides the current banner ad, if there is one, from the view. The next call to ShowWithOptions will unhide the banner ad hidden by this method.
        /// </summary>
        public static void Hide() {
            #if UNITY_ANDROID
            HZBannerAdAndroid.Hide();
            #endif
            
            #if UNITY_IPHONE && !UNITY_EDITOR
            HZBannerAdIOS.Hide();
            #endif
        }
        
        /// <summary>
        /// Destroys the current banner ad, if there is one. The next call to ShowWithOptions() will create a new banner ad.
        /// </summary>
        public static void Destroy() {
            #if UNITY_ANDROID
            HZBannerAdAndroid.Destroy();
            #endif
            
            #if UNITY_IPHONE && !UNITY_EDITOR
            HZBannerAdIOS.Destroy();
            #endif
        }
        
        /// <summary>
        /// Sets the AdDisplayListener for banner ads, which will receive callbacks regarding the state of banner ads.
        /// </summary>
        public static void SetDisplayListener(AdDisplayListener listener) {
            HZBannerAd.adDisplayListener = listener;
        }
        
        #region Internal methods
        public static void InitReceiver(){
            if (_instance == null) {
                GameObject receiverObject = new GameObject("HZBannerAd");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<HZBannerAd>();
            }
        }
        
        public void SetCallback(string message) {
            if (HZBannerAd.adDisplayListener != null) {
				HZBannerAd.adDisplayListener(message);
			}
        }
        
        #endregion
    }
    
    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class HZBannerAdIOS : MonoBehaviour {
        [DllImport ("__Internal")]
        private static extern void hz_ads_show_banner(string position);
        [DllImport ("__Internal")]
        private static extern bool hz_ads_hide_banner();
        [DllImport ("__Internal")]
        private static extern bool hz_ads_destroy_banner();
        
        public static void ShowWithOptions(HZBannerShowOptions showOptions) {
            hz_ads_show_banner(showOptions.Position);
        }
        
        public static bool Hide() {
            return hz_ads_hide_banner();
        }
        
        public static void Destroy() {
            hz_ads_destroy_banner();
        }
    }
    #endif
    
    #if UNITY_ANDROID
    public class HZBannerAdAndroid : MonoBehaviour {
        
        public static void ShowWithOptions(HZBannerShowOptions showOptions) {
            if(Application.platform != RuntimePlatform.Android) return;
            
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("showBanner", showOptions.Position);
            }
        }
        
        public static void Hide() {
            if(Application.platform != RuntimePlatform.Android) return;
            
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("hideBanner"); 
            }
        }
        
        public static void Destroy() {
            if(Application.platform != RuntimePlatform.Android) return;
            
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("destroyBanner"); 
            }
        }
        
    }
    #endif
    #endregion
}