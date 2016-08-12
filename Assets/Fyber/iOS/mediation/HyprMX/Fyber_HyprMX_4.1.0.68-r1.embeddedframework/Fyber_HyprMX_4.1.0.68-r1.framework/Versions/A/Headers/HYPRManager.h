//
//  HYPRManager.h
//  HyprMX
//
//  Created on 2/29/12.
//  Copyright (c) 2012 HyprMX Mobile LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

typedef enum {
    HYPRLogLevelError = 0, // Messages at this level get logged all the time.
    HYPRLogLevelVerbose,   //                               ... only when verbose logging is turned on.
    HYPRLogLevelDebug      //                               ... in debug mode.
} HYPRLogLevel;

@class HYPRError, HYPROffer, HYPRDisplayRequest, HYPRSettings;
@protocol HYPROfferPresentationDelegate;

/** Failure block type used for notifying callers of failed operations */
typedef void(^HYPRFailureBlock)(HYPRError *error);

/** Result block type used for notifying callers of boolean results */
typedef void (^HYPRBooleanResultCallback)(BOOL result);

/** Result block type used for notifying callers of boolean results */
typedef void (^HYPRDisplayOfferCallback)(BOOL completed, HYPROffer *offer);

/** 
 * Singleton providing app-wide configuration, expected to be initialized and configured in the application delegate
 */
@interface HYPRManager : NSObject

/** 
 * Library version.
 *
 * Obtain the current version of the HyprMX Library.
 */
@property (nonatomic, readonly) NSString *versionString;

/** 
 * Distributor Identifier.
 *
 * Generated Distributor ID for each application
 */
@property (nonatomic, strong) NSString *distributorId;

/** 
 * Distributor Property Identifier.
 *
 * ID/Name of a Distributor's Property (formerly Application) name
 */
@property (nonatomic, strong) NSString *distributorPropertyId;

/** 
 * User Identifier. 
 * Uniquely identifies the user.
 *
 * @discussion User ID must be unique across devices. It is recommended to use your Game Center ID
 * for the user or other uniquely identifying information.
 */
@property (nonatomic, strong) NSString *userId;

/** 
 * An array of HYPRReward objects.
 *
 * @discussion The rewards are sent to HyprMX. Only offers that can satisfy these rewards will be displayed.
 * Rewards specified here will apply to all offer requests that do not specify their own set of rewards.
 */
@property (nonatomic, copy) NSArray *rewards;

/** 
 * The HYPRSettings object associated with the HYPRManager.
 *
 *  @discussion The settings will change if the HYPRManager is re-initialized with a new userId.
 *  Settings will be stored and re-used by userId.
 */
@property (nonatomic, strong) HYPRSettings *settings;

/** 
 * The HYPRLogLevel indicating what level to log messages at.
 * 
 * @discussion This should be left at HYPRLogLevelError for all Release builds.
 */
@property (atomic, assign) HYPRLogLevel logLevel;

/** 
 * Provides the shared instance of the HyprMX Mobile SDK Manager.
 *
 * HYPRManager provides a singleton interface to the HyprMX Mobile SDK. 
 *
 * @return the instance of the HYPRManager
 */
+ (HYPRManager *)sharedManager;

/** 
 * Enables Debug logging. 
 *
 * @discussion Should not be used in production, as excessive logging can hurt performance.
 */
+ (void)enableDebugLogging;

/** 
 * Prevent the HYPRManager from preloading content when initialized.
 *
 * @discussion If you do this, be sure to call -preloadContent manually, if you don't you will have inventory problems.
 */
+ (void)disableAutomaticPreloading;

/** 
 * Initalize state on the HyprMX Mobile manager with properties.
 */
- (void)initializeWithDistributorId:(NSString *)distributorId propertyId:(NSString *)propertyId userId:(NSString *)userId;

/**
 * Initalize state on the HyprMX Mobile manager if you already set the properties.
 */
- (void)initialize;

/** 
 * Helper Method to set known required information.
 *
 * @param requiredInformation The dictionary of information to set.
 */
- (void)setRequiredInformation:(NSDictionary*)requiredInformation;

/** 
 * Begin loading offers for display. This includes preloading video content.
 * 
 * @discussion You only need to call this if you call +disableAutomaticPreloading before you initialize the HYPRManager.
 */
- (void)preloadContent;

/** 
 * Display a specific offer to a user.
 * 
 * @discussion This is for advanced use only, and not sutable for most integrations. You should likely use -displayOfferWithTransactionId:completion: instead.
 *
 * @param offer             Instance of class HYPROffer to display
 * @param displayRequest    The display request that supplied the offer.
 */
- (void)displayOffer:(HYPROffer *)offer forDisplayRequest:(HYPRDisplayRequest *)displayRequest;

/**
 * Clear all the user settings stored on this device.
 *
 * @discussion After clearing the user settings, you must re-initialize the HYPRManager.
 */
- (void)clearAllUserSettings;

/**
 * Clear the user settings associated with the current user.
 *
 * @discussion After clearing the user settings, you must re-initialize the HYPRManager.
 */
- (void)clearUserSettings;

/**
 * Check availability of offers.
 *
 * @param checkCallback         The block to call when the inventory check returns. If isOfferReady is YES, you can call displayOffer: or displayOfferWithTransactionId:completion: on the request object (now, or later).
 */
- (void)checkInventory:(HYPRBooleanResultCallback)checkCallback;

/**
 * Display an offer. We will set the transaction ID with a UUID.
 *
 * @param completionCallback    The block that is executed when an offer has been completed (successfully or not).
 *
 * @discussion We recommend calling this method inside of the param checkCallback of method checkInventory: to ensure than an offer is ready to display.
 */
- (void)displayOffer:(HYPRDisplayOfferCallback)completionCallback;

/**
 * Display an offer and set your own transaction ID. Use this method rather than displayOffer: if you wish to use your proprietary transaction ID's. If no
 * transaction ID is provided, we will set the transaction ID with a UUID.
 *
 * @param transactionId         String uniquely identifying this offer transaction.
 * @param completionCallback    The block that is executed when an offer has been completed (successfully or not).
 * 
 * @discussion We recommend calling this method inside of the param checkCallback of method checkInventory.
 */
- (void)displayOfferWithTransactionId:(NSString *)transactionId completion:(HYPRDisplayOfferCallback)completionCallback;

/* Depricated APIS */

/** 
 * Create a display request, begin fetching offers for it, and return it.
 *
 * @param presentationDelegate      An object you create to prepare and show offers once they are loaded. Must not be nil.
 * @return                          A new display request.
 */
- (HYPRDisplayRequest *)addDisplayRequestWithPresentationDelegate:(id<HYPROfferPresentationDelegate>)presentationDelegate __attribute__((deprecated));

/** 
 * Remove a display request that you added earlier. 
 *
 * @param displayRequest    A display request that you created earlier with -addDisplayRequestWithPresentationDelegate:
 */
- (void)removeDisplayRequest:(HYPRDisplayRequest *)displayRequest __attribute__((deprecated));


@end
