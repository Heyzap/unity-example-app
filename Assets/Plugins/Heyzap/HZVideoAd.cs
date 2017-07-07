//
//  HZVideoAd.cs
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
    /// Use this class to show video ads.
    /// </summary>
    public class HZVideoAd : MonoBehaviour {
      
        public delegate void AdDisplayListener(string state, string tag);
        private static AdDisplayListener adDisplayListener;
        private static HZVideoAd _instance = null;
      
        //provided since JS can't use default parameters
        /// <summary>
        /// Shows an ad with the default options.
        /// </summary>
        public static void Show() {
            HZVideoAd.ShowWithOptions(null);
        }
        /// <summary>
        /// Shows an ad with the given options.
        /// </summary>
        /// <param name="showOptions"> The options to show the ad with, or the default options if <c>null</c></param> 
        public static void ShowWithOptions(HZShowOptions showOptions) {
            if (showOptions == null) {
                showOptions = new HZShowOptions();
            }

            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HZVideoAdAndroid.ShowWithOptions(showOptions);
                #elif UNITY_IPHONE
                    HZVideoAdIOS.ShowWithOptions(showOptions);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to show an HZVideoAd, but the SDK does not function in the editor. You must use a device/emulator to fetch/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(HeyzapAds.NetworkCallback.SHOW_FAILED, showOptions.Tag));
            #endif
        }
      
        //provided since JS can't use default parameters
        /// <summary>
        /// Fetches an ad for the default ad tag.
        /// </summary>
        public static void Fetch() {
            HZVideoAd.Fetch(null);
        }
        /// <summary>
        /// Fetches an ad for the given ad tag.
        /// </summary>
        /// <param name="tag">The ad tag to fetch an ad for.</param>
        public static void Fetch(string tag) {
            tag = HeyzapAds.TagForString(tag);

            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HZVideoAdAndroid.Fetch(tag);
                #elif UNITY_IPHONE
                    HZVideoAdIOS.Fetch(tag);
                #endif
            #else
                UnityEngine.Debug.LogWarning("Call received to fetch an HZVideoAd, but the SDK does not function in the editor. You must use a device/emulator to fetch/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(HeyzapAds.NetworkCallback.FETCH_FAILED, tag));
            #endif
        }
      
        //provided since JS can't use default parameters
        /// <summary>
        /// Returns whether or not an ad is available for the default ad tag.
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable() {
            return HZVideoAd.IsAvailable(null);
        }
        /// <summary>
        /// Returns whether or not an ad is available for the given ad tag.
        /// </summary>
        /// <returns><c>true</c>, if an ad is available, <c>false</c> otherwise.</returns>
        public static bool IsAvailable(string tag) {
            tag = HeyzapAds.TagForString(tag);

            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return HZVideoAdAndroid.IsAvailable(tag);
                #elif UNITY_IPHONE
                    return HZVideoAdIOS.IsAvailable(tag);
                #endif
            #else
                return false;
            #endif
        }

        /// <summary>
        /// Sets the AdDisplayListener for video ads, which will receive callbacks regarding the state of video ads.
        /// </summary>
        public static void SetDisplayListener(AdDisplayListener listener) {
            HZVideoAd.adDisplayListener = listener;
        }

        #region Internal methods
        public static void InitReceiver(){
            if (_instance == null) {
                GameObject receiverObject = new GameObject("HZVideoAd");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<HZVideoAd>();
            }
        }

        public void SetCallback(string message) {
            string[] displayStateParams = message.Split(',');
            HZVideoAd.SetCallbackStateAndTag(displayStateParams[0], displayStateParams[1]); 
        }

        protected static void SetCallbackStateAndTag(string state, string tag) {
            if (HZVideoAd.adDisplayListener != null) {
                HZVideoAd.adDisplayListener(state, tag);
            }
        }

        protected static IEnumerator InvokeCallbackNextFrame(string state, string tag) {
            yield return null; // wait a frame
            HZVideoAd.SetCallbackStateAndTag(state, tag);
        }
        #endregion
    }

    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class HZVideoAdIOS : MonoBehaviour {
        [DllImport ("__Internal")]
        private static extern void hz_ads_show_video(string tag);
        [DllImport ("__Internal")]
        private static extern void hz_ads_fetch_video(string tag);
        [DllImport ("__Internal")]
        private static extern bool hz_ads_video_is_available(string tag);


        public static void ShowWithOptions(HZShowOptions showOptions) {
            hz_ads_show_video(showOptions.Tag);
        }

        public static void Fetch(string tag) {
            hz_ads_fetch_video(tag);
        }

        public static bool IsAvailable(string tag) {
            return hz_ads_video_is_available(tag);
        }
    }
    #endif

    #if UNITY_ANDROID && !UNITY_EDITOR
    public class HZVideoAdAndroid : MonoBehaviour {
      
        public static void ShowWithOptions(HZShowOptions showOptions) {
        if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("showVideo", showOptions.Tag);
            }
        }

        public static void Fetch(string tag) {
            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("fetchVideo", tag); 
            }
        }
          
        public static Boolean IsAvailable(string tag) {
            if(Application.platform != RuntimePlatform.Android) return false;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                return jc.CallStatic<Boolean>("isVideoAvailable", tag);
            }
        }
    }
    #endif
    #endregion
}
