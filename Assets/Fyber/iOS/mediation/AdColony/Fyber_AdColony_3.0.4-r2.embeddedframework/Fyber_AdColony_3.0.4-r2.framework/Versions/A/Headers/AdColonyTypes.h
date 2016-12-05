/**
 Enum for in-app purchase (IAP) engagement types
 */
typedef NS_ENUM(NSUInteger, AdColonyIAPEngagement) {
    
    /** IAP was enabled for the ad, and the user engaged via a dynamic end card (DEC). */
    AdColonyIAPEngagementEndCard = 0,
    
    /** IAP was enabled for the ad, and the user engaged via an in-vdeo engagement (Overlay). */
    AdColonyIAPEngagementOverlay
};
