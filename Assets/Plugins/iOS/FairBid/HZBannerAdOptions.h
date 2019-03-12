//
//  HZBannerAdOptions.h
//  Heyzap
//
//  Created by Maximilian Tagher on 3/11/15.
//  Copyright (c) 2015 Heyzap. All rights reserved.
//

#import <UIKit/UIKit.h>

/**
 * The size to use for Facebook banners
 */
typedef NS_ENUM(NSUInteger, HZFacebookBannerSize) {
    /**
     *  A fixed size 320x50 pt banner. Corresponds to kFBAdSize320x50.
     */
    HZFacebookBannerSize320x50 __attribute__((deprecated("Facebook has deprecated the 320x50 size."))),
    /**
     *  A banner 50 pts in height whose width expands to fill its containing view. Corresponds to kFBAdSizeHeight50Banner.
     *  **Default value** for Facebook banners.
     */
    HZFacebookBannerSizeFlexibleWidthHeight50,
    /**
     *  A banner 90 pts in height whose width expands to fill its containing view. Corresponds to kFBAdSizeHeight90Banner.
     */
    HZFacebookBannerSizeFlexibleWidthHeight90,
};

/**
 *  The size to use for AdMob banners. NB: Some of AdMob's banner heights vary by iPad/iPhone.
 */
typedef NS_ENUM(NSUInteger, HZAdMobBannerSize){
    /**
     *  An ad size that spans the full width of the application in portrait orientation. The height is
     *  typically 50 pixels on an iPhone/iPod UI, and 90 pixels tall on an iPad UI. Corresponds to kGADAdSizeSmartBannerPortrait.
     *
     *  This is the **default size**
     */
    HZAdMobBannerSizeFlexibleWidthPortrait,
    /**
     *  An ad size that spans the full width of the application in landscape orientation. The height is
     *  typically 32 pixels on an iPhone/iPod UI, and 90 pixels tall on an iPad UI. Corresponds to kGADAdSizeSmartBannerLandscape.
     */
    HZAdMobBannerSizeFlexibleWidthLandscape,
    /**
     *  iPhone and iPod Touch sized banner. Typically 320x50. Corresponds to kGADAdSizeBanner.
     */
    HZAdMobBannerSizeBanner,
    /**
     *  Taller version of HZAdMobBannerSizeBanner. Typically 320x100. Corresponds to kGADAdSizeLargeBanner.
     */
    HZAdMobBannerSizeLargeBanner,
    /**
     *  Leaderboard size for the iPad. Typically 728x90. Corresponds to kGADAdSizeLeaderboard.
     */
    HZAdMobBannerSizeLeaderboard,
    /**
     *  Full Banner size for the iPad (especially in a UIPopoverController or in
     *  UIModalPresentationFormSheet). Typically 468x60. Corresponds to kGADAdSizeFullBanner.
     */
    HZAdMobBannerSizeFullBanner,
};

/** The constant for a banner 320 points wide and 50 points high. */
extern const CGSize HZInMobiBannerSize320x50;
/** The constant for a banner 468 points wide and 60 points high. */
extern const CGSize HZInMobiBannerSize468x60;
/** The constant for a banner 480 points wide and 75 points high. */
extern const CGSize HZInMobiBannerSize480x75;
/** The constant for a banner 728 points wide and 90 points high. */
extern const CGSize HZInMobiBannerSize728x90;

/** The constant for a banner 50 pts in height whose width expands to fill its presentingViewController (default). */
extern const CGSize HZHeyzapExchangeBannerSizeFlexibleWidthHeight50;
/** The constant for a banner 32 pts in height whose width expands to fill its presentingViewController. */
extern const CGSize HZHeyzapExchangeBannerSizeFlexibleWidthHeight32;
/** The constant for a banner 90 pts in height whose width expands to fill its presentingViewController. */
extern const CGSize HZHeyzapExchangeBannerSizeFlexibleWidthHeight90;
/** The constant for a banner 100 pts in height whose width expands to fill its presentingViewController. */
extern const CGSize HZHeyzapExchangeBannerSizeFlexibleWidthHeight100;
/** The constant for a banner 320 points wide and 50 points high. */
extern const CGSize HZHeyzapExchangeBannerSize320x50;
/** The constant for a banner 468 points wide and 60 points high. */
extern const CGSize HZHeyzapExchangeBannerSize468x60;
/** The constant for a banner 480 points wide and 75 points high. */
extern const CGSize HZHeyzapExchangeBannerSize480x75;
/** The constant for a banner 728 points wide and 90 points high. */
extern const CGSize HZHeyzapExchangeBannerSize728x90;

/** The 320x50 size. This is the default for phones. */
extern CGSize const HZMoPubBannerSize;
/** The 728x90 size. This is the default for tablets. */
extern CGSize const HZMoPubLeaderboardSize;

typedef NS_ENUM(NSUInteger, HZAppLovinBannerSize) {
    /** The 320x50 size. This is the default for phones. */
    HZAppLovinBannerSizeBanner,
    /** The 728x90 size. This is the default for tablets. */
    HZAppLovinBannerSizeLeaderboard,
};

@interface HZBannerAdOptions : NSObject <NSCopying>

/**
 *  The size to use for Facebook Audience Network banners. Defaults to HZFacebookBannerSizeFlexibleWidthHeight50.
 */
@property (nonatomic) HZFacebookBannerSize facebookBannerSize;

/**
 *  The size to use for Admob banners.
 */
@property (nonatomic) HZAdMobBannerSize admobBannerSize;

/**
 *  The size to use for Heyzap Exchange banners.
 *  Currently, any size is allowed to be requested, but depending on the exchange providers' inventory, an ad may be returned that is scaled or letterboxed to meet your request. Some constants (named `HZHeyzapExchangeBannerSize...`) are provided above for convenience. 
 *  If you would like to provide your own `CGSize`, you may. Using `-1` for the width will indicate to the SDK that the banner should fill the width of the `presentingViewController`.
 */
@property (nonatomic) CGSize heyzapExchangeBannerSize;

/**
 *  InMobi allows setting an arbitrary size to use for banners, but only some intrinsic banner sizes are supported. 
 *  It's recommended that you choose one of the `HZInMobiBannerSize` constants above.
 *  If you choose an unsupported size, InMobi may scale a similar sized ad to the size you requested.
 *
 *  If you don't select a size, a default size is used based on the current orientation and interface idiom:
 *  Portrait iPhones: `HZInMobiBannerSize320x50`
 *  Landscape iPhones: `HZInMobiBannerSize468x60`
 *  iPads: `HZInMobiBannerSize728x90`
 */
@property (nonatomic) CGSize inMobiBannerSize;

@property (nonatomic) CGSize moPubBannerSize;

/**
 *  If you don't select a size, a default size is used based on the current interface idiom:
 *  iPhones: `HZAppLovinBannerSizeBanner`
 *  iPads: `HZAppLovinBannerSizeLeaderboard`
 */
@property (nonatomic) HZAppLovinBannerSize appLovinBannerSize;

/// @name Other Banner Options
#pragma mark - Other Banner Options

/**
 *  The view controller to present the ad from. 
 *
 *  This property is optional. If not set, it defaults to the root view controller of the application.
 *
 *  @note Setting this property doesn't change where the actual banner (a `UIView`) is placed. For Heyzap Exchange banners, the width of this property will determine the width of a flexible-width banner.
 */
@property (nonatomic, weak) UIViewController *presentingViewController;

/**
 *  An identifier for the location of the ad, which you can use to disable the ad from your dashboard. If not specified the tag "default" is always used.
 */
@property (nonatomic, strong) NSString *tag;

/**
 *  Banner ads have some internal retry logic, to prevent show failures caused by bad network connectivity. Set this time interval to restrict how many seconds Heyzap spends trying to fetch banner ads. This value will only be considered after an initial failure; for instance, a fetchTimeout of 0 will not stop the initial fetch, but it will stop all retries.
 *
 *  @b Default: @c DBL_MAX (no timeout).
 *
 *  @b Minimum: 0
 *
 *  @note Setting this value too low will prevent Heyzap from retrying at all. Give time for slow network requests in this value if you wish to allow Heyzap to retry at least a couple of times before failing (i.e.: at least 60 seconds).
 */
@property (nonatomic) NSTimeInterval fetchTimeout;

@end
