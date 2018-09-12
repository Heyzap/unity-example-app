//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import "FYBUser.h"


@interface FYBUser (Unity)

- (NSString *)dataWithKey:(NSString *)key;
- (NSString *)setDataWithKey:(NSString *)key value:(id)value;

@end

FOUNDATION_EXPORT NSString *const FYBUserActionParam;

FOUNDATION_EXPORT NSString *const FYBUserValueParam;
FOUNDATION_EXPORT NSString *const FYBUserKeyParam;
FOUNDATION_EXPORT NSString *const FYBUserSuccessParam;
FOUNDATION_EXPORT NSString *const FYBUserErrorParam;

FOUNDATION_EXPORT NSString *const FYBUserGetAction;
FOUNDATION_EXPORT NSString *const FYBUserPutAction;
