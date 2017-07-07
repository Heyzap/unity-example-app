//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>
#import "FYBMediationBundleInfo.h"

/**
 *  Object that contains information about the FyberSDK and the mediated networks integration
 */
@interface FYBIntegrationReport : NSObject

/**
 *  The version of the Test Suite
 */
@property (nonatomic, strong, readonly) NSString *testSuiteVersion;

/**
 *  The version of the FyberSDK
 */
@property (nonatomic, strong, readonly) NSString *sdkVersion;

/**
 *  The App ID that was used to start the SDK
 */
@property (nonatomic, strong, readonly) NSString *appId;

/**
 *  The User ID that was used to start the SDK
 */
@property (nonatomic, strong, readonly) NSString *userId;

/**
 *  The cookie accept policy configuration for the shared cookie storage
 */
@property (nonatomic, assign, readonly) NSHTTPCookieAcceptPolicy cookieAcceptPolicy;

/**
 *  The list of bundles started by the SDK
 */
@property (nonatomic, strong, readonly) NSArray<FYBMediationBundleInfo *> *startedBundles;

/**
 *  The list of bundles not started by the SDK
 */
@property (nonatomic, strong, readonly) NSArray<FYBMediationBundleInfo *> *unstartedBundles;

@end
