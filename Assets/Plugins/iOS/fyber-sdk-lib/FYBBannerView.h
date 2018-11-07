//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <UIKit/UIKit.h>
#import "FYBBannerSize.h"

@protocol FYBBannerViewDelegate;

@interface FYBBannerView : UIView

@property (nonatomic, weak) id<FYBBannerViewDelegate> delegate;

- (instancetype)initWithMediatedBanner:(UIView *)mediatedBannerView size:(FYBBannerSize *)size autoresizing:(BOOL)isAutoresizing;

@end
