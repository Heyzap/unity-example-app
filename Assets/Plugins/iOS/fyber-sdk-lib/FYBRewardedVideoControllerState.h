//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

/**
 *  The state of the Rewarded Video controller
 */
typedef NS_ENUM(NSInteger, FYBRewardedVideoControllerState) {
    /**
     *  The controller is ready to query video offers
     */
    FYBRewardedVideoControllerStateReadyToQuery,

    /**
     *  The controller is querying video offers
     */
    FYBRewardedVideoControllerStateQuerying,

    /**
     *  The controller received a video offer and is ready to show it
     */
    FYBRewardedVideoControllerStateReadyToShow,

    /**
     *  The controller is showing the video
     */
    FYBRewardedVideoControllerStateShowing
};
