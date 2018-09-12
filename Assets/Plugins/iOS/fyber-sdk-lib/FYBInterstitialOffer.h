//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>
#import "FYBBaseOffer.h"
#import "FYBInterstitialCreativeType.h"

@interface FYBInterstitialOffer : FYBBaseOffer

@property (nonatomic, copy, readonly) NSString *tpnPlacementId;
@property (nonatomic, assign, readonly) FYBInterstitialCreativeType creativeType;

@end
