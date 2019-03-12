/*
 * Copyright (c) 2014, Smart Balloon, Inc.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:
 *
 * * Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * * Redistributions in binary form must reproduce the above copyright
 *   notice, this list of conditions and the following disclaimer in the
 *   documentation and/or other materials provided with the distribution.
 *
 * * Neither the name of 'Smart Balloon, Inc.' nor the names of its contributors
 *   may be used to endorse or promote products derived from this software
 *   without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
 * TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#import <Foundation/Foundation.h>
#import <os/log.h>

/**
 *  The level of logging the SDK should do.
 */
typedef enum {
    HZDebugLevelVerbose = 3,
    HZDebugLevelInfo = 2,
    HZDebugLevelError = 1,
    HZDebugLevelSilent = 0
} HZDebugLevel;

extern NSString *const kHZLogThirdPartyLoggingEnabledChangedNotification;

/**
 *  A class Heyzap uses to log errors and information. You should only need to set the debugLevel 
 @see setDebugLevel
 */
@interface HZLog : NSObject

/**
 *  The level of logging the SDK should do.
 *
 *  @param debugLevel The extent to which to log.
 */
+ (void) setDebugLevel: (HZDebugLevel) debugLevel;
+ (HZDebugLevel) debugLevel;

+ (void) debug: (NSString *) message;
+ (void) info: (NSString *) message;
+ (void) error: (NSString *) message;
+ (void) always: (NSString *) message;
+ (void) log: (NSString *) message atDebugLevel: (HZDebugLevel) debugLevel;

#define HZGenericLog(_debugLevel, fmt, ...) do { \
if (_debugLevel <= [HZLog debugLevel]) { \
[HZLog log:[NSString stringWithFormat:fmt,##__VA_ARGS__] atDebugLevel:_debugLevel]; \
} \
} while (0)


#define HZDLog(fmt, ...) HZGenericLog(HZDebugLevelVerbose, fmt, ##__VA_ARGS__)
#define HZILog(fmt, ...) HZGenericLog(HZDebugLevelInfo, fmt, ##__VA_ARGS__)
#define HZELog(fmt, ...) HZGenericLog(HZDebugLevelError, fmt, ##__VA_ARGS__)
#define HZAlwaysLog(fmt, ...) HZGenericLog(HZDebugLevelSilent, fmt, ##__VA_ARGS__)

/**
 *  If this is set to YES, Heyzap will attempt to enable logging on all mediated networks' SDKs, if possible.
 *  Note that, depending on the implementation of each mediated SDK, this may or may not be effective after an SDK is initialized, so it is best to set this property before starting the Heyzap SDK.
 *  Defaults to NO.
 */
+ (BOOL) isThirdPartyLoggingEnabled;
+ (void) setThirdPartyLoggingEnabled:(BOOL)enabled;


@end
