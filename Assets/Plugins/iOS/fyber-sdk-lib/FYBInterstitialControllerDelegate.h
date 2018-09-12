//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import "FYBInterstitialControllerDismissReason.h"

@class FYBInterstitialController;

/**
 *  The delegate of a FYBInterstitialController object must adopt the FYBInterstitialControllerDelegate protocol. Optional methods
 *  of the protocol allow the delegate to be aware of the status of the interstitial controller
 */
@protocol FYBInterstitialControllerDelegate<NSObject>

@optional

#pragma mark - Request Interstitial

/**
 *  The interstitial controller received an interstitial offer
 *
 *  @param interstitialController The interstitial controller that received the interstitial offer
 */
- (void)interstitialControllerDidReceiveInterstitial:(FYBInterstitialController *)interstitialController;

/**
 *  The interstitial controller failed to receive the interstitial offer
 *
 *  @param interstitialController  The interstitial controller that failed to receive the interstitial offer
 *  @param error                   The error that occurred during the request of the interstitial offer
 */
- (void)interstitialController:(FYBInterstitialController *)interstitialController didFailToReceiveInterstitialWithError:(NSError *)error;


#pragma mark - Show Interstitial


/**
 *  The interstitial controller showed an interstitial offer
 *
 *  @param interstitialController  The interstitial controller that showed the interstitial offer
 */
- (void)interstitialControllerDidPresentInterstitial:(FYBInterstitialController *)interstitialController;


/**
 *  The interstitial controller dismissed the interstitial
 *
 *  @param interstitialController  The interstitial controller that dismissed the interstitial
 *  @param reason                  The reason why the interstitial was dismissed.
 *  @see FYBInterstitialControllerDismissReason
 */
- (void)interstitialController:(FYBInterstitialController *)interstitialController didDismissInterstitialWithReason:(FYBInterstitialControllerDismissReason)reason;

/**
 *  The interstitial controller failed to show an interstitial offer
 *
 *  @param interstitialController  The interstitial controller that failed to show the interstitial offer
 *  @param error                   The error that occurred while trying to show the interstitial
 */
- (void)interstitialController:(FYBInterstitialController *)interstitialController didFailToPresentInterstitialWithError:(NSError *)error;

@end
