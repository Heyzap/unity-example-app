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

#define HZ_VIDEO_KLASS @"HZVideoAd"
#define HZ_INTERSTITIAL_KLASS @"HZInterstitialAd"
#define HZ_INCENTIVIZED_KLASS @"HZIncentivizedAd"

@interface HeyzapUnityAdDelegate : NSObject<HZAdsDelegate,HZIncentivizedAdDelegate>

@property (nonatomic, strong) NSString *klassName;

- (id) initWithKlassName: (NSString *) klassName;
- (void) sendMessageForKlass: (NSString *) klass withMessage: (NSString *) message andTag: (NSString *) tag;

@end

@implementation HeyzapUnityAdDelegate

- (id) initWithKlassName: (NSString *) klassName {
    self = [super init];
    if (self) {
        _klassName = klassName;
    }
    
    return self;
}

- (void) didReceiveAdWithTag:(NSString *)tag { [self sendMessageForKlass: self.klassName withMessage: @"available" andTag: tag]; }

- (void) didFailToReceiveAdWithTag:(NSString *)tag { [self sendMessageForKlass: self.klassName withMessage: @"fetch_failed" andTag: tag]; }

- (void) didShowAdWithTag:(NSString *)tag { [self sendMessageForKlass: self.klassName withMessage: @"show" andTag: tag]; }

- (void) didHideAdWithTag:(NSString *)tag { [self sendMessageForKlass: self.klassName withMessage:  @"hide" andTag: tag]; }

- (void) didFailToShowAdWithTag:(NSString *)tag andError:(NSError *)error { [self sendMessageForKlass: self.klassName withMessage:  @"failed" andTag: tag]; }

- (void) didClickAdWithTag:(NSString *)tag { [self sendMessageForKlass: self.klassName withMessage:  @"click" andTag: tag]; }

- (void) didCompleteAdWithTag: (NSString *) tag { [self sendMessageForKlass: self.klassName withMessage:  @"incentivized_result_complete" andTag: tag]; }

- (void) didFailToCompleteAdWithTag: (NSString *) tag { [self sendMessageForKlass: self.klassName withMessage:  @"incentivized_result_incomplete" andTag: tag]; }

- (void) willStartAudio { [self sendMessageForKlass: self.klassName  withMessage: @"audio_starting" andTag:  @""]; }

- (void) didFinishAudio { [self sendMessageForKlass: self.klassName withMessage:  @"audio_finished" andTag:  @""]; }

- (void) sendMessageForKlass: (NSString *) klass withMessage: (NSString *) message andTag: (NSString *) tag {
    NSString *unityMessage = [NSString stringWithFormat: @"%@,%@", message, tag];
    UnitySendMessage("HeyzapAds", "setDisplayState", [unityMessage UTF8String]);
    UnitySendMessage([klass UTF8String], "setDisplayState", [unityMessage UTF8String]);
}

@end

static HeyzapUnityAdDelegate *HZInterstitialDelegate = nil;
static HeyzapUnityAdDelegate *HZIncentivizedDelegate = nil;
static HeyzapUnityAdDelegate *HZVideoDelegate = nil;

extern "C" {

     void hz_ads_start_app(const char *publisher_id, int flags) {
        NSString *publisherID = [NSString stringWithUTF8String: publisher_id];
        
        [HeyzapAds startWithPublisherID: publisherID andOptions: flags andFramework: @"unity3d"];
        [HeyzapAds setDebugLevel: HZDebugLevelVerbose];

        HZIncentivizedDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName: HZ_INCENTIVIZED_KLASS];
        [HZIncentivizedAd setDelegate: HZIncentivizedDelegate];

        HZInterstitialDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName: HZ_INTERSTITIAL_KLASS];
        [HZInterstitialAd setDelegate: HZInterstitialDelegate];
        
        HZVideoDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName: HZ_VIDEO_KLASS];
        [HZVideoAd setDelegate: HZVideoDelegate];
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

     void hz_ads_show_incentivized(const char *tag) {
         [HZIncentivizedAd showForTag: [NSString stringWithUTF8String: tag]];
     }

     void hz_ads_hide_incentivized() {
         [HZIncentivizedAd hide];
     }

     void hz_ads_fetch_incentivized(const char *tag) {
        [HZIncentivizedAd fetchForTag: [NSString stringWithUTF8String: tag]];
     }

     bool hz_ads_incentivized_is_available(const char *tag) {
        return [HZIncentivizedAd isAvailableForTag: [NSString stringWithUTF8String: tag]];
     }

     void hz_ads_incentivized_set_user_identifier(const char *identifier) {
        NSString *userID = (identifier == "") ? nil : [NSString stringWithUTF8String: identifier];
        return [HZIncentivizedAd setUserIdentifier: userID];
     }
}