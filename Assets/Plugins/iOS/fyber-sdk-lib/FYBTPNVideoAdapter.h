//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import "FYBTPNMediationTypes.h"
#import "FYBBaseNetwork.h"

@protocol FYBRewardedVideoNetworkAdapterDelegate;
@protocol FYBTPNVideoAdapter<NSObject>

- (void)setNetwork:(FYBBaseNetwork *)network;
- (NSString *)networkName;
- (void)videosAvailable:(FYBTPNValidationResultBlock)callback;
- (void)playVideoWithParentViewController:(UIViewController *)parentVC notifyingCallback:(FYBTPNVideoEventsHandlerBlock)eventsCallback;
- (BOOL)startAdapterWithDictionary:(NSDictionary *)dict;

@end

typedef NS_ENUM(NSInteger, FYBTPNProviderPlayingState) {
    FYBTPNProviderPlayingStateNotPlaying,
    FYBTPNProviderPlayingStateWaitingForPlayStart,
    FYBTPNProviderPlayingStatePlaying
};
