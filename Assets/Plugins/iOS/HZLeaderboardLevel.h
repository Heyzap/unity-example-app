//
//  HZLeaderboardLevel.h
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
//

#import <Foundation/Foundation.h>
/** `HZLeaderboardLevel` is the model object describing a level. You shouldn't need to interact with these objects directly; they are returned by Heyzap network requests to be used there.
 
 You can create Leaderboards and check level IDs at https://developers.heyzap.com/dashboard*/
@interface HZLeaderboardLevel : NSObject <NSCoding>


/** The same level ID you would find at https://developers.heyzap.com/dashboard */
@property (nonatomic, strong, readonly) NSString *levelID;

/** The name of the level */
@property (nonatomic, strong, readonly) NSString *name;

/** What direction the scores are sorted in in the leaderboard. A golf game might sort based on the fewest strokes (lowest first), whereas a game sorting on points would sort highest first. You can configure this value at https://developers.heyzap.com/dashboard */
@property (nonatomic, readonly) BOOL lowestScoreFirst;

@end
