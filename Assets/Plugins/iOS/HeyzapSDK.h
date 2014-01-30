/*
 * Copyright (c) 2014, Smart Balloon, Inc.
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
 * * Neither the name of 'Smart Balloon, Inc.' nor the names of its contributors
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
#import <UIKit/UIKit.h>
#import "HZLog.h"

#ifndef NS_ENUM
#define NS_ENUM(_type, _name) enum _name : _type _name; enum _name : _type
#endif

typedef NS_ENUM(NSUInteger, HZOptions) {
    HZOptionsNone    = 0,
    HZOptionsHideStartScreen   = 1 << 1, // DEPRECATED: Heyzap no longer shows any popup at app launch time.
    HZOptionsHideDeleteScore   = 1 << 2, // DEPRECATED; has no effect.
    HZOptionsShowErrors        = 1 << 3, // Equivalent to setting the logging behavior to `HZDebugLevelError`
};

@protocol HeyzapAchievementProtocol <NSObject>
- (NSString *)heyzapAchievementIdentifier;
@end

@class HZCheckinButton;
@class HZScore;
@class HZLeaderboardRank;

/** `HeyzapSDK` is a singleton object which handles all the functionality of the Heyzap SDK. */
@interface HeyzapSDK : NSObject

/**---------------------------------------------------------------------------------------
 * @name Initialization
 *  ---------------------------------------------------------------------------------------
 */

/** Accessing the singleton `HeyzapSDK` */
+ (id) sharedSDK;

/** The App Store ID for your app. Instructions for getting this value: http://support.heyzap.com/entries/23201783-How-do-I-find-my-game-s-App-Store-ID- . If you haven't submitted your app to the store, see here: http://developers.heyzap.com/docs/ios_sdk_advanced*/
@property (nonatomic, strong) NSString *appId;
/** The URL the Heyzap SDK uses to return to your app. For information, see here: http://developers.heyzap.com/docs/ios_sdk_advanced*/
@property (nonatomic, strong) NSURL *appURL;

/** Whether or not Heyzap supports the current version of iOS. Heyzap will run fine on iOS versions below 5.0, but will not show Leaderboards, Achievements, or Ads on those versions.
 @return `YES` if supported
 */
+ (BOOL) isSupported;

#pragma mark - Initialization

+ (void) setAppName: (NSString *) passedAppName;

/** Initialize the Heyzap SDK.
 @param appId The App Store ID used in your app. See the `appId` property for details.
 */
+ (void)startHeyzapWithAppId:(NSString *)appId;

/** Initialize the Heyzap SDK.
 @param appId The App Store ID used in your app. See the `appId` property for details.
 @param options A bitmask of options; see the `HZOptions` enum for details.
 */
+ (void)startHeyzapWithAppId:(NSString *)appId andOptions:(int) options;

/** Initialize the Heyzap SDK.
 @param appId The App Store ID used in your app. See the `appId` property for details.
 @param url The 
 @param options A bitmask of options; see the `HZOptions` enum for details.
 */
+ (void)startHeyzapWithAppId:(NSString *)appId andAppURL:(NSURL *)url andOptions:(int)options;

#pragma mark - Debug
/** For development purposes, the Heyzap SDK will print helpful information. This method controls how much is logged. The default level is `HZDebugLevelSilent`.
 @param debugLevel The `HZDebugLevel` to use for print statements.
 */
- (void)setDebugLevel: (HZDebugLevel) debugLevel;

#pragma mark - Level
- (void) onStartLevel:(void (^)(NSString *))block;

#pragma mark - Leaderboard/Score Methods
/**---------------------------------------------------------------------------------------
 * @name Leaderboard/Score Methods
 *  ---------------------------------------------------------------------------------------
 */

/** Submit a new score to Heyzap. See the `HZScore` header for how to create an `HZScore`. If the score isn't a personal best, this method will show a small notification at the top of the screen. If the score is a personal best, shows a full leaderboard.
 
 @param completionBlock A block with following arguments:
   rank The `HZLeaderboardRank` describing the user's position on the leaderboard. You do not need to use this object.
   error If there was an error submitting the score, `error` describes the issue. Otherwise, `nil`.
 */
- (IBAction) submitScore:(HZScore *)score withCompletion: (void(^)(HZLeaderboardRank *rank, NSError *error))completionBlock;

/** Shows the leaderboard for the specified level ID
 @param levelID The string for the level ID. You can find these values at https://developers.heyzap.com/dashboard
 */
- (IBAction) openLeaderboardLevel: (NSString *) levelID;

/** Shows the leaderboard for the default level ID */
- (IBAction) openLeaderboard;

/** Deletes all the scores for the current user for the given level. This is primarily intended to enable deleting test data. */
- (void) deleteCurrentUserScoresForLevel:(NSString *)levelID;

#pragma mark - Parsing URLs coming from Heyzap
extern NSString * const kHeyzapRequestTypeKey;
extern NSString * const kHeyzapRequestArgumentsKey;

+ (BOOL)canParseURL:(NSURL *)url;
+ (NSDictionary *)parseURL:(NSURL *)url;

/**---------------------------------------------------------------------------------------
 * @name Achievements
 *  ---------------------------------------------------------------------------------------
 */

#pragma mark - Achievements

/** Unlocks achievements and then displays a popup showing the achievements the user has unlocked. This method will only display UI if there are new achievements to show. Because this method displays UI over the screen, call it at e.g. the end of a level. To unlock achievements without interrupting gameplay, see `silentlyUnlockAchievements:`
 
 @param achievementIDs An array of achievement ID strings.
 As a convenience, if you internally represent achievements as objects within your application, you can pass those objects. Just have them conform to the `HeyzapAchievementProtocol`. You can create achievements and find their achievement identifier at https://developers.heyzap.com/dashboard
 @param block A completion block with the following arguments:
 
   `achievements` An `NSArray` of `HZAchievement` model objects. You do not need to use these objects unless you wish to disable Heyzap's Achievement UI and create your own.
 
   `error` If the request sent to Heyzap was a success, this value will be `nil`. Otherwise, it will contain an `NSError` object describing the error.
 
   `showPopup` A reference to a Boolean value. The block can set this value to `NO` to not show the Achievement popup (if for example the user has already started a new game).
 */
- (void)unlockAchievementsWithIDs:(NSArray *)achievementIDs completion:(void(^)(NSArray *achievements, NSError *error, BOOL *showPopup))block;

/** Stores achievement IDs in `NSUserDefaults` that should be unlocked on the next call of `unlockAchievementsWithIDs:completion:` without adding UI to the screen. Use this method to unlock an achievement without interrupting gameplay.
 
 @param achievementIDs This parameter has the same semantics as in the `unlockAchievementsWithIDs:completion:`. It is safe to call this method multiple times with the same achievement and to pass duplicate achievements -- the UI displayed by `unlockAchievementsWithIDs:completion:` will always be accurate at displaying what achievements are new.
 */
- (void)silentlyUnlockAchievements:(NSArray *)achievementIDs;

/** Shows all achievements for the game.
 @param block A completion block called after finishing a Heyzap network request. The block arguments are identical to those used in `unlockAchievementsWithIDs:completion:`.
 */
- (void)showAllAchievementsWithCompletion:(void(^)(NSArray *achievements, NSError *error, BOOL *showPopup))block;

/** Deletes all the achievements for the current user. This is primarily intended to enable deleting test data.  */
- (void)deleteAllAchievementsForCurrentUser;


/**---------------------------------------------------------------------------------------
 * @name Deprecated Methods
 *  ---------------------------------------------------------------------------------------
 */

#pragma mark - Deprecated methods

// Heyzap no longer shows any popup at launch.
// Use startHeyzapWithAppId:andOptions: or startHeyzapWithAppId:andAppURL:andOptions: instead

+ (void) startHeyzapWithAppId: (NSString *) appId andAppURL: (NSURL *) url andShowPopup:(BOOL)showPopup DEPRECATED_ATTRIBUTE;
+ (void) startHeyzapWithAppId:(NSString *) appId andShowPopup:(BOOL)showPopup DEPRECATED_ATTRIBUTE;

// Heyzap no longer allows checking in from the SDK
// This is because Apple does not allow an app to encourage downloading other apps (Guideline 3.1.0)

/** Returns `nil`. */
+ (HZCheckinButton*) getCheckinButtonWithLocation: (CGPoint) location DEPRECATED_ATTRIBUTE;
/** Returns `nil`. */
+ (HZCheckinButton*) getCheckinButtonWithLocation: (CGPoint) location andMessage: (NSString *) message DEPRECATED_ATTRIBUTE;

// These methods do nothing.
- (IBAction) checkin DEPRECATED_ATTRIBUTE;
- (IBAction) checkinWithMessage: (NSString *) message DEPRECATED_ATTRIBUTE;

@end
