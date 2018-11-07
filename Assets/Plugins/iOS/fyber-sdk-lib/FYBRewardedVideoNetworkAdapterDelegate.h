//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>
#import "FYBRewardedVideoNetworkAdapter.h"

/**
 * Defines the interface of a delegate object through which a class implementing
 * the FYBVideoNetworkAdapter protocol will communicate with the Fyber SDK
 */
@protocol FYBRewardedVideoNetworkAdapterDelegate <NSObject>

@required

/** Request Cycle */

/**
 * Tells the delegate about the availability of videos from the wrapped video network,
 * as a response to the [FYBRewardedVideoNetworkAdapter checkAvailability] invocation
 */
- (void)adapterDidReceiveVideo:(id<FYBRewardedVideoNetworkAdapter>)adapter;

/**
 * Tells the delegate that an error occurred while the wrapped SDK was checking for
 * available videos or attempting to play a video
 */
- (void)adapter:(id <FYBRewardedVideoNetworkAdapter>)adapter didFailToReceiveVideoWithError:(NSError *)error;



/** Play Cycle */

/**
 * Tells the delegate that the wrapped video network SDK started playing a video
 */
- (void)adapterVideoDidStart:(id<FYBRewardedVideoNetworkAdapter>)adapter;

/**
 * Tells the delegate that the wrapped video network SDK aborted the playing of
 * a video due to user action
 */
- (void)adapterVideoDidAbort:(id<FYBRewardedVideoNetworkAdapter>)adapter;

/**
 * Tells the delegate that the wrapped video network SDK played a video until
 * its completion
 */
- (void)adapterVideoDidFinish:(id<FYBRewardedVideoNetworkAdapter>)adapter;

/**
 * Tells the delegate that the wrapped video network SDK closed the video player /
 * post video screen and relinquished control of the user flow
 */
- (void)adapterVideoDidDismiss:(id<FYBRewardedVideoNetworkAdapter>)adapter;

/**
 * Tells the delegate that an error occurred while the wrapped SDK was checking for
 * available videos or attempting to play a video
 */
- (void)adapter:(id <FYBRewardedVideoNetworkAdapter>)adapter didFailToShowVideoWithError:(NSError *)error;

/**
 * Tells the delegate that the wrapped SDK didn't respond timely to the command
 * to start playing a video, and will not play. Typically, it's not necessary to
 * invoke this delegate method unless the wrapped SDK doesn't always start playing
 * immediately when requested
 */
- (void)adapterDidTimeout:(id<FYBRewardedVideoNetworkAdapter>)adapter;

@end
