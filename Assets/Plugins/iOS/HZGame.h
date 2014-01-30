//
//  HZGame.h
//  Heyzap
//
//  Created by Daniel Rhodes on 4/3/13.
//
//

#import <Foundation/Foundation.h>

@interface HZGame : NSObject

+ (void) checkinWithCompletion: (void(^)(NSDictionary *data, NSError* error))completionBlock;

@end
