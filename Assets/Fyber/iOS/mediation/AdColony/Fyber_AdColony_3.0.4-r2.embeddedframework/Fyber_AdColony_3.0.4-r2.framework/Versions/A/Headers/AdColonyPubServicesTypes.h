/** Domain used within NSError objects */
FOUNDATION_EXPORT NSString *const AdColonyPubServicesErrorDomain;

/** Stat related keys for data returned from [AdColonyPubServices getStats] call., name of the stat */
FOUNDATION_EXPORT NSString *const AdColonyPubServicesStatName;

/** Stat related keys for data returned from [AdColonyPubServices getStats] call, value of the stat */
FOUNDATION_EXPORT NSString *const AdColonyPubServicesStatValue;

/**
 Error codes used within NSError objects
 */
typedef NS_ENUM(NSUInteger, AdColonyPubServicesError) {
    /** Unable to intialize the app */
    AdColonyPubServicesErrorInitializationError = 1,
    
    /** Unable to redeem a digital product */
    AdColonyPubServicesErrorDigitalRedemptionError,
    
    /** Unable to grant reward for IAP purchase */
    AdColonyPubServicesErrorInAppPurchaseRewardError,
    
    /** Unable to update stats */
    AdColonyPubServicesErrorStatUpdateError,
    
    /** Unable to claim a server reward */
    AdColonyPubServicesErrorServerRewardError,
    
    /** Unknown error */
    AdColonyPubServicesErrorUnknown
};

/**
 AdColony Publisher Services availability states
 */
typedef NS_ENUM(NSInteger, AdColonyPubServicesAvailability) {
    /** Undetermined availability */
    AdColonyPubServicesAvailabilityUnknown = -1,
    
    /** Confirmed unavailable due to error condition */
    AdColonyPubServicesAvailabilityUnavailable,
    
    /** Currently connecting */
    AdColonyPubServicesAvailabilityConnecting,
    
    /** Confirmed as available */
    AdColonyPubServicesAvailabilityAvailable,
    
    /** Requests are made to the service without visual notifications. The overlay will not be available. */
    AdColonyPubServicesAvailabilityInvisible,
    
    /** Service is currently under maintenance */
    AdColonyPubServicesAvailabilityMaintenance,
    
    /** Service will still attempt to queue grants, redemption & analytics */
    AdColonyPubServicesAvailabilityDisabled,
    
    /** Service will no longer operate */
    AdColonyPubServicesAvailabilityBanned
};

/**
 @abstract Allowed types of notifications.
 @see AdColonyPubServicesNotificationMask
 @see setNotificationsAllowed:
 */
typedef NS_ENUM(NSUInteger, AdColonyPubServicesNotification) {
    /** No notification messages are allowed */
    AdColonyPubServicesNotificationNone     = 0,
    
    /** Toast popup notification messages are allowed */
    AdColonyPubServicesNotificationToast,
    
    /** Full-screen modal notification messages are allowed */
    AdColonyPubServicesNotificationModal
};

/**
 Masks for combining allowed types of notifications
 @see setNotificationsAllowed:
 */
typedef NS_OPTIONS(NSUInteger, AdColonyPubServicesNotificationMask) {
    /** No notification messages are allowed */
    AdColonyPubServicesNotificationMaskNone     = 0,
    
    /** Toast popup notification messages are allowed */
    AdColonyPubServicesNotificationMaskToast    = 1 << AdColonyPubServicesNotificationToast,
    
    /** Full-screen modal notification messages are allowed */
    AdColonyPubServicesNotificationMaskModal    = 1 << AdColonyPubServicesNotificationModal,
    
    /** All notification messages are allowed */
    AdColonyPubServicesNotificationMaskAll      = (AdColonyPubServicesNotificationMaskToast | AdColonyPubServicesNotificationMaskModal)
};

/**
 Alert types allowed with Push notifications
 @see AdColonyPubServices:registerForPushNotifications:
 */
typedef NS_ENUM(NSUInteger, AdColonyPubServicesPushType) {
    /** The application may not present any UI upon a notification being received */
    AdColonyPubServicesPushTypeNone    = 0,
    
    /**  The application may badge its icon upon a notification being received */
    AdColonyPubServicesPushTypeBadge   = 1 << 0,
    
    /** The application may play a sound upon a notification being received */
    AdColonyPubServicesPushTypeSound   = 1 << 1,
    
    /** The application may show a visual notification being received */
    AdColonyPubServicesPushTypeAlert   = 1 << 2
};
