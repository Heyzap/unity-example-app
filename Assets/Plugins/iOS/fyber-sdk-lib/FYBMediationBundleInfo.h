//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

/**
 * Object that contains information about a mediated network
 */
@interface FYBMediationBundleInfo : NSObject

/** 
 * The name of the network
 */
@property (nonatomic, copy, readonly, nonnull) NSString *networkName;

/**
 * The version of the bundle
 */
@property (nonatomic, copy, readonly, nullable) NSString *version;

/**
 * The error that occurred during the initialization of the bundle, if any.
 *
 * @discussion The possible error codes for this property are defined in FYBMediationBundleInitializationErrorReason.
 */
@property (nonatomic, strong, readonly, nullable) NSError *initializationError;

@end
