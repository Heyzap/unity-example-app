//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

/**
 *  The delegate of a FYBCacheManager object must adopt the FYBCacheManagerDelegate protocol
 */
@protocol FYBCacheManagerDelegate <NSObject>

@optional

/**
 *  The cache manager finished precaching videos
 *
 *  @param videosAvailable Will be YES if at least one video is present in cache
 */
- (void)cacheManagerDidCompletePrecachingWithVideosAvailable:(BOOL)videosAvailable;

@end
