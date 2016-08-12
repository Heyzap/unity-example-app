//
//  HYPROfferDelegate.h
//  HyprMX
//
//  Created on 2/29/12.
//  Copyright (c) 2012 HyprMX Mobile LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

@class HYPROffer;

/** 
 * Delegate protocol for handling offer events (eg. Pausing/resuming game when video is displayed) 
 */
@protocol HYPROfferDelegate <NSObject>

/** 
 * Delegate callback notifying the delegate that an offer did complete. 
 *
 * At this point, if you set a hyprTransactionID in willDisplayOffer, this is where you verify it.
 * 
 * @param offer     Instance of class HYPROffer that did complete
 */
- (void)didCompleteOffer:(HYPROffer *)offer;

/** 
 * Delegate callback notifying the delegate that an offer did cancel.
 *
 * @param offer     Instance of class HYPROffer that did cancel
 */
- (void)didCancelOffer:(HYPROffer *)offer;

@optional

/** 
 * Delegate callback notifying the delegate that an offer will display.
 *
 * This is your opportunity to set a hyprTransactionID on the offer.
 *
 * @param offer     Instance of class HYPROffer that will display
 */
- (void)willDisplayOffer:(HYPROffer *)offer;

/** 
 * Delegate callback notifying the delegate that an offer did display.
 *
 * @param offer     Instance of class HYPROffer that did display
 */
- (void)didDisplayOffer:(HYPROffer *)offer;

/** 
 * Delegate callback that returns rewards specific to this offer.
 *
 * If you return nil, this will be ignored. To return no rewards, return an empty array.
 */
- (NSArray*)rewardsForOffer:(HYPROffer*)offer;
@end