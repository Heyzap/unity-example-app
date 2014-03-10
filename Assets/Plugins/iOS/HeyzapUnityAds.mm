//
//  HeyzapUnitySDK.m
//
//  Copyright 2014 Smart Balloon, Inc. All Rights Reserved
//
//  Permission is hereby granted, free of charge, to any person
//  obtaining a copy of this software and associated documentation
//  files (the "Software"), to deal in the Software without
//  restriction, including without limitation the rights to use,
//  copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following
//  conditions:
//
//  The above copyright notice and this permission notice shall be
//  included in all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//  OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
//  HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//  WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//  OTHER DEALINGS IN THE SOFTWARE.
//

#import "HeyzapAds.h"
#import "HZInterstitialAd.h"
#import "HZVideoAd.h"
#import "HZIncentivizedAd.h"

extern void UnitySendMessage(const char *, const char *, const char *);

@interface HeyzapUnityAdDelegate : NSObject<HZAdsDelegate,HZIncentivizedAdDelegate>
@end

@implementation HeyzapUnityAdDelegate

- (void) didReceiveAdWithTag:(NSString *)tag {
    UnitySendMessage("HeyzapAds", "setDisplayState", [[NSString stringWithFormat: @"%@,%@", @"available", tag] UTF8String]);
}

- (void) didFailToReceiveAdWithTag:(NSString *)tag {
    UnitySendMessage("HeyzapAds", "setDisplayState", [[NSString stringWithFormat: @"%@,%@", @"fetch_failed", tag] UTF8String]);
}

- (void) didShowAdWithTag:(NSString *)tag {
    UnitySendMessage("HeyzapAds", "setDisplayState", [[NSString stringWithFormat: @"%@,%@", @"show", tag] UTF8String]);
}

- (void) didHideAdWithTag:(NSString *)tag {
    UnitySendMessage("HeyzapAds", "setDisplayState", [[NSString stringWithFormat: @"%@,%@", @"hide", tag] UTF8String]);
}

- (void) didFailToShowAdWithTag:(NSString *)tag andError:(NSError *)error {
    UnitySendMessage("HeyzapAds", "setDisplayState", [[NSString stringWithFormat: @"%@,%@", @"failed", tag] UTF8String]);
}

- (void) didClickAdWithTag:(NSString *)tag {
    UnitySendMessage("HeyzapAds", "setDisplayState", [[NSString stringWithFormat: @"%@,%@", @"click", tag] UTF8String]);
}

- (void)didCompleteAd {
    UnitySendMessage("HeyzapAds", "setDisplayState", "incentivized_result_complete,");
}

- (void)didFailToCompleteAd {
    UnitySendMessage("HeyzapAds", "setDisplayState", "incentivized_result_incomplete,");
}

@end

extern "C" {
    void hz_ads_start(int flags) {
        HeyzapUnityAdDelegate *delegate = [[HeyzapUnityAdDelegate alloc] init];
        [HeyzapAds startWithOptions: flags];
        [HeyzapAds setFramework: @"unity3d"];
        [HeyzapAds setDelegate: delegate];
        [HeyzapAds setIncentiveDelegate: delegate];
    }
    
     void hz_ads_start_app(int flags) {
        HeyzapUnityAdDelegate *delegate = [[HeyzapUnityAdDelegate alloc] init];
        [HeyzapAds startWithOptions: flags];
        [HeyzapAds setFramework: @"unity3d"];
        [HeyzapAds setDelegate: delegate];
        [HeyzapAds setIncentiveDelegate: delegate];
     }

     //Interstitial
     
     void hz_ads_show_interstitial(const char *tag) {
         [HZInterstitialAd showForTag: [NSString stringWithUTF8String: tag]];
     }
     
     void hz_ads_hide_interstitial(void) {
         [HZInterstitialAd hide];
     }
     
     void hz_ads_fetch_interstitial(const char *tag) {
         [HZInterstitialAd fetchForTag: [NSString stringWithUTF8String: tag]];
     }
     
     bool hz_ads_interstitial_is_available(const char *tag) {
         return [HZInterstitialAd isAvailableForTag: [NSString stringWithUTF8String: tag]];
     }

     // Video

     void hz_ads_show_video(const char *tag) {
         [HZVideoAd showForTag: [NSString stringWithUTF8String: tag]];
     }

     void hz_ads_hide_video(void) {
         [HZVideoAd hide];
     }

     void hz_ads_fetch_video(const char *tag) {
         [HZVideoAd fetchForTag: [NSString stringWithUTF8String: tag]];
     }

     bool hz_ads_video_is_available(const char *tag) {
        return [HZVideoAd isAvailable];
     }

     // Incentivized

     void hz_ads_show_incentivized() {
         [HZIncentivizedAd show];
     }

     void hz_ads_hide_incentivized() {
         [HZIncentivizedAd hide];
     }

     void hz_ads_fetch_incentivized(const char *tag) {
        [HZIncentivizedAd fetch];
     }

     bool hz_ads_incentivized_is_available() {
        return [HZIncentivizedAd isAvailable];
     }

     void hz_ads_incentivized_set_user_identifier(const char *identifier) {
        NSString *userID = (identifier == "") ? nil : [NSString stringWithUTF8String: identifier];
        return [HZIncentivizedAd setUserIdentifier: userID];
     }
}