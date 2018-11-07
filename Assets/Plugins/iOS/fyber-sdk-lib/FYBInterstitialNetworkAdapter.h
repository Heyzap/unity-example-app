//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <UIKit/UIKit.h>

@class FYBBaseNetwork;
@class FYBInterstitialOffer;

@protocol FYBInterstitialNetworkAdapterDelegate;

/**
 * Defines the interface required by an interstitial network SDK wrapper
 */
@protocol FYBInterstitialNetworkAdapter<NSObject>

@property (nonatomic, strong) NSDictionary *offerData;

/**
 *  Name of the network which the interstitial adapter belongs to
 *
 *  @return the name of the network (e.g. AdMob)
 */
- (NSString *)networkName;


/**
 *  Sets the network which the interstitial adapter belongs to
 *
 *  @param network the network which the adapter belongs to
 */
- (void)setNetwork:(FYBBaseNetwork *)network;

/**
 *  Sets the delegate to be notified of interstitial availability and lifecycle
 *
 *  @param delegate An object conform to the FYBInterstitialNetworkAdapterDelegate protocol
 *
 *  @see FYBInterstitialNetworkAdapterDelegate
 */
- (void)setDelegate:(id<FYBInterstitialNetworkAdapterDelegate>)delegate;


#pragma mark - Start

/**
 *  Starts the interstitial adapter with its corresponding credentials and starts the interstitial caching process if available
 *
 *  @param dict Dictionary of settings and credentials
 *
 *  @return YES if the adapter started, otherwise NO
 */
- (BOOL)startAdapterWithDict:(NSDictionary *)dict;


#pragma mark - Request

/**
 *  Requests an interstitial
 *
 *  @param offer Description of the offer to request (e.g. placement_id, creative_type)
 */
- (void)requestInterstitial:(FYBInterstitialOffer *)offer;


#pragma mark - Show

/**
 *  Presents an interstitial
 *
 *  @param offer Description of the offer to present (e.g. placement_id, creative_type)
 */
- (void)presentInterstitial:(FYBInterstitialOffer *)offer viewController:(UIViewController *)viewController;

@end
