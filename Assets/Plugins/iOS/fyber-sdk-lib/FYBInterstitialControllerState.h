//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

/**
 *  The state of the Interstitial controller
 */
typedef NS_ENUM(NSInteger, FYBInterstitialControllerState) {
    /**
     *  The controller is ready to query interstitial offers
     */
    FYBInterstitialControllerStateReadyToQuery,

    /**
     *  The controller is querying interstitial offers
     */
    FYBInterstitialControllerStateQuerying,

    /**
     *  The controller is validating the offers
     */
    FYBInterstitialControllerStateValidatingOffers,

    /**
     *  The controller received an interstitial offer and is ready to show it
     */
    FYBInterstitialControllerStateReadyToShow,

    /**
     *  The controller is showing the interstitial
     */
    FYBInterstitialControllerStateShowing
};
