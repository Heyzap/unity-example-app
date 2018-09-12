//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

/**
 *  The creative type of an ad
 */
typedef NS_ENUM(NSInteger, FYBInterstitialCreativeType) {
    
    /**
     *  The creative type can be both, video or static
     */
    FYBInterstitialCreativeTypeMixed,

    /**
     *  The creative type is static only
     */
    FYBInterstitialCreativeTypeStatic,

    /**
     *  The creative type is video only
     */
    FYBInterstitialCreativeTypeVideo,

    /**
     *  The creative type is invalid
     */
    FYBInterstitialCreativeTypeInvalid = -1

};
