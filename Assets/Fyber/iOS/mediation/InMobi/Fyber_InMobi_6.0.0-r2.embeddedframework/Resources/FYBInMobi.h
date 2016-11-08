//
//  Copyright © 2016 Fyber. All rights reserved.
//

#import "FYBBannerSize.h"

FOUNDATION_EXPORT NSString *const FYBInMobiNetworkName;


@interface FYBBannerSize (InMobi)

/**
 *  Returns default size used in request when size is not secified - inMobiBanner320x50.
 */
+ (FYBBannerSize *)inMobiDefault;

+ (FYBBannerSize *)inMobiBanner320x50;

+ (FYBBannerSize *)inMobiBanner320x48;

+ (FYBBannerSize *)inMobiBanner300x250;

+ (FYBBannerSize *)inMobiBanner120x600;

+ (FYBBannerSize *)inMobiBanner468x60;

+ (FYBBannerSize *)inMobiBanner728x90;

+ (FYBBannerSize *)inMobiBanner1024x768;

@end
