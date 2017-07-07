//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>
#import "FYBTPNVideoAdapter.h"
#import "FYBRewardedVideoNetworkAdapter.h"
#import "FYBRewardedVideoNetworkAdapterDelegate.h"

@interface FYBTPNGenericAdapter : NSObject<FYBTPNVideoAdapter, FYBRewardedVideoNetworkAdapterDelegate>

- (id)initWithVideoNetworkAdapter:(id<FYBRewardedVideoNetworkAdapter>)adapter;

@end
