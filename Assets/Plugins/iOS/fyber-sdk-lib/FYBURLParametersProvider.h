//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

@protocol FYBURLParametersProvider<NSObject>

@required
- (NSDictionary *)dictionaryWithKeyValueParameters;

@end
