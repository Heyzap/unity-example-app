//
//
//  Copyright © 2016 Fyber. All rights reserved.
//
//

#import "FYBBannerSize.h"

FOUNDATION_EXPORT NSString *const FYBFacebookNetworkName;


@interface FYBBannerSize (Facebook)

/**
 *  Returns default size used in request when size is not secified - facebookSmartx50.
 */
+ (FYBBannerSize *)facebookDefault;

+ (FYBBannerSize *)facebook320x50;

+ (FYBBannerSize *)facebookSmartx50;

+ (FYBBannerSize *)facebookSmartx90;

+ (FYBBannerSize *)facebookSmartx250;

+ (FYBBannerSize *)facebookInterstitial;

@end