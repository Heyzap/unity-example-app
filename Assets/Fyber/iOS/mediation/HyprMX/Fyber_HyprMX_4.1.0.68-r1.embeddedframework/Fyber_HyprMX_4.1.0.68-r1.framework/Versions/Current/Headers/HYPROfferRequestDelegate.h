//
//  HYPROfferRequestDelegate.h
//  HyprMX
//
//  Created on 2/29/12.
//  Copyright (c) 2012 HyprMX Mobile LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

@class HYPRError, HYPROfferRequest;

/** 
 * Main protocol for managing the lifecycle for the class HYPROfferRequest 
 */
@protocol HYPROfferRequestDelegate <NSObject>

@required

/** 
 * Delegate callback method for a successful load offer request.
 *
 * @param request   Instance of successful class HYPROfferRequest
 * @param offers    Array of class HYPROffer offers
 */
- (void)request:(HYPROfferRequest *)request didLoadOffers:(NSArray *)offers;

/** 
 * Delegate callback method for a failed load offer request.
 *
 * @param request   Instance of failed class HYPROfferRequest
 * @param error     Instance of class HYPRError error describing the failure
 */
- (void)request:(HYPROfferRequest *)request didFailWithError:(HYPRError *)error;

@optional

/** 
 * Delegate callback method for when a request is canceled.
 *
 * @param request   Instance of cancelled class HYPROfferRequest
 */
- (void)requestWasCancelled:(HYPROfferRequest *)request;

/** 
 * Delegate callback method asking the delegate if the request should prompt the user for information.
 *
 * @param request   Instance of class HYPROfferRequest needing user information
 * @return          BOOL specifying if the request should prompt the user for information
 */
- (BOOL)requestShouldPromptUserForInfo:(HYPROfferRequest *)request;

/** 
 * Delegate callback method notifying the delegate that the request did prompt the user for information.
 *
 * @param request   Instance of class HYPROfferRequest that prompted user for information
 */
- (void)requestDidPromptUserForInfo:(HYPROfferRequest *)request;

@end