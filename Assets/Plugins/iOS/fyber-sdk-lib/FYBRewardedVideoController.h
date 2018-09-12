//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <UIKit/UIKit.h>

#import "FYBRewardedVideoControllerDelegate.h"
#import "FYBVirtualCurrencyClientDelegate.h"
#import "FYBRequestParameters.h"
#import "FYBRewardedVideoControllerState.h"

/**
 *  Provides methods to request and play Rewarded Videos
 */
@interface FYBRewardedVideoController : NSObject

/**
 *  The current state of the Rewarded Video controller
 *
 *  @see FYBRewardedVideoControllerState
 */
@property (nonatomic, assign, readonly) FYBRewardedVideoControllerState state;

/**
 *  The object that acts as the delegate of the Rewarded Video controller
 *
 *  @discussion The delegate must adopt the FYBRewardedVideoControllerDelegate protocol. The delegate is not retained
 *
 *  @see FYBRewardedVideoControllerDelegate
 */
@property (nonatomic, weak) id<FYBRewardedVideoControllerDelegate>delegate;

/**
 *  The object that acts as the delegate of the Virtual Currency client
 *
 *  @discussion The delegate must adopt the FYBVirtualCurrencyClientDelegate protocol. The delegate is not retained
 *
 *  @discussion If set, a request for the user's virtual currencies will be automatically sent once this one has been engaged
 *              The retrieved currency will be the default. If you want to specify a currencyId for this request you need to pass
 *              a configured FYBRequestParameters object to the [FYBRewardedVideoController requestVideoWithParameters:] method
 *
 *  @see FYBVirtualCurrencyClientDelegate
 */
@property (nonatomic, weak) id<FYBVirtualCurrencyClientDelegate>virtualCurrencyClientDelegate;

/**
 *  If set to YES the SDK will show a toast-like notification to the user after completing an engagement
 *  An example notification would be "Thanks! Your reward will be paid out shortly"
 *  
 *  @discussion Default value is YES
 */
@property (nonatomic, assign) BOOL shouldShowToastOnCompletion;

/**
 *  Queries the server for Rewarded Video offers
 *
 *  @discussion You need to set the delegate property of this controller in order to be aware of the request and show cycle of the Rewarded Video
 *
 *  @see FYBVirtualCurrencyClientDelegate
 */
- (void)requestVideo;

/**
 *  Same as -requestVideo but accepts a FYBRequestParameters object as parameter. Through this object you can add custom parameters to the request
 *  You can also specify a currencyId that will be used for the virtual currency request if you don't want to request the default currency
 *
 *  @param parameters A configured instance of FYBRequestParameters
 */
- (void)requestVideoWithParameters:(FYBRequestParameters *)parameters;

/**
 *  Presents the Rewarded Video controller
 *
 *  @discussion One of the requestVideo methods above needs to be called before presenting the controller
 *              You must ensure a Rewarded Video has been received before trying to present the controller. For that you need to implement
 *              the FYBRewardedVideoControllerDelegate protocol and set the delegate property of the controller
 *
 *  @param viewController The view controller on top of which the controller is presented
 */
- (void)presentRewardedVideoFromViewController:(UIViewController *)viewController;

/**
 *  Please use [FyberSDK rewardedVideoController] instead
 */
- (instancetype)init __attribute__((unavailable("not available, use [FyberSDK rewardedVideoController] instead")));

@end
