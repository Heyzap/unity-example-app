//
//  HeyzapUnitySDK.m
//
//  Copyright 2015 Smart Balloon, Inc. All Rights Reserved
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
#import "HZIncentivizedAd.h"
#import "HZBannerAdController.h"

extern void UnitySendMessage(const char *, const char *, const char *);

#define HZ_FRAMEWORK_NAME @"unity3d"

#define HZ_INTERSTITIAL_KLASS @"HZInterstitialAd"
#define HZ_INCENTIVIZED_KLASS @"HZIncentivizedAd"
#define HZ_BANNER_KLASS @"HZBannerAd" // No longer the class name, unsure if it needs to stay the same for backwards compat?

@interface HeyzapUnityAdDelegate : NSObject<HZAdsDelegate, HZIncentivizedAdDelegate, HZBannerAdDelegate>

@property (nonatomic, strong) NSString *klassName;

- (id) initWithKlassName: (NSString *) klassName;
- (void) sendMessageForKlass: (NSString *) klass withMessage: (NSString *) message andTag: (NSString *) tag;

extern "C" {
    void hz_ads_start_app(const char *appID, const char *securityToken, HZAdOptions flags);
    
    void hz_ads_show_interstitial(void);
    void hz_ads_fetch_interstitial(void);
    bool hz_ads_interstitial_is_available(void);
    
    void hz_ads_show_incentivized(void);
    void hz_ads_fetch_incentivized(void);
    bool hz_ads_incentivized_is_available(void);
    
    void hz_ads_show_banner(const char *position);
    void hz_ads_hide_banner(void);
    void hz_ads_destroy_banner(void);
}

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

- (void) didCompleteAdWithTag: (NSString *) tag { [self sendMessageForKlass: self.klassName withMessage:  @"incentivized_result_complete" andTag: tag]; }

- (void) didFailToCompleteAdWithTag: (NSString *) tag { [self sendMessageForKlass: self.klassName withMessage:  @"incentivized_result_incomplete" andTag: tag]; }

- (void) willStartAudio { [self sendMessageForKlass: self.klassName  withMessage: @"audio_starting" andTag:  @""]; }

- (void) didFinishAudio { [self sendMessageForKlass: self.klassName withMessage:  @"audio_finished" andTag:  @""]; }

- (void)bannerDidReceiveAd:(HZBannerAdController *)banner {
    [self sendMessageForKlass:self.klassName withMessage:@"loaded" andTag:@""];
}

- (void)bannerDidFailToReceiveAd:(HZBannerAdController *)banner error:(NSError *)error {
    [self sendMessageForKlass:self.klassName withMessage:@"error" andTag:@""];
}

- (void)bannerWasClicked:(HZBannerAdController *)banner {
    [self sendMessageForKlass:self.klassName withMessage:@"click" andTag:@""];
}

- (void) sendMessageForKlass: (NSString *) klass withMessage: (NSString *) message andTag: (NSString *) tag {
    UnitySendMessage([klass UTF8String], "SetCallback", [message UTF8String]);
}

@end

static HeyzapUnityAdDelegate *HZInterstitialDelegate = nil;
static HeyzapUnityAdDelegate *HZIncentivizedDelegate = nil;
static HeyzapUnityAdDelegate *HZBannerDelegate = nil;

extern "C" {
    void hz_ads_start_app(const char *appID, const char *securityToken, HZAdOptions flags) {
        static dispatch_once_t onceToken;
        dispatch_once(&onceToken, ^{
            
            [HeyzapAds startWithAppID:[NSString stringWithUTF8String:appID]
                        securityToken:[NSString stringWithUTF8String:securityToken]
                              options:flags];
            
            HZIncentivizedDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName: HZ_INCENTIVIZED_KLASS];
            [HZIncentivizedAd setDelegate: HZIncentivizedDelegate];
            
            HZInterstitialDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName: HZ_INTERSTITIAL_KLASS];
            [HZInterstitialAd setDelegate: HZInterstitialDelegate];
            
            HZBannerDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName: HZ_BANNER_KLASS];
            [[HZBannerAdController sharedInstance] setDelegate:HZBannerDelegate];
        });
    }
    
    void hz_ads_show_interstitial(void) {
        [HZInterstitialAd show];
    }
    
    void hz_ads_fetch_interstitial(void) {
        [HZInterstitialAd fetch];
    }
    
    bool hz_ads_interstitial_is_available(void) {
        return [HZInterstitialAd isAvailable];
    }
    
    void hz_ads_show_incentivized(void) {
        [HZIncentivizedAd show];
    }
    
    void hz_ads_fetch_incentivized(void) {
        [HZIncentivizedAd fetch];
    }
    
    bool hz_ads_incentivized_is_available(void) {
        return [HZIncentivizedAd isAvailable];
    }
    
    void hz_ads_show_banner(const char *position) {
        // After the banner is loaded initially, this function doubles as a way to un-hide the banner.
        if ([HZBannerAdController sharedInstance].bannerView) {
            [HZBannerAdController sharedInstance].bannerView.hidden = NO;
            return;
        }
        
        
        HZBannerPosition pos = HZBannerPositionBottom;
        NSString *positionStr = [NSString stringWithUTF8String: position];
        if ([positionStr isEqualToString: @"top"]) {
            pos = HZBannerPositionTop;
        }
        
        [[HZBannerAdController sharedInstance] placeBannerAtPosition:pos
                                                             options:nil
                                                             success:nil
                                                             failure:nil];
    }
    void hz_ads_hide_banner(void) {
        [HZBannerAdController sharedInstance].bannerView.hidden = YES;
    }
    
    void hz_ads_destroy_banner(void) {
        [[HZBannerAdController sharedInstance] destroyBanner];
    }
    
}
