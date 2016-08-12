//
//  HYPROffer.h
//  HyprMX
//
//  Created on 2/29/12.
//  Copyright (c) 2012 HyprMX Mobile LLC. All rights reserved.
//

#import <HyprMX/HYPROfferDelegate.h>

@class HYPRImage;

@protocol HYPROffer <NSObject>

@required

- (NSString*)offerId;
- (NSString*)offerType;

@end

/** 
 * Models a HyprMX Mobile Offer 
 */
@interface HYPROffer : NSObject <HYPROffer>

/** 
 * Identifier for the offer 
 */
@property (nonatomic, strong) NSString *identifier;

/** 
 * Title of the offer to be displayed to the user 
 */
@property (nonatomic, copy) NSString *title;

/** 
 * Description of the offer to be displayed to the user 
 */
@property (nonatomic, copy) NSString *offerDescription;

/** 
 * Reward Identifier for this offer 
 */
@property (nonatomic, strong) NSNumber *rewardIdentifier;

/** 
 * Reward Title for this offer 
 */
@property (nonatomic, copy) NSString *rewardText;

/** 
 * Quantity of the Reward for this offer 
 */
@property (nonatomic, retain) NSNumber *rewardQuantity;

/** 
 * Reward token. This is used to identify the reward when we open the offer view.
 */
@property (nonatomic, retain) NSString *rewardToken;

/** 
 * Reward Image URL for this offer. Will be used if the reward does not supply one. 
 */
@property (nonatomic, strong) HYPRImage *rewardImage;

/** 
 * Delegate conforming to protocol HYPROfferDelegate to notify for significant offer events. 
 */
@property (nonatomic, weak) id<HYPROfferDelegate> delegate;

/** 
 * Type of offer, typically "video" - combined with identifier to uniquely identify an offer when communicating with certain APIs.
 */
@property (nonatomic, copy) NSString *offerType;

/** 
 * Sets a Default Offer. If set, this offer will be displayed even when more than one are returned
 */
@property (nonatomic, assign, getter = isDefaultOffer) BOOL defaultOffer;

/** 
 * String uniquely identifying this offer. Provided by the SDK user and returned when the offer is completed. 
 */
@property (nonatomic, strong) NSString *hyprTransactionID;

/** 
 *Thank You URL, used by offers that don't start out in a web view, but still need to display the thank you screen. 
 */
@property (nonatomic, strong) NSString *thankYouUrl;


/* Notifies delegates of actions taken on the offer. */
- (void)willDisplay;
- (void)didDisplay;
- (void)didComplete;
- (void)didCancel;

@end