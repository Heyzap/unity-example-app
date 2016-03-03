/*
 * Copyright (c) 2015, Heyzap, Inc.
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

@protocol HZAdsDelegate;

/** HZInterstitialAd is responsible for fetching and showing interstitial ads. All methods on this class must be called from the main queue. */
@interface HZInterstitialAd : NSObject

/** Fetches a new ad from Heyzap.  */
+ (void)fetch;

/**
 *  Fetches an interstitial ad.
 *
 *  @param completion completion A block called when the ad is fetched or failed to fetch. result contains whether or not the fetch was successful, and if not, error contains the reason why.
 
 */
+ (void)fetchWithCompletion:(void (^)(BOOL result, NSError *error))completion;

/** Whether or not an interstitial ad is available to show. */
+ (BOOL)isAvailable;

/** Shows an interstitial ad. */
+ (void)show;

#pragma mark - Callbacks

/** Sets the delegate to receive the messages listed in the `HZAdsDelegate` protocol.
 
 @param delegate The object to receive the callbacks. 
 */
+ (void)setDelegate:(id<HZAdsDelegate>)delegate;

@end
