//
//  HZOfferWallShowOptions.h
//  Heyzap
//
//  Created by Monroe Ekilah on 7/3/17.
//  Copyright © 2017 Heyzap. All rights reserved.
//

/*
 * Copyright (c) 2017, Heyzap, Inc.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:
 *
 * * Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * * Redistributions in binary form must reproduce the above copyright
 *   notice, this list of conditions and the following disclaimer in the
 *   documentation and/or other materials provided with the distribution.
 *
 * * Neither the name of 'Heyzap, Inc.' nor the names of its contributors
 *   may be used to endorse or promote products derived from this software
 *   without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
 * TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#import "HeyzapAds.h"
#import "HZShowOptions.h"

@interface HZOfferWallShowOptions : HZShowOptions

/**
 *  Whether or not you would like the OfferWall ad to automatically close itself after the user interacts with their first offer.
    
    Defaults to `NO`.
 */
@property (nonatomic) BOOL shouldCloseAfterFirstClick;

@property (nonatomic) BOOL animatePresentation;

/**
 *  A dictionary of custom parameters are added to the request when showing the OfferWall. These custom parameters are used for server-side rewarded callbacks.
 *
 *  @discussion Setting this will clear all previously custom parameters; it won't combine old dictionaries with this one.
 
    @discussion Parameters must be named [`pub0`, `pub1`,..., `pub9`] to be sent through to your currency server callback.
 */
@property (nonatomic, strong, nullable) NSDictionary *customParameters;

@end
