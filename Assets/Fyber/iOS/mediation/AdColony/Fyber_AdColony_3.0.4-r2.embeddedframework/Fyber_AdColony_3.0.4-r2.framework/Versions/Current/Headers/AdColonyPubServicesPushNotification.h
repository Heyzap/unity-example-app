/**
 Wrapper for push notifications that have gone through handleRemoteNotification:action:callback:
 */
@interface AdColonyPubServicesPushNotification : NSObject

/** @name Properties */
/** 
 @abstract Unique notification id 
 */
@property (nonatomic, readonly) NSString *notificationId;

/** 
 @abstract Action that triggered handling of the push notification (iOS8 only)
 */
@property (nonatomic, readonly) NSString *action;

/**
 @abstract String displayed within the push notification
 */
@property (nonatomic, readonly) NSString *message;

/**
 @abstract Title displayed within the push notification
 */
@property (nonatomic, readonly) NSString *title;

/**
 @abstract Category of the notification (iOS8 only)
 */
@property (nonatomic, readonly) NSString *category;

/**
 @abstract Date user interacted with the push or was received if in foreground
 */
@property (nonatomic, readonly) NSDate *dateReceived;

/**
 @abstract Developer-specific data set from the AdColony Developer Portal.
 @discussion This can be used for deep-linking or any special logic the developer would like to specify.
 */
@property (nonatomic, readonly) NSString *payload;

/**
 @abstract Whether or not this is a notification sent by AdColony PubServices
 */
@property (nonatomic, readonly) BOOL isPubServicesNotification;

@end
