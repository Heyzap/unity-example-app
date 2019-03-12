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
    /// Use this class to show banner ads.
    /// </summary>
    public class Banner : MonoBehaviour {
        public delegate void AdDisplayListener(string state, string placement);

        private static AdDisplayListener adDisplayListener;
        private static Banner _instance;

        // these are reproduced here for convenience since they were here in old SDK versions
        /// <summary>
        /// Set `BannerShowOptions.Position` to this value to show ads at the top of the screen.
        /// </summary>
        [Obsolete("This constant has been relocated to BannerShowOptions")]
        public const string POSITION_TOP = BannerShowOptions.POSITION_TOP;
        /// <summary>
        /// Set `BannerShowOptions.Position` to this value to show ads at the bottom of the screen.
        /// </summary>
        [Obsolete("This constant has been relocated to BannerShowOptions")]
        public const string POSITION_BOTTOM = BannerShowOptions.POSITION_BOTTOM;

        /// <summary>
        /// Shows a banner ad with the given options.
        /// </summary>
        /// <param name="showOptions">The options with which to show the banner ad, or the defaults if <c>null</c> </param>
        public static void ShowWithOptions(BannerShowOptions showOptions) {
            if (showOptions == null) {
                showOptions = new BannerShowOptions();
            }

            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    BannerAndroid.ShowWithOptions(showOptions);
                #elif UNITY_IPHONE
                    BannerIOS.ShowWithOptions(showOptions);
                #endif
            #else
                Debug.LogWarning("Call received to show an Banner, but the SDK does not function in the editor. You must use a device/emulator to request/show ads.");
                _instance.StartCoroutine(InvokeCallbackNextFrame(FairBidSDK.NetworkCallback.SHOW_FAILED, showOptions.Placement));
            #endif
        }

        /// <summary>
        /// Hides the current banner ad, if there is one, from the view. The next call to ShowWithOptions will unhide the banner ad hidden by this method.
        /// </summary>
        public static void Hide() {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    BannerAndroid.Hide();
                #elif UNITY_IPHONE
                    BannerIOS.Hide();
                #endif
            #else
            #endif
        }

        /// <summary>
        /// Destroys the current banner ad, if there is one. The next call to ShowWithOptions() will create a new banner ad.
        /// </summary>
        public static void Destroy() {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    BannerAndroid.Destroy();
                #elif UNITY_IPHONE
                    BannerIOS.Destroy();
                #endif
            #else
            #endif
        }

        /// <summary>
        /// Gets the current banner ad's dimensions.
        /// </summary>
        /// <returns><c>true</c>, if the dimensions were successfully retrieved, <c>false</c> otherwise.</returns>
        /// <param name="banner">An out param where the dimensions of the current banner ad will be stored, if they are retrieved successfully.</param>
        public static bool GetCurrentBannerDimensions(out Rect banner) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    return BannerAndroid.GetCurrentBannerDimensions(out banner);
                #elif UNITY_IPHONE
                    return BannerIOS.GetCurrentBannerDimensions(out banner);
                #endif
            #else
                banner = new Rect(0, 0, 0, 0);
                return false;
            #endif
        }

        /// <summary>
        /// Sets the AdDisplayListener for banner ads, which will receive callbacks regarding the state of banner ads.
        /// </summary>
        public static void SetDisplayListener(AdDisplayListener listener) {
            adDisplayListener = listener;
        }

        #region Internal methods
        public static void InitReceiver() {
            if (_instance == null) {
                GameObject receiverObject = new GameObject("Banner");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<Banner>();
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
    public class BannerIOS : MonoBehaviour {
        [DllImport ("__Internal")]
        private static extern void fyb_sdk_show_banner(string position, string placement);
        [DllImport ("__Internal")]
        private static extern bool fyb_sdk_hide_banner();
        [DllImport ("__Internal")]
        private static extern bool fyb_sdk_destroy_banner();
        [DllImport ("__Internal")]
        private static extern string fyb_sdk_banner_dimensions();


        public static void ShowWithOptions(BannerShowOptions showOptions) {
            fyb_sdk_show_banner(showOptions.Position, showOptions.Placement);
        }

        public static bool Hide() {
            return fyb_sdk_hide_banner();
        }

        public static void Destroy() {
            fyb_sdk_destroy_banner();
        }

        public static bool GetCurrentBannerDimensions(out Rect banner) {
            banner = new Rect(0,0,0,0); // default value in error cases

            string returnValue = fyb_sdk_banner_dimensions();
            if (returnValue == null || returnValue.Length == 0) {
                return false;
            }

            string[] split = returnValue.Split(' ');
            if (split.Length != 4) {
                return false;
            }

            banner = new Rect(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
            return true;
        }
    }
    #endif

    #if UNITY_ANDROID && !UNITY_EDITOR
    public class BannerAndroid : MonoBehaviour {

        public static void ShowWithOptions(BannerShowOptions showOptions) {
            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("showBanner", showOptions.Placement, showOptions.Position);
            }
        }

        public static void Hide() {
            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("hideBanner"); 
            }
        }

        public static void Destroy() {
            if (Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("destroyBanner"); 
            }
        }

        public static bool GetCurrentBannerDimensions(out Rect banner) {
            banner = new Rect(0, 0, 0, 0); // default value in error cases

            if (Application.platform != RuntimePlatform.Android) {
                return false;
            }

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.fyber.fairbid.sdk.extensions.unity3d.UnityHelper")) { 
                string returnValue = jc.CallStatic<string>("getBannerDimensions");
                if (returnValue == null || returnValue.Length == 0) {
                    return false;
                }

                string[] split = returnValue.Split(' ');
                if (split.Length != 4) {
                    return false;
                }

                banner = new Rect(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]), int.Parse(split[3]));
                return true;
            }
        }
    }
    #endif
    #endregion
}
