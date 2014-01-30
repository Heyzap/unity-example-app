//
//  Achievement.h
//  Heyzap
//
//  Created by Maximilian Tagher on 12/7/12.
//
//

#import <Foundation/Foundation.h>

/** Model class representing an achievement on Heyzap, `HZAchievement`s are provided in the achievement-related methods of `HeyzapSDK`. You would only need to use this class if you wanted to create a custom UI for showing achievements. */
@interface HZAchievement : NSObject

/** The name of the achievement. */
@property (nonatomic, strong, readonly) NSString *title;

/** The description of the achievement. */
@property (nonatomic, strong, readonly) NSString *subtitle;

/** The image URL for the achievement icon, provided as an `NSString`. If the user has not unlocked the achievement, this URL will point to the grayscale version of the achievement icon. */
@property (nonatomic, strong, readonly) NSString *imageURLString;

/** The image URL for the achievement icon, provided as an `NSURL`. If the user has not unlocked the achievement, this URL will point to the grayscale version of the achievement icon. */
@property (nonatomic, strong, readonly) NSURL *imageURL;

/** Whether or not the user has unlocked the achievement. */
@property (nonatomic, readonly) BOOL unlocked;

/** If the user has just unlocked the achievement. */
@property (nonatomic, readonly) BOOL isNew;

// Private
- (HZAchievement *)initWithDictionary:(NSDictionary *)dictionary;

@end
