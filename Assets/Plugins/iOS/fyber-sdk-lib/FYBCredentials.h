//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

@import Foundation;

@interface FYBCredentials : NSObject<NSCopying>

@property (nonatomic, copy) NSString *appId;
@property (nonatomic, copy) NSString *userId;
@property (nonatomic, copy) NSString *securityToken;

+ (FYBCredentials *)credentialsWithAppId:(NSString *)appId userId:(NSString *)userId securityToken:(NSString *)securityToken;

/**
 *  Used to determine if a given credential belongs to a publisher of advertiser
 
 *  @return YES if the credential belong to an advertiser. NO otherwise
 */
- (BOOL)isAdvertiser;

@end
