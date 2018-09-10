//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

#if __has_feature(objc_modules)
@import AdSupport;
@import CoreGraphics;
@import CoreLocation;
@import CoreTelephony;
@import MediaPlayer;
@import StoreKit;
@import SystemConfiguration;
@import QuartzCore;
@import WebKit;
#endif

#import "FYBBannerController.h"
#import "FYBInterstitialController.h"
#import "FYBOfferWallViewController.h"
#import "FYBRewardedVideoController.h"
#import "FYBVirtualCurrencyClient.h"
#import "FYBLogLevel.h"
#import "FYBUser.h"
#import "FYBSDKOptions.h"
#import "FYBCacheManager.h"

/**
 *  Notification sent when the SDK sent the start message to the adapters
 */
FOUNDATION_EXPORT NSString *const FYBAdaptersStartedNotification;

/**
 *  Provides convenience class methods to access the products of the Fyber SDK
 */
@interface FyberSDK : NSObject


/** ---------------- */
/** @name Properties */
/** ---------------- */

/**
 *  Provide access to the user
 *
 *  @see FYBUser
 */
@property (nonatomic, strong, readonly) FYBUser *user;

/**
 *  The identifier of a particular User in the app.
 *  @discussion This identifier is used by the Virtual Currency Client in order to reward an User.
 *
 *  This property is set when the SDK is started, with the userId passed to startWithOptions: within the FYBSDKOptions object.
 *
 *  The userId can be only changed after starting the SDK. Any non-empty string is a valid identifier. Calling setUserId: with a nil parameter or before calling startWithOptions: will not produce any side effect.
 *
 *  If the userId is changed after an offer is requested but before it's completed, upon completion of that offer the rewarded User will be the one active by the time of the request.
 *  @see FYBSDKOptions
 */
@property (nonatomic, copy) NSString *userId;

/**
 *  Determines if a notification should be shown to the user when rewarded
 */
@property (nonatomic, assign) BOOL shouldShowToastOnReward;


/** ---------------------- */
/** @name Starting the SDK */
/** ---------------------- */

/**
 *  Starts the Fyber SDK
 *
 *  @param options A FYBSDKOptions object containing information like appId, userId, securityToken...
 *
 *  @discussion You need to call this method before being able to use the Offer Wall, Interstitials, Rewarded Videos or the Virtual Currency Client
 *
 *  @see FYBSDKOptions
 */
+ (void)startWithOptions:(FYBSDKOptions *)options;


/** ---------------- */
/** @name Offer Wall */
/** ---------------- */

/**
 *  Convenient way of creating a new instance of FYBOfferWallViewController
 *
 *  @return An instance of FYBOfferWallViewController
 */
+ (FYBOfferWallViewController *)offerWallViewController;


/** --------------------- */
/** @name Rewarded Videos */
/** --------------------- */

/**
 *  Accessor to retrieve the controller for Rewarded Videos
 *
 *  @return The FYBRewardedVideoController singleton
 */
+ (FYBRewardedVideoController *)rewardedVideoController;


/** ------------------- */
/** @name Interstitials */
/** ------------------- */

/**
 *  Accessor to retrieve the controller for Interstitials
 *
 *  @return The FYBInterstitialController singleton
 */
+ (FYBInterstitialController *)interstitialController;


/** ------------- */
/** @name Banners */
/** ------------- */

/**
 *  Accessor to retrieve the controller for Banners
 *
 *  @return The FYBBannerController singleton
 */
+ (FYBBannerController *)bannerController;


/** ---------------------- */
/** @name Virtual Currency */
/** ---------------------- */

/**
 *  Retrieves the client for Virtual Currencies
 *
 *  @return The FYBVirtualCurrencyClient singleton
 *
 *  @discussion You have to provide a securityToken to the -[FyberSDK startWithOptions:] method in order for this product to work
 */
+ (FYBVirtualCurrencyClient *)virtualCurrencyClient;


/** ------------------------ */
/** @name Reporting Installs */
/** ------------------------ */

/**
 *  Reports an install to Fyber's server for a given appId
 *
 *  @param appId Your advertiser application ID
 */
+ (void)reportInstall:(NSString *)appId;


/** -------------------------------- */
/** @name Reporting Rewarded Actions */
/** -------------------------------- */

/**
 *  Reports a Rewarded Action as completed to the Fyber servers
 *
 *  @param actionId The ID of the action to report as completed
 *  @param appId    Your advertiser application ID for which you want to report the action
 */
+ (void)reportRewardedAction:(NSString *)actionId appId:(NSString *)appId;


/** ------------------- */
/** @name Miscellaneous */
/** ------------------- */

/**
 *  The FyberSDK shared instance
 *
 *  @return The FyberSDK singleton
 */
+ (FyberSDK *)instance;

/**
 *  Accessors for the FYBCacheManager shared instance,
 *
 *  @return The FYBCacheManager singleton
 *
 *  @see FYBCacheManager
 */
+ (FYBCacheManager *)cacheManager;

/**
 *  Returns the version of the SDK.
 *
 *  @return A string containing the version of the SDK (e.g. 8.0.0)
 */
+ (NSString *)versionString;

/**
 *  Sets the log level
 *
 *  @param level The level of logging you want to see. By default the log level is set to FYBLogLevelInfo
 *
 *  @see FYBLogLevel
 */
+ (void)setLoggingLevel:(FYBLogLevel)level;

/**
 *  Please use [FyberSDK instance] instead
 */
- (instancetype)init __attribute__((unavailable("not available, use [FyberSDK instance] instead")));

@end
