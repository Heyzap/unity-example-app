#import "AdColonyPubServicesTypes.h"
#import "AdColonyPubServicesDigitalItem.h"
#import "AdColonyPubServicesVIPInformation.h"
#import "AdColonyPubServicesDelegate.h"
#import "AdColonyPubServicesPushNotification.h"

@class SKPaymentTransaction;

NS_ASSUME_NONNULL_BEGIN

/**
 Main interface for the AdColonyPubServices SDK.
 */
@interface AdColonyPubServices : NSObject

#pragma mark Initialization & Settings
/**
 @abstract AdColony Publisher Services initialization method.
 @param initParams Custom data provided by the developer for advanced customization (user segmentation, etc.)
 @param delegate Receives callbacks about service connectivity, rewards, stats
 @see AdColonyPubServicesDelegate
 */
 + (void)configureWithInitParams:(NSDictionary * _Nullable)initParams delegate:(id<AdColonyPubServicesDelegate>)delegate;

/** @name Initialization & Setup */

/**
 @abstract Sets which notifications are allowed from the service. 
 @discussion Based on user interaction or server communication, various types of messages may be requested for display. This will control which can appear immediately. A developer might want to disable toast or modal notifications during gameplay and re-enable them after a round completes. It is up to the developer to re-enable notifications when appropriate. The default is AdColonyPubServicesNotificationMaskAll.
 @param allowedNotifications The set of notifications allowed
 @see AdColonyPubServicesNotificationMask
 */
+ (void)setNotificationsAllowed:(AdColonyPubServicesNotificationMask)allowedNotifications;

/**
 @abstract Retrieves the availability of the service.
 @return TRUE when the service is available.
 */
+ (BOOL)isServiceAvailable;


#pragma mark User Interface
/** @name User Interface */

/**
 @abstract This method shows the overlay.
 @discussion This will only show the overlay if the service is in the correct application state for displaying a overlay to the user. The call will be ignored in all other cases.
 
 Developers should listen to the onServiceAvailabilityChanged delegate callback and check isServiceAvailable to determine when to show a button for the overlay.

 @see [AdColonyPubServicesDelegate onOverlayVisibilityChanged]
 @see [AdColonyPubServicesDelegate onServiceAvailabilityChanged:error:]
 @see isServiceAvailable
 */
+ (void)showOverlay;

/**
 @abstract This method closes the overlay.
 @discussion The developer may want to call this after redeeming a digital item. It is left up to the developer to allow the overlay to remain open after a digital item has been redeemed.
 @see [AdColonyPubServicesDelegate onOverlayVisibilityChanged]
 */
+ (void)closeOverlay;

/**
 @abstract Retrieves the visibility status of the overlay.
 @return TRUE when the overlay is currently open.
 @see [AdColonyPubServicesDelegate onOverlayVisibilityChanged]
 */
+ (BOOL)isOverlayVisible;


#pragma mark A/B Testing
/** @name A/B Testing */

/**
 @abstract Returns the current status of all experiments
 @discussion The keys and values depend on the configuration set within the AdColony Developer Portal.
 @return The current status of all experiments as a key-value pair.
 @see getExperimentValue:
 */
+ (NSDictionary *)getExperiments;

/**
 @abstract Convenience method for retrieving the value of an experiment.
 @discussion The type will correspond to the data specified on the server.
 @param key Name of the experiment
 @return The value of the experiment
 @see getExperiments
 */
+ (NSObject *)getExperimentValue:(NSString *)key;


#pragma mark VIP Information
/** @name VIP Information */

/**
 @abstract Retrieves the latest AdColonyPubServicesVIPInformation data.
 @discussion This data might change after significant events such as IAP purchases or achievement/event completions. It is best to query for the latest data before presenting it to the user. This is a synchronous call to retrieve cached information from the service.
 @see AdColonyPubServicesVIPInformation
 */
+ (AdColonyPubServicesVIPInformation *)getVIPInformation;


#pragma mark Rewards
/** @name Rewards */

/**
 @abstract Use this method to grant a reward from the given IAP receipt. 
 @discussion This receipt will internally be validated on AdColony Publisher Services backend. If the receipt has already been used or is invalid, no reward will be given.
 @param productId Product id of the item
 @param base64ReceiptData Base-64 encoded string of the entire receipt data,
 @param transactionId Unique transaction ID of the purchase, contained within SKPaymentTransaction object
 @param inGameCurrencyQuantityForProduct If the product is in-game currency, pass in the amount of currency granted. This will be used to give the users a percentage of this currency back as a bonus based on their VIP status. Passing 0 will cause any in-game currency reward to be based on the price of the item.
 @see grantRewardFromInAppPurchase:inGameCurrencyQuantityForProduct:
 @see [AdColonyPubServicesDelegate onInAppPurchaseRewardSuccess:inGameCurrencyBonus:]
 */
+ (void)grantRewardFromInAppPurchase:(NSString *)productId receiptData:(NSString *)base64ReceiptData transactionId:(NSString *)transactionId inGameCurrencyQuantityForProduct:(int)inGameCurrencyQuantityForProduct;

/**
 @abstract Use this method as a helper to easily send the receipt information from a SKPaymentTransaction object.
 @param transaction Transaction object returned from StoreKit after purchase completes
 @param inGameCurrencyQuantityForProduct If the product is in-game currency, pass in the amount of currency granted. This will be used to give the users a percentage of this currency back as a bonus based on their VIP status. Passing 0 will cause any in-game currency reward to be based on the price of the item.
 @see grantRewardFromInAppPurchase:receiptData:transactionId:inGameCurrencyQuantityForProduct:
 @see [AdColonyPubServicesDelegate onInAppPurchaseRewardSuccess:inGameCurrencyBonus:]
 */
+ (void)grantRewardFromInAppPurchase:(SKPaymentTransaction *)transaction inGameCurrencyQuantityForProduct:(int)inGameCurrencyQuantityForProduct;

#pragma mark Stats
/**
 @name Stats

 Stats are downloaded by the service during initialization. Stat values will be locally cached and periodically sent to the server. If a user is logged into the service with two separate devices issuing a statSet(), the first call will be overwritten by the second. For that reason, it's recommended to use the increment and decrement methods as they will be atomic operations. Stats can immediately be queried after commands are issue for the latest value. Stats commands occur serially, so a command sequence of set, increment would result in a value of one. After all stat commands are sent, the client is notified via the onStatsRefreshed delegate method. Only integer stats are supported.

 To see the list of valid statistics, please visit the AdColony Developer Portal.
 */

/**
 @abstract Retrieves all the stats.
 @discussion Each stat is represented by a dictionary object with two entries, it's key and value. Each can be retrieved as follows.
 
    NSArray *stats = [AdColonyPubServices getStats];
    for (NSDictionary *stat in stats) {
        NSString *name = stat[AdColonyPubServicesStatName];
        NSNumber *value = stat[AdColonyPubServicesStatValue];
        NSLog(@"Stat: %@ = %@", name, value);
    }
 
 @return Stat data as NSDictionary objects.
 @see AdColonyPubServicesStatName
 @see AdColonyPubServicesStatValue
 */
+ (NSArray *)getStats;

/**
 @abstract Retrieves a single stat by name
 @return Numeric value of the stat, nil if there is no stat for the given name.
 @param name Name of stat
 */
+ (NSNumber *)getStatValue:(NSString *)name;

/**
 @abstract Sets a stat's value.

    [AdColonyPubServices setStat:@"level" value:7];

 @param name Name of stat
 @param value New value of the stat
 @return FALSE if the stat doesn't exist on the developer portal.
 @see [AdColonyPubServicesDelegate onStatsRefreshed]
 */
+ (BOOL)setStat:(NSString *)name value:(NSInteger)value;

/**
 @abstract Increments a stat by a specified value.

    [AdColonyPubServices incrementStat:@"score" value:42];

 @param name Name of stat
 @param value Value the stat will be incremented by, this can be negative
 @return FALSE if the stat doesn't exist on the developer portal.
 @see [AdColonyPubServicesDelegate onStatsRefreshed]
 */
+ (BOOL)incrementStat:(NSString *)name value:(NSInteger)value;

/**
 @abstract Refreshes the local stat cache from the server.
 @discussion As with any stat update, the onStatsRefreshed delegate method will be called upon completion.
 @see [AdColonyPubServicesDelegate onStatsRefreshed]
 */
+ (void)refreshStats;

/**
 @abstract Marks the start of a round for stat aggregation.
 @discussion This and markEndRound are important for auto-generated, round-based stats.
 @see markEndRound
 */
+ (void)markStartRound;

/**
 @abstract Marks the end of a round for stat aggregation.
 @discussion This and markStartRound are important for auto-generated, round-based stats.
 @see markStartRound
 */
+ (void)markEndRound;

#pragma mark Deep Linking
/** @name Deep Linking */

/**
 @abstract Tracks URLs used to open your application
 @discussion This function will track all entries into your application from openURL given your URL scheme. This allows you to execute custom commands interpretted by the developer or internal PubServices commands set from the Developer Portal.
 @param url The full url causing the application to open
 @param sourceApplication Source application from which URL opened
 @param callback This is called after the URL has been logged and formatted. The callback will have a dictionary containing key/value pairs decoded from the URL query string and a boolean indicating if URI is an AdColony scheme
 @return TRUE if URI is an AdColony scheme
 */
+ (BOOL)handleOpenURL:(NSURL *)url sourceApplication:(nullable NSString *)sourceApplication callback:(void(^)(NSDictionary * _Nullable, BOOL))callback;

#pragma mark Push Notifications
/** @name
 Push Notifications
 */

/**
 @abstract Enables the application for push notifications.
 @discussion This will cause application:didRegisterForRemoteNotificationsWithDeviceToken: to be called where the specific device token is registered with the service. If you have push notifications already enabled in your application, this can be skipped but you must still register the device token with AdColony PubServices to receive push notifications from the service. This is not automatically called by AdColony PubServices to allow the developer flexibility on when and frequency to attempt enabling push notifications.
 
     - (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
        [AdColonyPubServices registerForPushNotifications:(AdColonyPubServicesPushTypeAlert | AdColonyPubServicesPushTypeSound | AdColonyPubServicesPushTypeBadge)];
     }

 @param supportedPushAlertTypes Supported push alert types
 @see setPushNotificationDeviceToken:
 @see AdColonyPubServicesPushType
 */
+ (void)registerForPushNotifications:(NSUInteger)supportedPushAlertTypes;

/**
 @abstract Disables the application from push notifications.
 */
+ (void)unregisterForPushNotifications;

/**
 @abstract Registers the device token for push notifications. 
 @discussion This should be called after receiving deviceToken from application:didRegisterForRemoteNotificationsWithDeviceToken:.
 
     - (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
        [AdColonyPubServices setPushNotificationDeviceToken:deviceToken];
     }
 
 @param deviceToken Device token supplied from OS didRegisterForRemoteNotificationsWithDeviceToken callback after registering for notifications
 @see registerForPushNotifications:
 */
+ (void)setPushNotificationDeviceToken:(nullable NSData *)deviceToken;

/**
 @abstract AdColonyPubService handler for push notifications. 
 @discussion This will log activity and process internal commands. If the push is not from AdColony, the push will not be processed.
 
 This method should be called from applications:didReceiveRemoteNotification:, application:didReceiveRemoteNotification:fetchCompletionHandler:, or application:handleActionWithIdentifier:forRemoteNotification:completionHandler:.
 
 If you would like to check if it is a AdColonyPubServices remote notification before passing it to the handler, inspect the notification for the presence of the `adColonyPubServices` key.
 
 The AdColonyPubServices callback parameter is not the same as the OS completion handler. It is left to the developer to call the OS completion callback.
 
 This is an asynchronous call.
 
     - (void)application:(UIApplication *)application handleActionWithIdentifier:(NSString *)identifier forRemoteNotification:(NSDictionary *)notification completionHandler:(void (^)())completionHandler {
        [AdColonyPubServices handleRemoteNotification:notification action:identifier callback:^(AdColonyPubServicesPushNotification *pushNotification, BOOL handled) {
            // Must be called when finished
            completionHandler();
        }];
     }
 
 @param notification Notification data supplied from OS during handleActionWithIdentifier callback
 @param action Notification data supplied from OS during handleActionWithIdentifier callback (iOS 8 only). This can be nil.
 @param callback Callback that will be executed after processing the notification. This is not the same as the OS completionHandler object, do not attempt to pass completionHandler as the callback. It is left to the developer to call the OS's completionHandler callback.
 */
+ (void)handleRemoteNotification:(NSDictionary *)notification action:(nullable NSString *)action callback:(void(^)(AdColonyPubServicesPushNotification *, BOOL))callback;

@end

NS_ASSUME_NONNULL_END
