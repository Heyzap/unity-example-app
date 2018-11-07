//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

static NSString *const FyberErrorDomain = @"FyberErrorDomain";

typedef NS_ENUM(NSInteger, FYBErrorCode) {
    // SDK start
    FYBErrorCodeSDKNotStarted                   = 0001,

    // General ad request errors
    FYBErrorCodeNoNetworkConnection             = 1000,
    FYBErrorCodeNotReady                        = 1001,
    FYBErrorCodeNoOffers                        = 1002,
    FYBErrorCodeMediation                       = 1003,

    // Offer request
    FYBErrorCodeRequesting                      = 2010,
    FYBErrorCodeRequestTimeout                  = 2011,
    FYBErrorCodeRequestJSON                     = 2012,
    FYBErrorCodeRequestServer                   = 2013,

    // Offer validation
    FYBErrorCodeInvalidOffer                    = 2020,
    FYBErrorCodeInvalidSize                     = 2021,

    // Offer show
    FYBErrorCodeShowing                         = 2030,
    FYBErrorCodeShowPreload                     = 2031,
    FYBErrorCodeShowTimeout                     = 2032,
    FYBErrorCodeShowVideoTimeout                = 2033,
    FYBErrorCodeShowLoading                     = 2034,
    FYBErrorCodeShowInvalidJSResponse           = 2040,

    // Mediation
    FYBErrorCodeMediationOther                  = 5000,
    FYBErrorCodeMediationSDK                    = 5001,
    FYBErrorCodeMediationInvalidConfiguration   = 5002,
    FYBErrorCodeMediationNoFill                 = 5003,
    FYBErrorCodeMediationServer                 = 5004,
    FYBErrorCodeMediationNetwork                = 5005,
    FYBErrorCodeMediationAdTimeOut              = 5006,
    FYBErrorCodeMediationNoPlacementId          = 5007,
    FYBErrorCodeMediationNotReady               = 5008,
    FYBErrorCodeMediationInvalidSize            = 5009,
    FYBErrorCodeMediationLoading                = 5010,

    // Other
    FYBErrorCodeExternalStoreKit                = 9000,
};
