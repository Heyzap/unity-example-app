//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

@protocol FYBPrecachingNetworkAdapter <NSObject>

@required

/**
 *  Starts precaching ads of the Third Party Network
 */
- (void)startPrecaching;

/**
 *  Pauses precaching of the Third Party Network
 */
- (void)pausePrecaching;

/**
 *  Resumes precaching of the Third Party Network
 */
- (void)resumePrecaching;

@end
