//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#import "FYBInterstitialControllerDelegate.h"
#import "FYBInterstitialControllerState.h"
#import "FYBRequestParameters.h"

/**
 *  Provides methods to request and show Interstitials
 */
@interface FYBInterstitialController : NSObject

/**
 *  The current state of the Interstitial controller
 *
 *  @see FYBInterstitialControllerState
 */
@property (nonatomic, assign, readonly) FYBInterstitialControllerState state;

/**
 *  The object that acts as the delegate of the Interstitial controller
 *
 *  @discussion The delegate must adopt the FYBInterstitialControllerDelegate protocol. The delegate is not retained
 *
 *  @see FYBInterstitialControllerDelegate
 */
@property (nonatomic, weak) id<FYBInterstitialControllerDelegate> delegate;

/**
 *  Requests an Interstitial
 *
 *  @discussion You need to set the delegate property of this controller in order to be aware of the request and to show the Interstitial cycle
 *
 *  @see FYBInterstitialControllerDelegate
 */
- (void)requestInterstitial;

/**
 *  Same as -requestInterstitial but accepts a FYBRequestParameters object as parameter. Through this object you can add custom parameters to the request
 *  and also specify a placementId
 *
 *  @param parameters A configured instance of FYBRequestParameters
 */
- (void)requestInterstitialWithParameters:(FYBRequestParameters *)parameters;

/**
 *  Presents an Interstitial
 *
 *  @discussion One of the requestInterstitial methods above needs to be called before presenting the controller
 *              You also need to make sure that an Interstitial has been received before trying to present the controller. For that you need to implement
 *              the FYBInterstitialControllerDelegate protocol and set the delegate property of the controller
 *
 *  @param viewController The view controller where the controller is presented
 */
- (void)presentInterstitialFromViewController:(UIViewController *)viewController;


/**
 *  Please use [FyberSDK interstitialController] instead
 */
- (instancetype)init __attribute__((unavailable("not available, use [FyberSDK interstitialController] instead")));

@end
