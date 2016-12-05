/**
 * AdColony ad request error codes
 */
typedef NS_ENUM(NSUInteger, AdColonyRequestError) {
    
    /** An invalid app id or zone id was specified by the developer or an invalid configuration was received from the server (unlikely). */
    AdColonyRequestErrorInvalidRequest = 0,
    
    /** The ad was skipped due to the skip interval setting on the control panel. */
    AdColonyRequestErrorSkippedRequest,
    
    /** The current zone has no ad fill. */
    AdColonyRequestErrorNoFillForRequest,
    
    /** Either AdColony has not been configured, is still in the process of configuring, is still downloading assets, or is already showing an ad. */
    AdColonyRequestErrorUnready
};

@interface AdColonyAdRequestError : NSError
@end
