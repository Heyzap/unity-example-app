//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

/**
 * The domain of NSError instances created by FYBIntegrationAnalyzer
 */
FOUNDATION_EXPORT NSString *const FYBIntegrationAnalyzerErrorDomain;

/**
 * Error codes to describe reasons of failure in the bundle initialization
 */
typedef NS_ENUM(NSInteger, FYBMediationBundleInitializationErrorReason) {
    /** bundle is not present or not compatible with the current version of the SDK */
    FYBMediationBundleInitializationErrorReasonCompatibleBundleNotIntegrated,
    /** bundle not started due to the lack of credentials */
    FYBMediationBundleInitializationErrorReasonMissingCredentials,
    /** bundle not started because network is not enabled in the Fyber Dashboard
      * or because of the Ad Network Initialization Controls settings
      */
    FYBMediationBundleInitializationErrorReasonNetworkNotEnabled
};


/**
 * Error codes to describe the reason of a failure in the SDK integration analysis
 */
typedef NS_ENUM(NSInteger, FYBIntegrationAnalyzerErrorReason) {
    /** integration analysis cannot be performed before starting the SDK */
    FYBIntegrationAnalyzerErrorReasonSDKNotStarted
};
