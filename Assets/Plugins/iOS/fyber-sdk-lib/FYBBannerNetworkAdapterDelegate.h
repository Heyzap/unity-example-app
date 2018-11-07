//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <UIKit/UIKit.h>
#import "FYBBannerView.h"

@protocol FYBBannerNetworkAdapter;

@protocol FYBBannerNetworkAdapterDelegate<NSObject>
@required

- (void)adapter:(id<FYBBannerNetworkAdapter>)adapter didReceiveBanner:(FYBBannerView *)banner;

- (void)adapter:(id<FYBBannerNetworkAdapter>)adapter didFailToReceiveBannerWithError:(NSError *)error;

- (void)bannerWasClicked:(id<FYBBannerNetworkAdapter>)adapter;

- (void)adapterWillPresentModalView:(id<FYBBannerNetworkAdapter>)adapter;

- (void)adapterDidDismissModalView:(id<FYBBannerNetworkAdapter>)adapter;

- (void)adapterWillLeaveApplication:(id<FYBBannerNetworkAdapter>)adapter;

- (UIViewController *)rootViewController;

- (UIViewController *)modalViewController;

@end
