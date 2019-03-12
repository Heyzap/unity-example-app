//
//  HZFyberVirtualCurrencyClient.h
//  Heyzap
//
//  Created by Monroe Ekilah on 7/3/17.
//  Copyright © 2017 Heyzap. All rights reserved.
//

/*
 * Copyright (c) 2017, Heyzap, Inc.
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
 * * Neither the name of 'Heyzap, Inc.' nor the names of its contributors
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

@protocol HZFyberVirtualCurrencyClientDelegate;

/**
 *  Use this class to request an update from Fyber's Virtual Currency Server (VCS) product regarding how much currency a user has earned since the last time you asked.
 
 *  @discussion Virtual currency can be earned via OfferWall and Rewarded Video views from Fyber. If you only want to use this class for OfferWall rewarding (most publishers) and not for RV rewarding, make sure to set the rewards up on the Fyber dashboard such that RVs don't produce any virtual currency, or at least not the same currency as offerwall interactions so you can tell the difference. Otherwise, you may end up rewarding users twice for RV views from the Fyber ad network.
 */
@interface HZFyberVirtualCurrencyClient : NSObject

/**
 *  Accessor for the singleton client. Use this instead of `init`ing your own copy.
 */
+ (nonnull instancetype)sharedClient;

/**
 *  Whether or not the Fyber SDK's Virtual Currency Client class is available for us to forward messages to. If `false`, the Fyber SDK is not present & none of the methods here will do anything.
 */
- (BOOL)available;

/**
 *  Latest transaction ID for your user and app ID, as reported by the server. It is used to keep track of new transactions between invocations to requestDeltaOfCoins
 */
@property (nonatomic, copy, readonly, nullable) NSString *latestTransactionId;

/**
 *  The object that acts as the delegate of the virtual currency client
 *
 *  @discussion The delegate must adopt the HZFyberVirtualCurrencyClientDelegate protocol. The delegate is not retained
 */
@property (weak, nonatomic, nullable) id<HZFyberVirtualCurrencyClientDelegate> delegate;

/**
 *  Requests the amount of currency earned since the last time this method was invoked for the default currency
 */
- (void)requestDeltaOfCurrency;

/**
 *  Requests the amount of currency earned since the last time this method was invoked for the specified currency
 */
- (void)requestDeltaOfCurrency:(nullable NSString *)currencyId;

/**
 *  Please use [HZFyberVirtualCurrencyClient sharedClient] instead.
 */
- (nullable instancetype)init __attribute__((unavailable("This is a singleton class. Use `[HZFyberVirtualCurrencyClient sharedClient]` instead.")));

@end


@interface HZFyberVirtualCurrencyResponse : NSObject

/**
 *  Latest transaction ID for your user and app IDs, as reported by the server. It is used to keep track of new transactions between invocations to requestDeltaOfCoins
 */
@property (nonatomic, copy, nonnull) NSString *latestTransactionId;

/**
 *  The ID of the currency being earned by the user
 */
@property (nonatomic, copy, nonnull) NSString *currencyId;

/**
 *  The name of the currency being earned by the user
 */
@property (nonatomic, copy, nonnull) NSString *currencyName;

/**
 *  Amount of currency earned by the user
 */
@property (nonatomic, assign) CGFloat deltaOfCurrency;

@end


/**
 *  This protocol allows a delegate to receive success and failure callbacks after sending `requestDeltaOfCurrency` to the HZFyberVirtualCurrencyClient.
 */
@protocol HZFyberVirtualCurrencyClientDelegate<NSObject>

@optional

- (void)didReceiveVirtualCurrencyResponse:(nonnull HZFyberVirtualCurrencyResponse *)response;

- (void)didFailToReceiveVirtualCurrencyResponse:(nonnull NSError *)error;

@end

