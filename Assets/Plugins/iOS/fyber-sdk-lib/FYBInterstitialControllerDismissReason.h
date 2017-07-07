//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

/**
 *  Reason why the Interstitial controller has been dismissed
 */
typedef NS_ENUM(NSInteger, FYBInterstitialControllerDismissReason) {
    /**
     *  The Interstitial controller was dismissed for an unknown reason
     */
    FYBInterstitialControllerDismissReasonError = -1,

    /**
     *  The Interstitial controller was closed because the user clicked on the ad
     */
    FYBInterstitialControllerDismissReasonUserEngaged,

    /**
     *  The Interstitial controller was explicitly closed by the user
     */
    FYBInterstitialControllerDismissReasonAborted,

    /**
     *  The Interstitial controller was closed because the video interstitial ended
     */
    FYBInterstitialControllerDismissReasonVideoEnded
};
