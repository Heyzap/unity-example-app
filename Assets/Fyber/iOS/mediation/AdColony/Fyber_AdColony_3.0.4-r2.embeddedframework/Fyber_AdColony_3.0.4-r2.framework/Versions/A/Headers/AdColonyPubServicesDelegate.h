#import "AdColonyPubServicesTypes.h"

/**
 Use the AdColonyPubServicesDelegate to receive information about system status and control of subsystems.
 */
@protocol AdColonyPubServicesDelegate <NSObject>
@optional

/** @name User Interface */

/**
 @abstract Service availability changed callback
 @discussion This is called when the service availability has changed either due to an initialization, network conditions or server status. The developer should listen to this notification to determine visibility of UI elements.
 
    - (void)onServiceAvailabilityChanged:(AdColonyPubServicesAvailability)availability {
        // Show or hide overlay button based on result...
        if ([AdColonyPubServices shouldShowOverlay]) {
        }
        
        // Show something is new on the overlay with the previously set value
        if (self.updateAvailable) {
           // Show badge?
        }
    }

 @see AdColonyPubServices:shouldShowOverlay
 @param availability current service availability status
 @param error additional information coinciding with availability, could be nil
 @note This will be dispatched on the main thread.
 */
- (void)onServiceAvailabilityChanged:(AdColonyPubServicesAvailability)availability error:(NSString *)error;

/**
 @abstract Overlay availability changed callback
 @discussion This is called when the overlay view either appears or disappears. The developer could use this to determine when to pause their app if necessary or change the update availability of something within the overlay.
 */
- (void)onOverlayVisibilityChanged;

/** @name Rewards */

/**
 @abstract Grant digital item callback
 @discussion A digital package is a group of digital items setup on the dev portal to be granted to the user as a distinct item.
 For each item in the digital package, the service will query to see if an item can be granted before attempting to grant the item.
 If the developer wants to close the overlay to present a custom message to the user, call [AdColonyPubServices closeOverlay] before this delegate completes.
 @param digitalItem A single AdColonyPubServicesDigitalItem containing quantity and productId.
 @note This will be dispatched on the main thread.
 @see AdColonyPubServicesDigitalItem
 */
- (void)onGrantDigitalProductItem:(AdColonyPubServicesDigitalItem *)digitalItem;

/**
 @abstract IAP purchase success callback
 @discussion This delegate method is called after an In App Purchase is made and a reward has been granted by the system. An internal toast message will be shown if notifications are enabled.
 @note This will be dispatched on the main thread.
 @param productId Product id of IAP item
 @param inGameCurrencyBonus Additional quantity of in-game currency the user will expect based on their VIP rank
 @see setNotificationsAllowed:
 */
- (void)onInAppPurchaseRewardSuccess:(NSString *)productId inGameCurrencyBonus:(unsigned int)inGameCurrencyBonus;

/**
 @abstract IAP purchase failure callback
 @discussion This delegate method is called after an in-app purchase is made. A reward grant was attempted, but failed. See the error message for more details.
 @note This will be dispatched on the main thread.
 @param productId Product id of IAP item
 @param error Error describing failure reason
 */
- (void)onInAppPurchaseReward:(NSString *)productId didFail:(NSError *)error;

/** @name Stats */

/**
 @abstract Stats refreshed callback
 @discussion This delegate method is called after the stats have been retrieved from the server. This can happen at various times and it's up to the client how to propogate the message to its own user interface if appropriate.  It's possible that no changes in the stats have occurred between each callback.
 @note This will be dispatched on the main thread.
 */
- (void)onStatsRefreshed;

/**
 @abstract VIP information changed callback
 @discussion This delegate method is called after the user's VIP information has been changed. This can happen at any time and it's up to the client how to propogate the message to its own user interface if appropriate.
 @note This will be dispatched on the main thread.
 */
- (void)onVIPInformationChanged;

@end