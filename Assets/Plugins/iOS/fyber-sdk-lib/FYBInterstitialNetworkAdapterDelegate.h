//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import "FYBInterstitialControllerDismissReason.h"

@protocol FYBInterstitialNetworkAdapter;

/**
 * Protocol that delegates wishing to be notified of interstitial availability
 * and lifecycle must conform to
 */
@protocol FYBInterstitialNetworkAdapterDelegate<NSObject>

#pragma mark - Request Interstitial

/**
 *  Informs the delegate that an Interstitial has been received
 *
 *  @param adapter The interstitial adapter that is sending the message
 */
- (void)adapterDidReceiveInterstitial:(id<FYBInterstitialNetworkAdapter>)adapter;

/**
 *  Informs the delegate that an Interstitial failed to be received
 *
 *  @param adapter The interstitial adapter that sent the message
 *  @param error   The error that occurred during the request of the interstitial offer
 */
- (void)adapter:(id<FYBInterstitialNetworkAdapter>)adapter didFailToReceiveInterstitialWithError:(NSError *)error;


#pragma mark - Show Interstitial

/**
 *  Informs the delegate that an Interstitial has been presented
 *
 *  @param adapter The interstitial adapter that sent the message
 */
- (void)adapterDidPresentInterstitial:(id<FYBInterstitialNetworkAdapter>)adapter;

/**
 *  Informs the delegate that an Interstitial has been dismissed
 *
 *  @param adapter The interstitial adapter that sent the message
 *  @param reason  The reason why the interstitial was dismissed
 *  @see FYBInterstitialControllerDismissReason
 */
- (void)adapter:(id<FYBInterstitialNetworkAdapter>)adapter didDismissInterstitialWithReason:(FYBInterstitialControllerDismissReason)reason;

/**
 *  Informs the delegate that an Interstitial failed to be presented
 *
 *  @param adapter The interstitial adapter that sent the message
 *  @param error   The error that occurred while trying to show the interstitial
 */
- (void)adapter:(id<FYBInterstitialNetworkAdapter>)adapter didFailToPresentInterstitialWithError:(NSError *)error;

@end
