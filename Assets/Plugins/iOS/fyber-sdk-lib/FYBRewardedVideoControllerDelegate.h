//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import "FYBRewardedVideoControllerDismissReason.h"

@class FYBRewardedVideoController;

/**
 *  The delegate of the FYBRewardedVideoController should adopt the FYBRewardedVideoControllerDelegate protocol. Optional methods of the protocol allow the delegate to
 *  be notified of the offers availability and engagement status
 */
@protocol FYBRewardedVideoControllerDelegate<NSObject>

@optional

#pragma mark - Request Rewarded Video

/**
 *  The Rewarded Video controller received a video offer
 *
 *  @param rewardedVideoController The Rewarded Video controller that received the video offer
 *
 *  @discussion Even though optional, we strongly recommend that you implement this delegate method in order to know when a Rewarded Video is ready to be shown
 */
- (void)rewardedVideoControllerDidReceiveVideo:(FYBRewardedVideoController *)rewardedVideoController;

/**
 *  The Rewarded Video controller failed to receive the video offer
 *
 *  @param rewardedVideoController The Rewarded Video controller that failed to receive the video offer
 *  @param error                   The error that occurred during the request of the video offer
 */
- (void)rewardedVideoController:(FYBRewardedVideoController *)rewardedVideoController didFailToReceiveVideoWithError:(NSError *)error;


#pragma mark - Show Rewarded Video

/**
 *  The Rewarded Video controller started playing a video offer
 *
 *  @param rewardedVideoController The Rewarded Video controller that played the video offer
 */
- (void)rewardedVideoControllerDidStartVideo:(FYBRewardedVideoController *)rewardedVideoController;


/**
 *  The Rewarded Video controller dismissed the rewarded video
 *
 *  @param rewardedVideoController The Rewarded Video controller that dismissed the rewarded video
 *  @param reason                  The reason why the video was dismissed
 *
 *  @see FYBRewardedVideoControllerDismissReason
 */
- (void)rewardedVideoController:(FYBRewardedVideoController *)rewardedVideoController didDismissVideoWithReason:(FYBRewardedVideoControllerDismissReason)reason;

/**
 *  The Rewarded Video controller failed to show the video offer
 *
 *  @param rewardedVideoController The Rewarded Video controller that failed to show the video offer
 *  @param error                   The error that occurred while trying to play the video offer
 */
- (void)rewardedVideoController:(FYBRewardedVideoController *)rewardedVideoController didFailToStartVideoWithError:(NSError *)error;


@end
