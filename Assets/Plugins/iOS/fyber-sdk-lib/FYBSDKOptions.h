//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <Foundation/Foundation.h>

/**
 *  An options object to pass to the +[FyberSDK startWithOptions:] to configure the SDK
 */
@interface FYBSDKOptions : NSObject

/**
 *  Your application ID
 */
@property (nonatomic, copy) NSString *appId;

/**
 *  The ID of your user
 *
 *  @discussion If you don't keep track of user IDs you can use the constructor +[FYBSDKOptions optionsWithAppId:securityToken:] in order
 *              for the Fyber SDK to generate one automatically
 */
@property (nonatomic, copy) NSString *userId;

/**
 *  The security token you need to use our Virtual Currency servers
 */
@property (nonatomic, copy) NSString *securityToken;

/**
 *  If set to NO, the Fyber SDK as well as other Mediated Networks' SDKs will not start precaching videos when the Fyber SDK starts
 *
 *  Default is set to YES
 *
 *  @discussion Use the method -[FYBCacheManager startPrecaching] to start precaching videos
 *
 *  @see FYBCacheManager
 */
@property (nonatomic, assign) BOOL startVideoPrecaching;

/**
 *  Constructor
 *
 *  @param appId         Your application ID
 *  @param userId        Your user ID
 *  @param securityToken Your security Token
 *
 *  @return An instance of FYBSDKOptions configured with an appId, a userId and a securityToken
 */
+ (FYBSDKOptions *)optionsWithAppId:(NSString *)appId userId:(NSString *)userId securityToken:(NSString *)securityToken;

/**
 *  Constructor
 *
 *  @param appId         Your application ID
 *  @param securityToken Your security Token
 *
 *  @return An instance of FYBSDKOptions configured with an appId, a automatically generated userId and a securityToken
 */
+ (FYBSDKOptions *)optionsWithAppId:(NSString *)appId securityToken:(NSString *)securityToken;

@end
