//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

@import Foundation;

typedef NS_ENUM(NSInteger, FYBURLEndpoint) {
    FYBURLEndpointNone = 0,
    FYBURLEndpointActions,
    FYBURLEndpointInstalls,
    FYBURLEndpointVC,
    FYBURLEndpointOfferWall,
    FYBURLEndpointInterstitial,
    FYBURLEndpointBanner,
    FYBURLEndpointInterstitialTracker,
    FYBURLEndpointBannerTracker,
    FYBURLEndpointMBE,
    FYBURLEndpointRV,
    FYBURLEndpointRVTracker,
    FYBURLEndpointAdaptersConfig,
    FYBURLEndpointVideoCache,
    FYBURLEndpointTestSuiteTracker
};

@protocol FYBURLProvider<NSObject>

@required
- (NSString *)urlForEndpoint:(FYBURLEndpoint)endpointKey;
- (void)enableSSL:(BOOL)ssl;
- (void)enableSSL:(BOOL)ssl endpoint:(FYBURLEndpoint)endpointKey;

@end


@interface FYBBaseURLProvider : NSObject<FYBURLProvider>

+ (FYBBaseURLProvider *)sharedInstance;
- (NSString *)urlForEndpoint:(FYBURLEndpoint)endpointKey;

@property (strong, nonatomic) id<FYBURLProvider> customProvider;

- (void)restoreUrlsToDefault;

@end
