//
//  HZDemographics.cs
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
    /// Use this class to pass information about a user to mediated ad networks that want the data. This kind of data is optional but can improve ad revenues.
    /// </summary>
    public class HZDemographics : MonoBehaviour {

        private static HZDemographics _instance = null;

        public enum Gender {
            UNKNOWN,
            MALE,
            FEMALE,
            OTHER
        }
        /// <summary>
        /// Set the gender of the user, if known, using the provided enum.
        /// </summary>
        public static void SetUserGender(Gender gender) {
            if (System.Enum.IsDefined(typeof(Gender), gender)) {
                #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                    #if UNITY_ANDROID
                        HeyzapDemographicsAndroid.SetUserGender(gender.ToString());
                    #elif UNITY_IPHONE
                        HeyzapDemographicsIOS.SetUserGender(gender.ToString());
                    #endif
                #else
                #endif
            }
        }

        /// <summary>
        /// Set the location of the user, if known. The required parameters match the parameters provided by the UnityEngine `LocationInfo` struct - see `https://docs.unity3d.com/ScriptReference/LocationInfo.html` for more information.
        /// </summary>
        public static void SetUserLocation(float latitude, float longitude, float horizontalAccuracy, float verticalAccuracy, float altitude, double timestamp) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HeyzapDemographicsAndroid.SetUserLocation(latitude, longitude, horizontalAccuracy, verticalAccuracy, altitude, timestamp);
                #elif UNITY_IPHONE
                    HeyzapDemographicsIOS.SetUserLocation(latitude, longitude, horizontalAccuracy, verticalAccuracy, altitude, timestamp);
                #endif
            #else
            #endif
        }

        /// <summary>
        /// Set the postal code (i.e.: the ZIP code in the US) of the user, if known. This is an alternative to setting the exact location but can provide similar benefits to ad revenues / targeting.
        /// </summary>
        public static void SetUserPostalCode(string postalCode) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HeyzapDemographicsAndroid.SetUserPostalCode(postalCode);
                #elif UNITY_IPHONE
                    HeyzapDemographicsIOS.SetUserPostalCode(postalCode);
                #endif
            #else
            #endif
        }

        /// <summary>
        /// Set the household income of the user, if known.
        /// </summary>
        public static void SetUserHouseholdIncome(int householdIncome) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HeyzapDemographicsAndroid.SetUserHouseholdIncome(householdIncome);
                #elif UNITY_IPHONE
                    HeyzapDemographicsIOS.SetUserHouseholdIncome(householdIncome);
                #endif
            #else
            #endif
        }

        public enum MaritalStatus {
            UNKNOWN,
            SINGLE,
            MARRIED
        }
        /// <summary>
        /// Set the marital status of the user, if known, using the provided enum.
        /// </summary>
        public static void SetUserMaritalStatus(MaritalStatus maritalStatus) {
            if (System.Enum.IsDefined(typeof(MaritalStatus), maritalStatus)) {
                #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                    #if UNITY_ANDROID
                        HeyzapDemographicsAndroid.SetUserMaritalStatus(maritalStatus.ToString());
                    #elif UNITY_IPHONE
                        HeyzapDemographicsIOS.SetUserMaritalStatus(maritalStatus.ToString());
                    #endif
                #else
                #endif
            }
        }

        public enum EducationLevel {
            UNKNOWN,
            GRADE_SCHOOL,
            HIGH_SCHOOL_UNFINISHED,
            HIGH_SCHOOL_FINISEHD,
            COLLEGE_UNFINISHED,
            ASSOCIATE_DEGREE,
            BACHELORS_DEGREE,
            GRADUATE_DEGREE,
            POSTGRADUATE_DEGREE
        }
        /// <summary>
        /// Set the highest education level already achieved by the user, if known, using the provided enum.
        /// </summary>
        public static void SetUserEducationLevel(EducationLevel educationLevel) {
            if (System.Enum.IsDefined(typeof(EducationLevel), educationLevel)) {
                #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                    #if UNITY_ANDROID
                        HeyzapDemographicsAndroid.SetUserEducationLevel(educationLevel.ToString());
                    #elif UNITY_IPHONE
                        HeyzapDemographicsIOS.SetUserEducationLevel(educationLevel.ToString());
                    #endif
                #else
                #endif
            }
        }

        /// <summary>
        /// Set the birth date of the user, if known, using this format: `YYYY/MM/DD`. Example: `2000/12/31` for December 31st, 2000.
        /// </summary>
        public static void SetUserBirthDate(string yyyyMMdd_date) {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
                #if UNITY_ANDROID
                    HeyzapDemographicsAndroid.SetUserBirthDate(yyyyMMdd_date);
                #elif UNITY_IPHONE
                    HeyzapDemographicsIOS.SetUserBirthDate(yyyyMMdd_date);
                #endif
            #else
            #endif
        }


        #region Internal methods

        public static void InitReceiver(){
            if (_instance == null) {
                GameObject receiverObject = new GameObject("HZDemographics");
                DontDestroyOnLoad(receiverObject);
                _instance = receiverObject.AddComponent<HZDemographics>();
            }
        }

        #endregion

    }

    #region Platform-specific translations
    #if UNITY_IPHONE && !UNITY_EDITOR
    public class HeyzapDemographicsIOS : MonoBehaviour {

        [DllImport ("__Internal")]
        private static extern void hz_demo_set_gender(string gender);
        public static void SetUserGender(string gender) {
            hz_demo_set_gender(gender);
        }

        [DllImport ("__Internal")]
        private static extern void hz_demo_set_location(float latitude, float longitude, float horizontalAccuracy, float verticalAccuracy, float altitude, double timestamp);
        public static void SetUserLocation(float latitude, float longitude, float horizontalAccuracy, float verticalAccuracy, float altitude, double timestamp) {
            hz_demo_set_location(latitude, longitude, horizontalAccuracy, verticalAccuracy, altitude, timestamp);
        }

        [DllImport ("__Internal")]
        private static extern void hz_demo_set_postal_code(string postalCode);
        public static void SetUserPostalCode(string postalCode) {
            hz_demo_set_postal_code(postalCode);
        }

        [DllImport ("__Internal")]
        private static extern void hz_demo_set_household_income(int householdIncome);
        public static void SetUserHouseholdIncome(int householdIncome) {
            hz_demo_set_household_income(householdIncome);
        }

        [DllImport ("__Internal")]
        private static extern void hz_demo_set_marital_status(string maritalStatus);
        public static void SetUserMaritalStatus(string maritalStatus) {
            hz_demo_set_marital_status(maritalStatus);
        }

        [DllImport ("__Internal")]
        private static extern void hz_demo_set_education_level(string educationLevel);
        public static void SetUserEducationLevel(string educationLevel) {
            hz_demo_set_education_level(educationLevel);
        }

        [DllImport ("__Internal")]
        private static extern void hz_demo_set_birth_date(string yyyyMMdd_date);
        public static void SetUserBirthDate(string yyyyMMdd_date) {
            hz_demo_set_birth_date(yyyyMMdd_date);
        }

    }
    #endif

    #if UNITY_ANDROID && !UNITY_EDITOR
    public class HeyzapDemographicsAndroid : MonoBehaviour {

        public static void SetUserGender(string gender) {
            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("setUserGender", gender);
            }
        }

        public static void SetUserLocation(float latitude, float longitude, float horizontalAccuracy, float verticalAccuracy, float altitude, double timestamp) {
            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("setUserLocation", latitude, longitude, horizontalAccuracy, verticalAccuracy, altitude, timestamp);
            }

        }

        public static void SetUserPostalCode(string postalCode) {

            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("setUserPostalCode", postalCode);
            }
        }

        public static void SetUserHouseholdIncome(int householdIncome) {

            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("setUserHouseholdIncome", householdIncome);
            }
        }

        public static void SetUserMaritalStatus(string maritalStatus) {

            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("setUserMaritalStatus", maritalStatus);
            }
        }

        public static void SetUserEducationLevel(string educationLevel) {

            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("setUserEducationLevel", educationLevel);
            }
        }

        public static void SetUserBirthDate(string yyyyMMdd_date) {

            if(Application.platform != RuntimePlatform.Android) return;

            AndroidJNIHelper.debug = false; 
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) { 
                jc.CallStatic("setUserBirthDate", yyyyMMdd_date);
            }
        }

    }
    #endif
    #endregion
}
