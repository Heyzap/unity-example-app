//
//  Copyright © 2016 Fyber. All rights reserved.
//

#import "FYBBannerSize.h"


FOUNDATION_EXPORT NSString *const FYBAdMobNetworkName;


@interface FYBBannerSize (AdMob)

/**
 *  Returns default size used in request when size is not secified - adMobSmartPortrait.
 */
+ (FYBBannerSize *)adMobDefault;

/**
 *  iPhone and iPod Touch sized banner. Typically 320x50. Corresponds to kGADAdSizeBanner.
 */
+ (FYBBannerSize *)adMobBanner;

/**
 *  Taller version of adMobStandard. Typically 320x100. Corresponds to kGADAdSizeLargeBanner.
 */
+ (FYBBannerSize *)adMobLargeBanner;

/**
 *  Medium rectangle size for the iPad (especially in a UISplitView's left pane). Typically 300x250. Corresponds to
 * kGADAdSizeMediumRectangle.
 */
+ (FYBBannerSize *)adMobMediumRectangle;

/**
 *  Full banner size for the iPad (especially in a UIPopoverController or in UIModalPresentationFormSheet). Typically
 * 468x60. Corresponds to kGADAdSizeFullBanner.
 */
+ (FYBBannerSize *)adMobFullBanner;

/**
 *  Leaderboard size for the iPad. Typically 728x90. Corresponds to kGADAdSizeLeaderboard.
 *
 */
+ (FYBBannerSize *)adMobLeaderBoard;

/**
 *  An ad size that spans the full width of the application in portrait orientation. The height is typically 50 pixels
 * on an iPhone/iPod UI, and 90 pixels tall on an iPad UI. Corresponds to kGADAdSizeSmartBannerPortrait.
 */
+ (FYBBannerSize *)adMobSmartPortrait;

/**
 *  An ad size that spans the full width of the application in landscape orientation. The height is typically 32 pixels
 * on an iPhone/iPod UI, and 90 pixels tall on an iPad UI. Corresponds to kGADAdSizeSmartBannerLandscape.
 */
+ (FYBBannerSize *)adMobSmartLandscape;

@end