//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

@class FYBBannerController;

/**
 *  The delegate of a FYBBannerController object must adopt the FYBBannerControllerDelegate protocol. Optional methods
 *  of the protocol allow the delegate to be aware of the status of the banner controller
 */
@protocol FYBBannerControllerDelegate<NSObject>

@optional

#pragma mark - Request Banner

/**
 *  The banner controller received a banner offer
 *
 *  @param bannerController The banner controller that received the banner offer
 */
- (void)bannerControllerDidReceiveBanner:(FYBBannerController *)bannerController;

/**
 *  The banner controller failed to receive the banner offer
 *
 *  @param bannerController  The banner controller that failed to receive the banner offer
 *  @param error             The error that occurred during the request of the banner offer
 */
- (void)bannerController:(FYBBannerController *)bannerController didFailToReceiveBannerWithError:(NSError *)error;


#pragma mark - User interaction

/**
 *  The banner was clicked
 *
 *  @param bannerController The banner controller that loaded the banner
 */
- (void)bannerControllerWasClicked:(FYBBannerController *)bannerController;

/**
 *  The banner will present a modal view
 *
 *  @param bannerController The banner controller that loaded the banner
 */
- (void)bannerControllerWillPresentModalView:(FYBBannerController *)bannerController;

/**
 *  The user did dismiss a modal view
 *
 *  @param bannerController The banner controller that loaded the banner
 */
- (void)bannerControllerDidDismissModalView:(FYBBannerController *)bannerController;

/**
 *  The user will leave the application upon interaction with the banner
 *
 *  @param bannerController The banner controller that loaded the banner
 */
- (void)bannerControllerWillLeaveApplication:(FYBBannerController *)bannerController;

@end
