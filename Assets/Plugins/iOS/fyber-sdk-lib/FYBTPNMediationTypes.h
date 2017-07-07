//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSInteger, FYBTPNValidationResult) {
    FYBTPNValidationNoVideoAvailable,
    FYBTPNValidationNoSdkIntegrated,
    FYBTPNValidationTimeout,
    FYBTPNValidationNetworkError,
    FYBTPNValidationDiskError,
    FYBTPNValidationError,
    FYBTPNValidationSuccess
};

NSString *FYBTPNValidationResultToString(FYBTPNValidationResult validationResult);

typedef NS_ENUM(NSInteger, FYBTPNVideoEvent) {
    FYBTPNVideoEventStarted,
    FYBTPNVideoEventAborted,
    FYBTPNVideoEventFinished,
    FYBTPNVideoEventClosed,
    FYBTPNVideoEventNoVideo,
    FYBTPNVideoEventTimeout,
    FYBTPNVideoEventNoSdk,
    FYBTPNVideoEventError
};

NSString *FYBTPNVideoEventToString(FYBTPNVideoEvent event);

typedef void (^FYBTPNValidationResultBlock)(NSString *tpnKey, FYBTPNValidationResult validationResult);
typedef void (^FYBTPNVideoEventsHandlerBlock)(NSString *tpnKey, FYBTPNVideoEvent event);
