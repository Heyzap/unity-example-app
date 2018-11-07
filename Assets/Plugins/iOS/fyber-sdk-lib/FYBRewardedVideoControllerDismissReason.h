//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//


/**
 *  Reason why the Rewarded Video Controller has been dismissed
 */
typedef NS_ENUM(NSInteger, FYBRewardedVideoControllerDismissReason) {
    /**
     *  An error occurred while playing the video
     */
    FYBRewardedVideoControllerDismissReasonError = -1,

    /**
     *  The video played until the end and the player was dismissed
     */
    FYBRewardedVideoControllerDismissReasonUserEngaged,

    /**
     *  The video was aborted by the user
     */
    FYBRewardedVideoControllerDismissReasonAborted
};
