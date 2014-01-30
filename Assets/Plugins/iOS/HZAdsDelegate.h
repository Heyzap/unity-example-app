/*
 * Copyright (c) 2013, Smart Balloon, Inc.
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
 * * Neither the name of 'MoPub Inc.' nor the names of its contributors
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

/** The `HZAdsDelegate` protocol provides global information about our ads. If you want to know if we had an ad to show after calling `showAd` (to e.g. fallback to another ads provider), I highly recommend using the `showAd:completion:` method instead. */
@protocol HZAdsDelegate <NSObject>

@optional

#pragma mark - Showing ads callbacks

/** Called when we succesfully show an ad. */
- (void)didShowAdWithTag: (NSString *) tag;

/** Called when `showAd` (or a variant) is called but we don't have an ad to show. Because we prefetch ads, this should be a rare occurence.
 @param error An `NSError` whose `userInfo` dictionary contains a description of the problem inside the `NSLocalizedDescriptionKey` key.
 */
- (void)didFailToShowAdWithTag: (NSString *) tag andError: (NSError *)error;

/** Called when we receive a valid ad from our server. */
- (void)didReceiveAdWithTag: (NSString *) tag;
/** Called when our server fails to send a valid ad. This should be a rare occurence; only when our server returns invalid data or has a 500 error, etc. */
- (void)didFailToReceiveAdWithTag: (NSString *) tag;

/** Called when the user clicks on an ad. */
- (void)didClickAdWithTag: (NSString *) tag;
/** Called when the ad is dismissed. */
- (void)didHideAdWithTag: (NSString *) tag;

@end
