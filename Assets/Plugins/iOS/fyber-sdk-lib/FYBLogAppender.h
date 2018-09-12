//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

@protocol FYBLogAppender<NSObject>

+ (id<FYBLogAppender>)logger;
- (void)logFormat:(NSString *)format arguments:(va_list)arguments;

@end
