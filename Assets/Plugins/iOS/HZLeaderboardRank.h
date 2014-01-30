//
//  HZLeaderboardRank.h
//
//  Copyright 2012 Smart Balloon, Inc. All Rights Reserved
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

#import <Foundation/Foundation.h>

@class HZLeaderboardLevel;
@class HZScore;

/** `HZLeaderboardRank` is a model object describing a rank on a Heyzap Leaderboard. You receive these objects from network requests to Heyzap. You shouldn't need to ever interact with `HZLeaderboardRank`s, unless you wish to create a custom leaderboard UI different from Heyzap's. */
@interface HZLeaderboardRank : NSObject <NSCoding>

/** The best score that the player has achieved */
@property (nonatomic, strong, readonly) HZScore *bestScore;

/** The score last submitted */
@property (nonatomic, strong, readonly) HZScore *currentScore;

/** The level with which the ranking is associated */
@property (nonatomic, strong, readonly) HZLeaderboardLevel *level;

/** The URL for the player's picture */
@property (nonatomic, strong, readonly) NSURL *userPicture;

/** If the last score submitted was a personal best */
@property (nonatomic, readonly) BOOL currentIsPersonalBest;

/** If the person is currently logged in to Heyzap */
@property (nonatomic, readonly) BOOL loggedIn;

@end
