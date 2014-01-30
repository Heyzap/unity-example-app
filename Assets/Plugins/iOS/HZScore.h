//
//  HZScore.h
//
//  Copyright 2011 Smart Balloon, Inc. All Rights Reserved
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

#import <Foundation/Foundation.h>

/** `HZScore` is the model object for scores on Heyzap Leaderboards. You should use one Leaderboard per concept: a racing game might use one leaderboard per track, ranking based on times. A side scroller might do two leaderboards per level, one for most coins and another for best time.
 
 You can create Leaderboards and check level IDs at https://developers.heyzap.com/dashboard */
@interface HZScore : NSObject <NSCoding>

#pragma mark - Initialization
/**---------------------------------------------------------------------------------------
 * @name Initialization
 *  ---------------------------------------------------------------------------------------
 */

/** The initializer for `HZScore` objects. After creating the score, set the `displayScore` and `relativeScore` properties.
 @param levelID The string representing the level, which you can find at https://developers.heyzap.com/dashboard
 @return An `HZScore` object.
 */
- (id) initWithLevelID: (NSString *) levelID;


#pragma mark - Properties
/**---------------------------------------------------------------------------------------
 * @name Properties
 *  ---------------------------------------------------------------------------------------
 */

/** Display Score: The score seen by the user in the game and on the leaderboard
 e.g. "25 points", "15.43 seconds", "$25", etc. ***Required***
 */
@property (nonatomic, strong) NSString *displayScore;

/** A number used internally to rank the score against other player scores. A racing game might return the number of seconds the user completed the level in, for example. This value is not user-facing. ***Required*** */
@property (nonatomic) float relativeScore;

/** The ID of the level. You can find this value at https://developers.heyzap.com/dashboard ***Required*** */
@property (nonatomic, strong) NSString *levelID;


/** The username of the player in the game. This property helps Heyzap keep track of the correct scores and correctly match up players. (_Optional_) */
@property (nonatomic, strong) NSString *username;

/** The user's rank on the leaderboard. This value is only available when you are getting a score after a network request to Heyzap. */
@property (nonatomic, readonly) int rank;


/** Whether or not the `HZScore` object has been correctly configured.
 @return `YES` if valid, otherwise `NO`.
 */
- (BOOL) isValid;

@end
