//
//  HZUser.h
//  Heyzap
//
//  Created by Daniel Rhodes on 4/3/13.
//
//

#import <Foundation/Foundation.h>

@interface HZUser : NSObject

@property (nonatomic, strong) NSString *username;

@property (nonatomic, strong) NSString *picture;

- (id) initWithDictionary: (NSDictionary *) dict;

+ (void) loginWithUsername: (NSString *) username andPassword: (NSString *)password withCompletion:(void (^)(NSDictionary *data, NSError *error))completionBlock;

+ (void) logout;

@end
