//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

/**
 *  The state of the Banner controller
 */
typedef NS_ENUM(NSInteger, FYBBannerControllerState) {
    /**
     *  The controller is ready to query banner offers
     */
    FYBBannerControllerStateReadyToQuery,

    /**
     *  The controller is querying banner offers
     */
    FYBBannerControllerStateQuerying,

    /**
     *  The controller received a banner offer and is ready to show it
     */
    FYBBannerControllerStateReadyToShow,

    /**
     *  The controller is showing the banner
     */
    FYBBannerControllerStateShowing
};
