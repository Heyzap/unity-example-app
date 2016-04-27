/*
 * Copyright (c) 2015, Heyzap, Inc.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:
 *
 * * Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * * Redistributions in binary form must reproduce the above copyright
 *   notice, this list of conditions and the following disclaimer in the
 *   documentation and/or other materials provided with the distribution.
 *
 * * Neither the name of 'Heyzap, Inc.' nor the names of its contributors
 *   may be used to endorse or promote products derived from this software
 *   without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
 * TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */


#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "HZLog.h"
#import "HZInterstitialAd.h"
#import "HZIncentivizedAd.h"
#import "HZBannerAdController.h"
#import "HZBannerAdOptions.h"

#ifndef NS_ENUM
#define NS_ENUM(_type, _name) enum _name : _type _name; enum _name : _type
#endif

#define SDK_VERSION @"9.6"

#if __has_feature(objc_modules)
@import AdSupport;
@import CoreGraphics;
@import CoreTelephony;
@import MediaPlayer;
@import QuartzCore;
@import StoreKit;
@import iAd;
@import MobileCoreServices;
@import Security;
@import SystemConfiguration;
@import EventKit;
@import EventKitUI;
@import MessageUI;
@import CoreLocation;
#endif

typedef NS_ENUM(NSUInteger, HZAdOptions) {
    HZAdOptionsNone = 0 << 0, // 0
    /**
     *  Pass this to disable automatic prefetching of ads. Ad prefetching occurs immediately after you initialize the Heyzap SDK and also after ads are dismissed.
     */
    HZAdOptionsDisableAutoPrefetching = 1 << 0, // 1
    /**
     *  Pass this if you are only integrating the Heyzap SDK into your app to track game installs as an advertiser. No ads will be fetched.
     */
    HZAdOptionsInstallTrackingOnly = 1 << 1, // 2
    /**
     *  @deprecated
     *  Please use HZAdOptionsInstallTrackingOnly instead.
     */
    HZAdOptionsAdvertiserOnly DEPRECATED_ATTRIBUTE = HZAdOptionsInstallTrackingOnly,
    // This doesn't do anything for iOS, but is here to keep parity with the Android SDK's flag values for the sake of Unity, AIR, etc.
    HZAdOptionsAmazon DEPRECATED_ATTRIBUTE = 1 << 2, // 4
    /**
     *  Pass this to disable mediation. This is not required, but is recommended for developers not using mediation (i.e: not integrating any 3rd-pary network SDKs). If you're mediating Heyzap through someone (e.g. AdMob), it is *strongly* recommended that you disable Heyzap's mediation to prevent any potential conflicts.
     */
    HZAdOptionsDisableMedation = 1 << 3, // 8
    /**
     *  Pass this to disable recording of In-App Purchase data
     */
    HZAdOptionsDisableAutomaticIAPRecording = 1 << 4, // 16
    
    //placeholder for android flag value NATIVE_ADS_ONLY = 1 << 5 // 32
    // (iOS does not (yet) use this option, but iOS and Android need to keep the same flag values for the sake of Unity, AIR, etc.)
    
    /**
     *  Pass this flag to mark mediated ads as "child-directed". This value will be passed on to networks that support sending such an option (for purposes of the Children's Online Privacy Protection Act (COPPA)).
     *  Currently, only AdMob is passed this information (see https://developers.google.com/admob/ios/targeting#child-directed_setting ). The AdMob setting will be left alone if this flag is not passed when the Heyzap SDK is started.
     */
    HZAdOptionsChildDirectedAds = 1 << 6, // 64
};

// HZAdsDelegate Callback NSNotifications
extern NSString * const HZMediationDidShowAdNotification;
extern NSString * const HZMediationDidFailToShowAdNotification;
extern NSString * const HZMediationDidReceiveAdNotification;
extern NSString * const HZMediationDidFailToReceiveAdNotification;
extern NSString * const HZMediationDidClickAdNotification;
extern NSString * const HZMediationDidHideAdNotification;
extern NSString * const HZMediationWillStartAdAudioNotification;
extern NSString * const HZMediationDidFinishAdAudioNotification;
// HZIncentivizedAdDelegate Callback NSNotifications
extern NSString * const HZMediationDidCompleteIncentivizedAdNotification;
extern NSString * const HZMediationDidFailToCompleteIncentivizedAdNotification;


/** The `HZAdsDelegate` protocol provides global information about our ads. If you want to know if we had an ad to show after calling `showAd` (for example, to fallback to another ads provider). It is recommend using the `showAd:completion:` method instead. */
@protocol HZAdsDelegate<NSObject>

@optional

#pragma mark - Showing ads callbacks

/**
 *  Called when we succesfully show an ad.
 *
 *  @param tag The identifier for the ad.
 */
- (void)didShowAdWithTag: (NSString *) tag;

/**
 *  Called when an ad fails to show
 *
 *  @param tag   The identifier for the ad.
 *  @param error An NSError describing the error
 */
- (void)didFailToShowAdWithTag: (NSString *) tag andError: (NSError *)error;

/**
 *  Called when a valid ad is received
 *
 *  @param tag The identifier for the ad.
 */
- (void)didReceiveAdWithTag: (NSString *) tag;

/**
 *  Called when our server fails to send a valid ad, like when there is a 500 error.
 *
 *  @param tag The identifier for the ad.
 */
- (void)didFailToReceiveAdWithTag: (NSString *) tag;



// Should probably have new API: didFailToReceiveAd (no tag)
// didRecieveAd (no tag)


/**
 *  Called when the user clicks on an ad.
 *
 *  @param tag An identifier for the ad.
 */
- (void)didClickAdWithTag: (NSString *) tag DEPRECATED_MSG_ATTRIBUTE("Click callbacks are not supported by the Fyber SDK");
/**
 *  Called when the ad is dismissed.
 *
 *  @param tag An identifier for the ad.
 */
- (void)didHideAdWithTag: (NSString *) tag;

/**
 *  Called when an ad will use audio
 */
- (void)willStartAudio;

/**
 *  Called when an ad will finish using audio
 */
- (void) didFinishAudio;

@end

/** The HZIncentivizedAdDelegate protocol provides global information about using an incentivized ad. If you want to give the user a reward
 after successfully finishing an incentivized ad, implement the didCompleteAd method */
@protocol HZIncentivizedAdDelegate<HZAdsDelegate>

@optional

/** Called when a user successfully completes viewing an ad */
- (void)didCompleteAdWithTag: (NSString *) tag;
/** Called when a user does not complete the viewing of an ad */
- (void)didFailToCompleteAdWithTag: (NSString *) tag;

@end

/**
 *  A class with miscellaneous Heyzap Ads methods. All methods on this class must be called from the main queue.
 */
@interface HeyzapAds : NSObject

+ (void)startWithAppID:(NSString *)appID securityToken:(NSString *)token options:(HZAdOptions)options;

+ (void) startWithPublisherID: (NSString *) publisherID andOptions: (HZAdOptions) options NS_UNAVAILABLE;
+ (void) startWithPublisherID:(NSString *)publisherID andOptions:(HZAdOptions)options andFramework: (NSString *) framework NS_UNAVAILABLE;
+ (void) startWithPublisherID: (NSString *) publisherID NS_UNAVAILABLE;

@end
