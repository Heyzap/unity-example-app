//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

#import "FYBLogLevel.h"

@protocol FYBLogAppender;

#define LogInvocation FYBLogDebug(@"%s", __PRETTY_FUNCTION__)

#define FYBLogDebug(...) _FYBLogDebug(__VA_ARGS__)
#define FYBLogInfo(...) _FYBLogInfo(__VA_ARGS__)
#define FYBLogWarn(...) _FYBLogWarn(__VA_ARGS__)
#define FYBLogError(...) _FYBLogError(__VA_ARGS__)

void FYBLogSetLevel(FYBLogLevel level);
void _FYBLogDebug(NSString *format, ...);
void _FYBLogInfo(NSString *format, ...);
void _FYBLogWarn(NSString *format, ...);
void _FYBLogError(NSString *format, ...);

@interface FYBLogger : NSObject

+ (instancetype)sharedInstance;

+ (void)addLogger:(id<FYBLogAppender>)logger;
+ (void)removeLogger:(id<FYBLogAppender>)logger;
+ (void)removeAllLoggers;

+ (void)checkDynamicLogLevel;

- (void)logFormat:(NSString *)format arguments:(va_list)arguments;

@end
