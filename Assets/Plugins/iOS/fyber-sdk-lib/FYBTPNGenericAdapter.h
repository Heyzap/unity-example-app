//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>
#import "FYBTPNVideoAdapter.h"
#import "FYBRewardedVideoNetworkAdapterDelegate.h"

/**
 *  Generic adapter that wraps a FYBRewardedVideoNetworkAdapter
 *
 *  We must continue to maintain this class for backwards compatibility. All Network
 *  adapters are encapsulated in a wrapper object of this type
 */
@interface FYBTPNGenericAdapter : NSObject<FYBTPNVideoAdapter, FYBRewardedVideoNetworkAdapterDelegate>

- (id)initWithVideoNetworkAdapter:(id<FYBRewardedVideoNetworkAdapter>)adapter;

@end
