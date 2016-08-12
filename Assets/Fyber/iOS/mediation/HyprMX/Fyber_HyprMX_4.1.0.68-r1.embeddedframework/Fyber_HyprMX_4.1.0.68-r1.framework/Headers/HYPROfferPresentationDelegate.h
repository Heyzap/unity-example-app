//
//  HYPROfferPresentationDelegate.h
//  HyprMX-Framework
//
//  Copyright (c) 2012 HyprMX Mobile LLC. All rights reserved.
//

#import "HYPROfferDelegate.h"
#import <UIKit/UIKit.h>

@class HYPRDisplayRequest;

@protocol HYPROfferPresentationDelegate <HYPROfferDelegate>

- (void)displayRequestCanDisplayRequiredInformation:(HYPRDisplayRequest *)displayRequest;
- (void)displayRequestCanDisplayOffers:(HYPRDisplayRequest *)displayRequest;
- (void)displayRequest:(HYPRDisplayRequest *)displayRequest canDisplayOffer:(HYPROffer *)offer;

- (void)displayRequestCannotDisplay:(HYPRDisplayRequest *)displayRequest;

- (void)displayRequestDidFinish:(HYPRDisplayRequest *)displayRequest;

@optional

/*
 Whether the display step should be presented in a popover on iPad, as opposed to a modal form sheet.
 If you don't implement this, the assumption is NO.
 If you return YES, you must also implement -displayRequest:willPresentPopoverFromRect:inView:.
 */
- (BOOL)displayRequestShouldPresentInPopover:(HYPRDisplayRequest *)displayRequest;

/*
 When the offer system will present a popover, this method will be called.
 You should set *rect and *view to supply the rect from which the popover presents.
 You must implement this on iPad if you return YES from -displayRequestShouldPresentInPopover:.
 */
- (void)displayRequest:(HYPRDisplayRequest *)displayRequest willPresentPopoverFromRect:(out CGRect *)rect inView:(out UIView **)view;

@end
