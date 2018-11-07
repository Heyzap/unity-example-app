//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <UIKit/UIKit.h>

#import "FYBBannerNetworkAdapterDelegate.h"

@class FYBBaseNetwork;
@class FYBBannerSize;


@protocol FYBBannerNetworkAdapter<NSObject>

/**
 * Arbitrary offer data that can be written and read by the Fyber SDK to keep track of context
 * This property needs to be synthesized in the adapter implementation
 */
@property (nonatomic, strong) NSDictionary *offerData;

/**
 * Returns the name of the banner-providing network wrapped by this adapter
 */
- (NSString *)networkName;

/**
 * Sets the delegate to be notified of banner availability and lifecycle
 */
- (void)setDelegate:(id<FYBBannerNetworkAdapterDelegate>)delegate;

/**
 *  Starts the banner adapter with corresponding credentials
 *
 *  @param dict a dictionary with credentials
 *
 *  @return return YES if the adapters started successfully
 */
- (BOOL)startAdapterWithDict:(NSDictionary *)dict;

/**
 *  Checks if third party network supports the size.
 *
 *  @param size size of the banner
 *
 *  @return YES if the third party network supports the provided size
 */
- (BOOL)supportsBannerWithSize:(FYBBannerSize *)size;

/**
 *  Requests banner using third party sdk
 *
 *  @param size size of the banner
 */
- (void)requestBannerWithSize:(FYBBannerSize *)size;

/**
 *  Removes strong reference to the banner view and clean up. After calling this function, new banner needs to be requested in order to display one.
 */
- (void)destroyBanner;

/**
 *  Sets instance of network object, which is used to access network specific properties.
 *
 *  @param network instance of network adapter class
 */
- (void)setNetwork:(FYBBaseNetwork *)network;

@end
