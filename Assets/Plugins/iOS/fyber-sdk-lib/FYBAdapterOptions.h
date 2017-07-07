//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

@class FYBSDKOptions;

@interface FYBAdapterOptions : NSObject

@property (nonatomic, copy) NSString *userId;

@property (nonatomic, assign) BOOL startVideoPrecaching;

@property (nonatomic, strong) NSDictionary *data;

+ (instancetype)optionsWithSDKOptions:(FYBSDKOptions *)options data:(NSDictionary *)data;

@end
