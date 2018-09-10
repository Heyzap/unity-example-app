//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <UIKit/UIKit.h>
#import "FYBRequestParameters.h"
#import "FYBBannerControllerState.h"
#import "FYBBannerControllerDelegate.h"
#import "FYBBannerSize.h"
#import "FYBBannerPosition.h"

/**
 *  Provides methods to request and show Banners
 */
@interface FYBBannerController : NSObject

/**
 *  The current state of the Banner controller
 *
 *  @see FYBBannerControllerState
 */
@property (nonatomic, assign, readonly) FYBBannerControllerState state;

/**
 *  The object that acts as the delegate of the Banner controller
 *
 *  @discussion The delegate must adopt the FYBBannerControllerDelegate protocol. The delegate is not retained
 *
 *  @see FYBBannerControllerDelegate
 */
@property (nonatomic, weak) id<FYBBannerControllerDelegate> delegate;

/**
 *  The view controller to present modal content in case a banner has been clicked
 */
@property (nonatomic, weak) UIViewController *modalViewController;

/**
 *  A Banner view
 *
 *  @discussion One of the requestBannerWithSizes: methods needs to be called before presenting a banner
 *              You also need to make sure that a Banner has been received before trying to present the banner. For that you need to implement
 *              the FYBBannerControllerDelegate protocol and set the delegate property of the controller
 */
@property (nonatomic, strong, readonly) UIView *bannerView;

/**
 *  Requests a Banner
 *
 *  @discussion You need to set the delegate property of this controller in order to be aware of the request and to show the Banner
 *
 *  @param sizes A dictionary of banner sizes for the networks
 *
 *  @see FYBBannerControllerDelegate
 */
- (void)requestBannerWithSizes:(NSDictionary<NSString *, FYBBannerSize *> *)sizes;

/**
 *  Same as -requestBannerWithSizes: but accepts a FYBRequestParameters object as parameter. Through this object you can add custom parameters to the request
 *  and also specify a placementId
 *
 *  @param sizes A dictionary of banner sizes for the networks
 *  @param parameters A configured instance of FYBRequestParameters
 */
- (void)requestBannerWithSizes:(NSDictionary<NSString *, FYBBannerSize *> *)sizes parameters:(FYBRequestParameters *)parameters;

/**
 *  Presents a Banner in the application window's rootViewcontroller
 *
 *  @discussion One of the requestBannerWithSizes: methods needs to be called before presenting a banner
 *              You also need to make sure that a Banner has been received before trying to present the banner. For that you need to implement
 *              the FYBBannerControllerDelegate protocol and set the delegate property of the controller
 *
 *  @param position The position where the Banner is presented (either top or bottom)
 */
- (void)presentBannerAtPosition:(FYBBannerPosition)position;

/**
 *  Removes a banner from memory, by removing it from its superview and setting the banner to nil
 */
- (void)destroyBanner;

/**
 *  Please use [FyberSDK bannerController] instead
 */
- (instancetype)init __attribute__((unavailable("not available, use [FyberSDK bannerController] instead")));

@end
