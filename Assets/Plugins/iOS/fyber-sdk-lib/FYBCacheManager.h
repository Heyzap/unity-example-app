//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import "FYBCacheManagerDelegate.h"

/**
 *  Provides methods to control the precaching flow on the FyberSDK and on the mediated networks
 */
@interface FYBCacheManager : NSObject

/**
 *  The object that acts as the delegate of the cache manager
 *
 *  @discussion The delegate must adopt the FYBCacheManagerDelegate protocol. The delegate is not retained
 *
 *  @see FYBCacheManagerDelegate
 */
@property (nonatomic, weak) id<FYBCacheManagerDelegate> delegate;

/**
 *  Starts the video cache download operations
 *
 *  @discussion This method is only useful if you prevented the SDK from starting precaching videos by passing a FYBSDKOptions object
 *              configured with startVideoPrecaching = NO
 *
 *  @discussion Use -pausePrecaching to pause the downloads
 */
- (void)startPrecaching;

/**
 *  Pauses the video cache download operations
 *
 *  @discussion Use -resumePrecaching to reschedule the downloads
 *
 *  @discussion If they provide this feature, this will also pause the downloads triggered by third party networks
 */
- (void)pausePrecaching;

/**
 *  Resumes the downloads for rewarded video caching
 *
 *  @discussion Use -pausePrecaching to pause the downloads
 *
 *  @discussion If they provide this feature, this will also resume the downloads triggered by third party networks
 */
- (void)resumePrecaching;

/**
 *  Checks whether videos are available in the cache.
 *
 *  @return YES if at least one video is present in cache, otherwise NO.
 */
- (BOOL)hasCachedVideos;

/**
 *  Please use [FyberSDK cacheManager] instead
 */
- (instancetype)init __attribute__((unavailable("not available, use [FyberSDK cacheManager] instead")));

@end
