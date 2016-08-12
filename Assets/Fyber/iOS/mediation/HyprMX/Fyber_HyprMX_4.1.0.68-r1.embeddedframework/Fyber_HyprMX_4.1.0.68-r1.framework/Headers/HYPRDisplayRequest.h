//
//  HYPRDisplayRequest.h
//  HyprMX-Framework
//
//  Created on 3/23/12.
//  Copyright (c) 2012 HyprMX Mobile LLC. All rights reserved.
//
#import <Foundation/Foundation.h>
/**
 HYPRDisplayRequest is an opaque handle to an offer-fetching and offer-displaying session.
 See HYPRManager for more information.
 */
@interface HYPRDisplayRequest : NSObject

/**
 Causes the appropriate thing to be displayed, whether it is an offer, a list of offers, a required information view, etc.
 This is valid only for certain offer responses.
 */
- (void)display;

@end
