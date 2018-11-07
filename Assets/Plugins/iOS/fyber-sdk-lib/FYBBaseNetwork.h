//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import "FYBInterstitialNetworkAdapter.h"
#import "FYBPrecachingNetworkAdapter.h"
#import "FYBBannerNetworkAdapter.h"

@protocol FYBTPNVideoAdapter;
@class FYBAdapterOptions;


typedef NS_OPTIONS(NSUInteger, FYBNetworkSupport) {
    FYBNetworkSupportNone = 0,
    FYBNetworkSupportRewardedVideo = 1 << 0,
    FYBNetworkSupportInterstitial = 1 << 1,
    FYBNetworkSupportBanner = 1 << 2
};


/**
 Abstract Class for provider
 This class provides common functionality between all networks, such as initializing the underlying SDKs, instantiating the suitable adapters, and to store information about the supported services
 */
@interface FYBBaseNetwork : NSObject

/** The ad formats that are supported by this network */
@property (nonatomic, assign, readonly) FYBNetworkSupport supportedServices;

/** The rewarded video adapter to be used to access a network */
@property (nonatomic, strong, readonly) id<FYBTPNVideoAdapter> rewardedVideoAdapter;

/** The interstitial adapter to be used to access a network */
@property (nonatomic, strong, readonly) id<FYBInterstitialNetworkAdapter> interstitialAdapter;

/** The banner adapter to be used to access a network */
@property (nonatomic, strong, readonly) id<FYBBannerNetworkAdapter> bannerAdapter;

/** The name of the network */
@property (nonatomic, copy, readonly) NSString *name;

/**
* The API version that the SDK will use for compatibility check
*/
+ (NSUInteger)apiVersion;

/**
* A human readable version of the adapter bundle.
*/
+ (NSString *)bundleVersion;

- (BOOL)startNetwork:(NSString *)networkName options:(FYBAdapterOptions *)adapterOptions;

- (BOOL)startWithOptions:(FYBAdapterOptions *)options;
- (void)startInterstitialAdapter:(NSDictionary *)data;
- (void)startRewardedVideoAdapter:(NSDictionary *)data;
- (void)startBannerAdapter:(NSDictionary *)data;

- (void)setGDPRConsent:(NSInteger)gdprConsent;

@end
