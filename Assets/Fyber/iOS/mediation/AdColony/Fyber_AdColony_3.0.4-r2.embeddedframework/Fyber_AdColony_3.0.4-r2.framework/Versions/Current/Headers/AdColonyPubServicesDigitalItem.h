/**
 Wrapper for digital items granted to a user.
 @see AdColonyPubServicesDelegate:onGrantDigitalProductItem:
 */
@interface AdColonyPubServicesDigitalItem : NSObject

/** @name Properties */
/**
 @abstract ProductId of the digital item as set in the AdColony Developer Portal
 */
@property (nonatomic, readonly) NSString *productId;

/**
 @abstract Quantity of the item, i.e. 500 Coins
 */
@property (nonatomic, readonly) NSInteger quantity;

/**
 @abstract Name as set in the AdColony Developer Portal
 */
@property (nonatomic, readonly) NSString *name;

/**
 @abstract Description as set in the AdColony Developer Portal
 */
@property (nonatomic, readonly) NSString *productDescription;

/**
 @abstract Additional parameters set on the AdColony Developer Portal
 */
@property (nonatomic, readonly) NSDictionary *userParams;

@end
