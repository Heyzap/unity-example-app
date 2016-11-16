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

- (id)initWithKlassName:(NSString *)klassName;
- (void)sendMessageForKlass:(NSString *)klass withMessage:(NSString *)message andTag:(NSString *)tag;

@end

@implementation HeyzapUnityAdDelegate

- (id) initWithKlassName:(NSString *) klassName {
    self = [super init];
    if (self) {
        _klassName = klassName;
    }
    
    return self;
}

- (void)didReceiveAdWithTag:(NSString *)tag { [self sendMessageForKlass:self.klassName withMessage:@"available" andTag:tag]; }

- (void)didFailToReceiveAdWithTag:(NSString *)tag { [self sendMessageForKlass:self.klassName withMessage:@"fetch_failed" andTag:tag]; }

- (void)didShowAdWithTag:(NSString *)tag { [self sendMessageForKlass:self.klassName withMessage:@"show" andTag:tag]; }

- (void)didHideAdWithTag:(NSString *)tag { [self sendMessageForKlass:self.klassName withMessage:@"hide" andTag:tag]; }

- (void)didFailToShowAdWithTag:(NSString *)tag andError:(NSError *)error { [self sendMessageForKlass:self.klassName withMessage:@"failed" andTag:tag]; }

- (void)didCompleteAdWithTag:(NSString *)tag { [self sendMessageForKlass:self.klassName withMessage:@"incentivized_result_complete" andTag:tag]; }

- (void)didFailToCompleteAdWithTag:(NSString *)tag { [self sendMessageForKlass:self.klassName withMessage:@"incentivized_result_incomplete" andTag:tag]; }

- (void)willStartAudio { [self sendMessageForKlass:self.klassName withMessage:@"audio_starting" andTag:@""]; }

- (void)didFinishAudio { [self sendMessageForKlass:self.klassName withMessage:@"audio_finished" andTag:@""]; }

- (void)bannerDidReceiveAd:(HZBannerAdController *)banner {
    [self sendMessageForKlass:self.klassName withMessage:@"loaded" andTag:@""];
}

- (void)bannerDidFailToReceiveAd:(HZBannerAdController *)banner error:(NSError *)error {
    [self sendMessageForKlass:self.klassName withMessage:@"error" andTag:@""];
}

- (void)bannerWasClicked:(HZBannerAdController *)banner {
    [self sendMessageForKlass:self.klassName withMessage:@"click" andTag:@""];
}

- (void)bannerWillLeaveApplication:(HZBannerAdController *)banner {
    [self sendMessageForKlass:self.klassName withMessage:@"leave_application" andTag:@""];
}

- (void)sendMessageForKlass:(NSString *)klass withMessage:(NSString *)message andTag:(NSString *)tag {
    UnitySendMessage([klass UTF8String], "SetCallback", [message UTF8String]);
}

@end

static HeyzapUnityAdDelegate *HZInterstitialDelegate = nil;
static HeyzapUnityAdDelegate *HZIncentivizedDelegate = nil;
static HeyzapUnityAdDelegate *HZBannerDelegate = nil;

extern "C" {
    void hz_ads_start_app(const char *publisher_id, HZAdOptions flags) {
        static dispatch_once_t onceToken;
        dispatch_once(&onceToken, ^{
            
            [HeyzapAds startWithPublisherID:[NSString stringWithUTF8String:publisher_id] andOptions:flags andFramework:HZ_FRAMEWORK_NAME];
            
            HZIncentivizedDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName:HZ_INCENTIVIZED_KLASS];
            [HZIncentivizedAd setDelegate:HZIncentivizedDelegate];
            
            HZInterstitialDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName:HZ_INTERSTITIAL_KLASS];
            [HZInterstitialAd setDelegate:HZInterstitialDelegate];
            
            HZBannerDelegate = [[HeyzapUnityAdDelegate alloc] initWithKlassName:HZ_BANNER_KLASS];
            [[HZBannerAdController sharedInstance] setDelegate:HZBannerDelegate];
        });
    }
    
    void hz_ads_show_interstitial(const char *tag) {
        [HZInterstitialAd showForTag:[NSString stringWithUTF8String:tag]];
    }
    
    void hz_ads_fetch_interstitial(void) {
        [HZInterstitialAd fetch];
    }
    
    bool hz_ads_interstitial_is_available(void) {
        return [HZInterstitialAd isAvailable];
    }
    
    void hz_ads_show_incentivized(const char *tag) {
        [HZIncentivizedAd showForTag:[NSString stringWithUTF8String:tag]];
    }
    
    void hz_ads_fetch_incentivized(void) {
        [HZIncentivizedAd fetch];
    }
    
    bool hz_ads_incentivized_is_available(void) {
        return [HZIncentivizedAd isAvailable];
    }
    
    void hz_ads_show_banner(const char *position, const char *tag, const char *admobSizeChar, const char *facebookSizeChar, const char *inmobiSizeChar) {
        // After the banner is loaded initially, this function doubles as a way to un-hide the banner.
        if ([HZBannerAdController sharedInstance].bannerView) {
            [HZBannerAdController sharedInstance].bannerView.hidden = NO;
            return;
        }
        
        
        HZBannerPosition pos = HZBannerPositionBottom;
        NSString *positionStr = [NSString stringWithUTF8String:position];
        if ([positionStr isEqualToString:@"top"]) {
            pos = HZBannerPositionTop;
        }


        HZBannerAdOptions *options = [[HZBannerAdOptions alloc] init];
        options.tag = [NSString stringWithUTF8String:tag];

        NSString *admobSize = [NSString stringWithUTF8String:admobSizeChar];
        NSString *facebookSize = [NSString stringWithUTF8String:facebookSizeChar];
        NSString *inmobiSize = [NSString stringWithUTF8String:inmobiSizeChar];

        if ([admobSize isEqualToString:@"SMART_BANNER"]) {
            options.admobBannerSize = HZAdMobBannerSizeFlexibleWidthPortrait;
        } else if ([admobSize isEqualToString:@"SMART_BANNER_LANDSCAPE"]) {
            options.admobBannerSize = HZAdMobBannerSizeFlexibleWidthLandscape;
        } else if ([admobSize isEqualToString:@"BANNER"]) {
            options.admobBannerSize = HZAdMobBannerSizeBanner;
        } else if ([admobSize isEqualToString:@"LARGE_BANNER"] || [admobSize isEqualToString:@"MEDIUM_RECTANGLE"]) { // iOS doesn't support MEDIUM_RECTANGLE
            options.admobBannerSize = HZAdMobBannerSizeLargeBanner;
        } else if ([admobSize isEqualToString:@"LEADERBOARD"]) {
            options.admobBannerSize = HZAdMobBannerSizeLeaderboard;
        } else if ([admobSize isEqualToString:@"FULL_BANNER"] || [admobSize isEqualToString:@"WIDE_SKYSCRAPER"]) { // iOS doesn't support WIDE_SKYSCRAPER
            options.admobBannerSize = HZAdMobBannerSizeFullBanner;
        }
        
        if ([facebookSize isEqualToString:@"BANNER_320_50"]) {
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
            options.facebookBannerSize = HZFacebookBannerSize320x50;
#pragma clang diagnostic pop
        } else if ([facebookSize isEqualToString:@"BANNER_HEIGHT_50"]) {
            options.facebookBannerSize = HZFacebookBannerSizeFlexibleWidthHeight50;
        } else if ([facebookSize isEqualToString:@"BANNER_HEIGHT_90"] || [facebookSize isEqualToString:@"BANNER_RECTANGLE_250"]) { // iOS doesn't support BANNER_RECTANGLE_250
            options.facebookBannerSize = HZFacebookBannerSizeFlexibleWidthHeight90;
        }

        if ([inmobiSize isEqualToString:@"BANNER_320_50"]) {
            options.inmobiBannerSize = HZInmobiBannerSize320x50;
        } else if ([inmobiSize isEqualToString:@"BANNER_320_48"]) {
            options.inmobiBannerSize = HZInmobiBannerSize320x48;
        } else if ([inmobiSize isEqualToString:@"BANNER_300_250"]) {
            options.inmobiBannerSize = HZInmobiBannerSize300x250;
        } else if ([inmobiSize isEqualToString:@"BANNER_120_600"]) {
            options.inmobiBannerSize = HZInmobiBannerSize120x600;
        } else if ([inmobiSize isEqualToString:@"BANNER_468_60"]) {
            options.inmobiBannerSize = HZInmobiBannerSize468x60;
        } else if ([inmobiSize isEqualToString:@"BANNER_728_90"]) {
            options.inmobiBannerSize = HZInmobiBannerSize728x90;
        } else if ([inmobiSize isEqualToString:@"BANNER_1024_768"] ) {
            options.inmobiBannerSize = HZInmobiBannerSize1024x768;
        }
        
        [[HZBannerAdController sharedInstance] placeBannerAtPosition:pos
                                                             options:options
                                                             success:^(UIView *banner) {
                                                                 // delegate isn't notified by HZBannerAdController on initial fetch, but since Unity devs don't have access to this success callback, we send the didReceiveAd: callback.
                                                                 [HZBannerDelegate bannerDidReceiveAd:[HZBannerAdController sharedInstance]];
                                                             }
                                                             failure:^(NSError *error) {
                                                                 // the delegate should already be called by the HZBannerAdController
                                                                 NSLog(@"[ Heyzap Unity ] Error fetching banner: %@", error);
                                                             }
        ];
    }
    
    char * hz_ads_banner_dimensions(void) {
        UIView *bannerView = [HZBannerAdController sharedInstance].bannerView;
        if (bannerView) {
            CGFloat scale = [[UIScreen mainScreen] scale];
            NSString *dimensionsString = [NSString stringWithFormat:@"%f %f %f %f", bannerView.frame.origin.x * scale, bannerView.frame.origin.y * scale, bannerView.frame.size.width * scale, bannerView.frame.size.height * scale];
            const char * dims = [dimensionsString UTF8String];
            if (dims == NULL) {
                return NULL;
            }
            
            char* returnValue = (char*)malloc(strlen(dims) + 1);
            strcpy(returnValue, dims);
            return returnValue;
        } else {
            NSLog(@"Can't get banner dimensions, there is no banner ad currently loaded.");
            return NULL;
        }
    }
    
    void hz_ads_hide_banner(void) {
        [HZBannerAdController sharedInstance].bannerView.hidden = YES;
    }
    
    void hz_ads_destroy_banner(void) {
        [[HZBannerAdController sharedInstance] destroyBanner];
    }

    void hz_ads_show_mediation_debug_view_controller(void) {
        [HeyzapAds presentMediationDebugViewController];
    }
    
    void hz_ads_show_debug_logs(void) {
        [HZLog setDebugLevel:HZDebugLevelVerbose];
    }
    
    void hz_ads_hide_debug_logs(void) {
        [HZLog setDebugLevel:HZDebugLevelSilent];
    }
    
}
