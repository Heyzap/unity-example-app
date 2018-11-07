//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import "FYBRewardedVideoNetworkAdapter.h"

@protocol FYBRewardedVideoNetworkAdapterDelegate;

/**
 *  Wrapper interface to FYBRewardedVideoNetworkAdapter
 *
 *  We must continue to maintain this protocol for backwards compatibility.
 */
@protocol FYBTPNVideoAdapter<NSObject>

- (void)setNetwork:(FYBBaseNetwork *)network;
- (BOOL)startAdapterWithDictionary:(NSDictionary *)dict;

- (id<FYBRewardedVideoNetworkAdapter>)networkAdapter;

@end
