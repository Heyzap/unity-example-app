//
//  HYPRErrorType.h
//  HyprMX
//
//  Created on 3/1/12.
//  Copyright (c) 2012 HyprMX Mobile LLC. All rights reserved.
//

typedef enum {
    HYPRErrorTypeSystemError = 0,
    HYPRErrorTypeNotConnected,
    HYPRErrorTypeFailureToLoad,
    HYPRErrorTypeFailureToLoadImage,
    HYPRErrorTypeMissingImageParameterError,
    HYPRErrorTypeUserOptedOutOfSplashView,
    HYPRErrorTypeControlNotAvailable,
    HYPRErrorTypeVastAssetDownloadError,
    HYPRErrorTypeVastPreloadResponseError,
    HYPRErrorTypeVastDownloadingVastTagError,
    HYPRErrorTypeVastParsingVastTagError,
    HYPRErrorTypeVastCachingVastTagError,
    HYPRErrorTypeVastCachingAssetError,
    HYPRErrorTypeVastPlayerError,
    HYPRErrorTypeUnspecified,
    HYPRErrorTypeLocationNotFound,
    HYPRErrorTypeLoadAppStore
} HYPRErrorType;

//
// static list of error messages for types
//
static NSString * const HYPRErrorMessages[] = {
    [HYPRErrorTypeNotConnected] = @"Device is not connected to the internet",
    [HYPRErrorTypeMissingImageParameterError] = @"Image URL is required to download an image.",
    [HYPRErrorTypeSystemError] = @"NSError converted to a HYPRError",
    [HYPRErrorTypeFailureToLoad] = @"Could not load from the API",
    [HYPRErrorTypeFailureToLoadImage] = @"Could not load image from the specified URL",
    [HYPRErrorTypeUserOptedOutOfSplashView] = @"User has opted out of splashscreens.",
    [HYPRErrorTypeControlNotAvailable] = @"Control being requested is not available",
    [HYPRErrorTypeVastAssetDownloadError] = @"Error while downloading VAST asset",
    [HYPRErrorTypeVastPreloadResponseError] = @"Error while reading preload response",
    [HYPRErrorTypeVastDownloadingVastTagError] = @"Error while downloading VAST tag",
    [HYPRErrorTypeVastParsingVastTagError] = @"Error while parsing VAST tag",
    [HYPRErrorTypeVastCachingVastTagError] = @"Error while caching VAST tag",
    [HYPRErrorTypeVastCachingAssetError] = @"Error while caching VAST asset",
    [HYPRErrorTypeVastPlayerError] = @"Error occurred while playing a VAST video",
    [HYPRErrorTypeUnspecified] = @"Unspecified Error Type",
    [HYPRErrorTypeLocationNotFound] = @"Location could not be identified",
    [HYPRErrorTypeLoadAppStore] = @"Error while load app store"
};