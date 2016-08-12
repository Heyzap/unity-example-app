//
//  HYPROfferRequest.h
//  HyprMX
//
//  Created on 2/29/12.
//  Copyright (c) 2012 HyprMX Mobile LLC. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <HyprMX/HYPROfferRequestDelegate.h>
#import <HyprMX/HYPRDisplayRequest.h>

@protocol HYPROfferDelegate;

/** 
 * Represents an offer request to load offers from the HyprMX Mobile API
 */
@interface HYPROfferRequest : NSObject

/** 
 * Delegate to the Offer Request.
 * 
 * Delegate conforms to protocol HYPROfferRequestDelegate.
 */
@property (nonatomic, weak) id<HYPROfferRequestDelegate> delegate;

/** 
 * userInfo Dictionary for this offer request
 */
@property (readonly) NSDictionary *userInfo;

@property (nonatomic, weak) id<HYPROfferDelegate> offerDelegate;

/** 
 * Helper to quickly create a class HYPROfferRequest with a delegate and user information.
 *
 * @param userInfo  Dictionary of user information to pass to the HyprMX Mobile API
 * @param delegate  Delegate to notify of significant request events
 *
 * @return id instance of a class HYPROfferRequest
 */
+ (id)requestForOfferWithUserInfo:(NSDictionary *)userInfo delegate:(id<HYPROfferRequestDelegate>)delegate;

/** 
 * Helper to quickly create a class HYPROfferRequest with user information
 *
 * @param userInfo  Dictionary of user information to pass to the HyprMX Mobile API
 *
 * @return id instance of a class HYPROfferRequest
 */
+ (id)requestForOfferWithUserInfo:(NSDictionary*)userInfo;

/** 
 * Helper to quickly create a class HYPROfferRequest
 *
 * @return id instance of a class HYPROfferRequest
 */
+ (id)request; 

/** 
 * Begin asynchronous communication with HyprMX Mobile API 
 */
- (void)send;

/** 
 * Cancel this request with HyprMX Mobile API 
 */
- (void)cancel;

@end