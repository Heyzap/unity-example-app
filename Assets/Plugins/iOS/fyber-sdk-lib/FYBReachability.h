//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

/*

 File: Reachability.h
 Abstract: Basic demonstration of how to use the SystemConfiguration Reachablity APIs

 Version: 2.2

 Disclaimer: IMPORTANT:  This Apple software is supplied to you by Apple Inc.
 ("Apple") in consideration of your agreement to the following terms, and your
 use, installation, modification or redistribution of this Apple software
 constitutes acceptance of these terms.  If you do not agree with these terms,
 please do not use, install, modify or redistribute this Apple software.

 In consideration of your agreement to abide by the following terms, and subject
 to these terms, Apple grants you a personal, non-exclusive license, under
 Apple's copyrights in this original Apple software (the "Apple Software"), to
 use, reproduce, modify and redistribute the Apple Software, with or without
 modifications, in source and/or binary forms; provided that if you redistribute
 the Apple Software in its entirety and without modifications, you must retain
 this notice and the following text and disclaimers in all such redistributions
 of the Apple Software.
 Neither the name, trademarks, service marks or logos of Apple Inc. may be used
 to endorse or promote products derived from the Apple Software without specific
 prior written permission from Apple.  Except as expressly stated in this notice,
 no other rights or licenses, express or implied, are granted by Apple herein,
 including but not limited to any patent rights that may be infringed by your
 derivative works or by other works in which the Apple Software may be
 incorporated.

 The Apple Software is provided by Apple on an "AS IS" basis.  APPLE MAKES NO
 WARRANTIES, EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION THE IMPLIED
 WARRANTIES OF NON-INFRINGEMENT, MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 PURPOSE, REGARDING THE APPLE SOFTWARE OR ITS USE AND OPERATION ALONE OR IN
 COMBINATION WITH YOUR PRODUCTS.

 IN NO EVENT SHALL APPLE BE LIABLE FOR ANY FYBECIAL, INDIRECT, INCIDENTAL OR
 CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
 GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 ARISING IN ANY WAY OUT OF THE USE, REPRODUCTION, MODIFICATION AND/OR
 DISTRIBUTION OF THE APPLE SOFTWARE, HOWEVER CAUSED AND WHETHER UNDER THEORY OF
 CONTRACT, TORT (INCLUDING NEGLIGENCE), STRICT LIABILITY OR OTHERWISE, EVEN IF
 APPLE HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

 Copyright (C) 2010 Apple Inc. All Rights Reserved.

*/

#import <Foundation/Foundation.h>
#import <SystemConfiguration/SystemConfiguration.h>

typedef NS_ENUM(NSInteger, FYBNetworkStatus) {
    FYBNetworkStatusNotReachable,
    FYBNetworkStatusReachableViaWiFi,
    FYBNetworkStatusReachableViaWWAN
};

//
// TODO remove for 7.0.0 - keep FYBNotReachable for backward compatibility with Adapters 2.0
typedef NS_ENUM(NSInteger, FYBNetworkStatus20) {
    FYBNotReachable
};


extern NSString *const FYBReachabilityChangedNotification;

@interface FYBReachability : NSObject

@property (nonatomic, assign) BOOL localWiFiRef;
@property (nonatomic, assign) SCNetworkReachabilityRef reachabilityRef;

//isSystemProxyEnabled - use to check if system proxy is enabled in system preferences
+ (BOOL)isSystemProxyEnabled;

//reachabilityWithHostName- Use to check the reachability of a particular host name
+ (FYBReachability *)reachabilityWithHostName:(NSString *)hostName;

/**
* Checks the internet connection to a default hostname
*/
+ (FYBReachability *)reachabilityForInternetConnection;



//Start listening for reachability notifications on the current run loop
- (BOOL)startNotifier;
- (void)stopNotifier;

- (FYBNetworkStatus)currentReachabilityStatus;
//WWAN may be available, but not active until a connection has been established
//WiFi may require a connection for VPN on Demand
- (BOOL)connectionRequired;

/**
 * Determines if the network can be reached
 *
 * @return YES if there's network connection. NO otherwise
 */
- (BOOL)isNetworkReachable;
@end
