//
//  HZBannerAdWrapper.h
//  Heyzap
//
//  Created by Maximilian Tagher on 3/6/15.
//  Copyright (c) 2015 Heyzap. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@class HZBannerAdOptions;

/**
 *  Locations where Heyzap can automatically place the banner.
 *
 *  See `placeBannerInView:position:options:success:failure:` for details on how this interacts with top/bottom layout guides in iOS 7+.
 */
typedef NS_ENUM(NSUInteger, HZBannerPosition){
    /**
     *  Option for placing the banner at the top of the view.
     */
    HZBannerPositionTop,
    /**
     *  Option for placing the banner at the bottom of the view.
     */
    HZBannerPositionBottom,
};

// In addition to the delegate methods, Heyzap also posts `NSNotification`s for each event. Each notification's `object` property will be the `HZBannerAd` instance issuing the notification, when applicable.
// This approach gives you the flexibility to have multiple objects listening for information about banners.
// Since notifications aren't coupled to individual `HZBannerAd` instances, they also ease integration with components not directly related to displaying banners (e.g. an analytics object tracking clicks).
// The `userInfo` property of the NSNotifications may contain more information. These keys may be present in the dictionary: `HZAdTagUserInfoKey` (from `HeyzapAds.h`), `HZNetworkNameUserInfoKey` (from `HeyzapAds.h`), and `NSUnderlyingErrorKey`.
extern NSString * const kHZBannerAdDidReceiveAdNotification;
extern NSString * const kHZBannerAdDidFailToReceiveAdNotification;
extern NSString * const kHZBannerAdWasClickedNotification;
extern NSString * const kHZBannerAdWillPresentModalViewNotification;
extern NSString * const kHZBannerAdDidDismissModalViewNotification;
extern NSString * const kHZBannerAdWillLeaveApplicationNotification;
extern NSString * const kHZBannerAdDidResizeNotification;


@class HZBannerAd;

@protocol HZBannerAdDelegate <NSObject>

@optional

/// @name Ad Request Notifications
#pragma mark - Ad Request Notifications

/**
 *  Called when the banner ad loads or refreshes itself.
 */
- (void)bannerDidReceiveAd:(HZBannerAd *)banner;

/**
 *  Called when the banner ad fails to load.
 *
 *  @param error An error describing the failure. 
 *
 *  If the underlying ad network provided an `NSError` object, it will be accessible in the `userInfo` dictionary
 *  with the `NSUnderlyingErrorKey`.
 */
- (void)bannerDidFailToReceiveAd:(HZBannerAd *)banner error:(NSError *)error;

/// @name Click-time Notifications
#pragma mark - Click-time Notifications

/**
 *  Called when the user clicks the banner ad.
 */
- (void)bannerWasClicked:(HZBannerAd *)banner;
/**
 *  Called when the banner ad will present a modal view controller, after the user clicks the ad.
 */
- (void)bannerWillPresentModalView:(HZBannerAd *)banner;
/**
 *  Called when a presented modal view controller is dismissed by the user.
 */
- (void)bannerDidDismissModalView:(HZBannerAd *)banner;
/**
 *  Called when a user clicks a banner ad that causes them to leave the application.
 */
- (void)bannerWillLeaveApplication:(HZBannerAd *)banner;

/**
 *  Called when a user clicks a banner ad that causes it's resizing.
 */
- (void)banner:(HZBannerAd *)banner didResizeToFrame:(CGRect)frame;

@end

/**
 *  A view containing a mediated banner ad.
 */
@interface HZBannerAd : UIView

/**
 *  Fetches a banner and places it in the view. This is the simplest way to use banners.
 *
 *  @param view       The view to place the banner in. If `view == options.presentingViewController.view`, the view controller's top/bottom layout guides are taken into consideration when placing the view.
 *  @param position   The position, either top or bottom, to place the view in.
 *  @param options    Configuration options to use for the banner.
 *  @param success    A block called if we fetch a banner.
 *  @param failure    A block called if we fail to fetch a banner.
 */
+ (void)placeBannerInView:(UIView *)view
                 position:(HZBannerPosition)position
                  options:(HZBannerAdOptions *)options
                  success:(void (^)(HZBannerAd *banner))success
                  failure:(void (^)(NSError *error))failure;

/**
 *  Fetches a banner and returns it in the `success` callback. You can add the `banner` as a subview from the callback, or keep a strong reference to it and add it to the screen later.
 *
 *  @param options Configuration options to use for the banner.
 *  @param success A block called if we fetch a banner.
 *  @param failure A block called if we fail to fetch a banner.
 */
+ (void)requestBannerWithOptions:(HZBannerAdOptions *)options
                         success:(void (^)(HZBannerAd *banner))success
                         failure:(void (^)(NSError *error))failure;

/**
 *  Returns a string describing the dimensions of the banner ad in pixels like `0.0 0.0 320.0 50.0` (`origin_x origin_y width height`). Used by our Unity SDK.
 */
- (NSString *) dimensionsDescription;

/**
 *  The delegate for the banner ad.
 */
@property (nonatomic, weak) id<HZBannerAdDelegate> delegate;

/**
 *  The options used to create the banner ad. You can use this property to access things like the `tag` or `presentingViewController` for the banner.
 */
@property (nonatomic, readonly, copy) HZBannerAdOptions *options;

/**
 *  An identifier of the ad network.
 * 
 *  This will be one of the network constants in HeyzapAds.h, e.g. `HZNetworkFacebook`
 */
@property (nonatomic, strong, readonly) NSString *mediatedNetwork;

@end
