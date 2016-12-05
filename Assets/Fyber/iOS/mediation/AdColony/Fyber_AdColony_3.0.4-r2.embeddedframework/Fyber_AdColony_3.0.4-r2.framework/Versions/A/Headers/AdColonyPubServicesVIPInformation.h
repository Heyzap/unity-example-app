/**
 Information about a user that can be used to customize their experience and determine how to market certain features.
 This data is based on a user's spend and activity levels. 
 For example, developers can display a user's VIP rank progress and bonus information before a purchase to incentivize IAP.
 */
@interface AdColonyPubServicesVIPInformation : NSObject

/** @name Properties */
/** 
 @abstract VIP rank name
 */
@property (nonatomic, readonly) NSString *rankName;

/**
 @abstract Next VIP rank name
 */
@property (nonatomic, readonly) NSString *nextRankName;

/**
 @abstract VIP rank level
 */
@property (nonatomic, readonly) NSUInteger rankLevel;

/**
 @abstract Next VIP rank level
 */
@property (nonatomic, readonly) NSUInteger nextRankLevel;

/**
 @abstract Percent progress to next rank. Max rank, 1.0.
 */
@property (nonatomic, readonly) CGFloat rankPercentProgress;

/**
 @abstract Total amount of in-game currency to grant per currency locale unit. 
 @discussion This value should only be used for non-in-game currency IAP items, i.e. non-consumables or consumable objects.
 The mapping of in-game currency to currency unit is set from the AdColony Developer Portal.
 For example:
 
 IAP Item:
 Level Unlock A  - $0.99 USD
 bonusPerCurrencyLocaleUnit = 0.15
 
 If the developer sets a ratio of 1000 coins per USD, when a user purchases this product, they would receive a currency bonus of:
 
 1000 * 0.99USD * 0.15 = 149 (148.5 rounded up)
 
 Bonus percentages may vary for each VIP rank. This must be granted in the onInAppPurchaseRewardSuccess delegate.
 @see [AdColonyPubServicesDelegate onInAppPurchaseRewardSuccess:inGameCurrencyBonus:]
 */
@property (nonatomic, readonly) CGFloat bonusPerCurrencyLocaleUnit;

/**
 @abstract Next VIP rank bonusPerCurrencyLocaleUnit
 @see bonusPerCurrencyLocaleUnit
 @see [AdColonyPubServicesDelegate onInAppPurchaseRewardSuccess:inGameCurrencyBonus:]
 */
@property (nonatomic, readonly) CGFloat nextBonusPerCurrencyLocaleUnit;

/**
 @abstract Percent of the in-game currency to grant per IAP item.
 @discussion This bonus is above and beyond the base quantity of the IAP item.
 
 For example:
 
 IAP Item:
 Gold Package A (1000 Coins) - $0.99 USD
 bonusPerProductUnit = 0.15
 
 In this case, since the IAP product is in-game currency based, the user will receive an in-game currency bonus of:
 
 1000 * 0.15 = 150 Coins
 
 Bonus percentages may vary for each VIP rank.
 @see [AdColonyPubServicesDelegate onInAppPurchaseRewardSuccess:inGameCurrencyBonus:]
 */
@property (nonatomic, readonly) CGFloat bonusPerProductUnit;

/**
 @abstract Next VIP rank bonusPerProductUnit
 @see bonusPerProductUnit
 @see [AdColonyPubServicesDelegate onInAppPurchaseRewardSuccess:inGameCurrencyBonus:]
 */
@property (nonatomic, readonly) CGFloat nextBonusPerProductUnit;

/**
 @abstract Number of achievements visible within the overlay, completed and uncompleted
 */
@property (nonatomic, readonly) NSInteger totalAchievementCount;

/**
 @abstract Number of pending redemptions available from achievements
 */
@property (nonatomic, readonly) NSInteger pendingAchievementRedemptionCount;

/**
 @abstract Raw data exposed for developer use for fields not yet strongly typed.
 */
@property (nonatomic, readonly) NSDictionary *rawData;

@end
