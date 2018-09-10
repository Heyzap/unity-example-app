//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <CoreLocation/CoreLocation.h>
#import <CoreGraphics/CoreGraphics.h>

#import "FYBUserConstants.h"

/**
 * Object that contains the information about the user. This is used to create user segments
 *
 */
@interface FYBUser : NSObject

///-------------------------
/// Setters
///-------------------------

/**
 *  Sets the user's age
 *
 *  @param age Age of the user. Pass `FYBEntryIgnore` if value needs to be ignored or to be removed, if already exists
 *
 *  @since v7.0.0
 */
- (void)setAge:(NSInteger)age;

/**
 *  Sets the user's date of birth
 *
 *  @param date Date of birth of the user. Pass `nil` if value needs to be ignored or to be removed, if already exists
 *
 *  @since v7.0.0
 */
- (void)setBirthdate:(NSDate *)date;

/**
 *  Sets the user's gender
 *
 *  @param gender Gender of the user. Pass FYBUserGenderUndefined if value needs to be ignored or to be removed, if already exists
 *
 *  @since v7.0.0
 */
- (void)setGender:(FYBUserGender)gender;

/**
 *  Sets the user's sexual orientation
 *
 *  @param sexualOrientation Sexual orientation of the user. Pass FYBUserSexualOrientationUndefined if value needs to be ignored or to be removed, if already exists.
 *
 *  @since v7.0.0
 */
- (void)setSexualOrientation:(FYBUserSexualOrientation)sexualOrientation;

/**
 *  Sets the user's ethnicity
 *
 *  @param ethnicity Ethnicity of the user. Pass FYBUserEthnicityUndefined if value needs to be ignored or to be removed, if already exists
 *
 *  @since v7.0.0
 */
- (void)setEthnicity:(FYBUserEthnicity)ethnicity;

/**
 *  Set the user's location
 *
 *  @param geoLocation takes CLLocation. Pass nil if location needs to be ignored or to be removed, if already exists
 *
 *  @since v7.0.0
*/
- (void)setLocation:(CLLocation *)geoLocation;

/**
 *  Sets the user's marital status
 *
 *  @param status Marital status of the user. Pass FYBUserMaritalStatusUndefined if value needs to be ignored or to be removed if already exists
 *
 *  @since v7.0.0
 */
- (void)setMaritalStatus:(FYBUserMaritalStatus)status;

/**
 *  Sets the user's number of children
 *
 *  @param numberOfChildren The number of children
 */
- (void)setNumberOfChildren:(NSInteger)numberOfChildren;
/**
 *  Sets the user's annual household income
 *
 *  @param income Annual household income of the user. Pass `FYBEntryIgnore` if value needs to be ignored or to be removed, if already exists
 *
 *  @since v7.0.0
 */
- (void)setAnnualHouseholdIncome:(NSInteger)income;

/**
 *  Sets the user's educational background
 *
 *  @param education Education of the user. Pass FYBUserEducationUndefined if value needs to be ignored or to be removed, if already exists
 *
 */
- (void)setEducation:(FYBUserEducation)education;

/**
 *  Sets the user's zipcode
 *
 *  @param zipcode Zipcode of the current living place of the user. Pass `nil` if value needs to be ignored or to be removed, if already exists
 *
 *  @since v7.0.0
 */
- (void)setZipcode:(NSString *)zipcode;

/**
 *  Set the user's list of interests
 *
 *  @param interests List of interests of the user. Pass `nil` if value needs to be ignored or to be removed, if already exists
 *
 *  @since v7.0.0
 */
- (void)setInterests:(NSArray *)interests;

/**
 *  Sets if in-app purchases are enabled.
 *
 *  @param flag Sets if in-app purchases are enabled
 *
 *  @since v7.0.0
 */
- (void)setIap:(BOOL)flag;

/**
 *  Sets the amount that the user has already spent on in-app purchases
 *
 *  @param amount The amount of money that the user has spent
 *
 *  @since v7.0.0
 */
- (void)setIapAmount:(CGFloat)amount;

/**
 *  Sets the number of sessions
 *
 *  @param numberOfSessions The number of sessions that has already been started
 *
 *  @since v7.0.0
 */
- (void)setNumberOfSessions:(NSInteger)numberOfSessions;

/**
 *  Sets the time spent on the current session
 *
 *  @param timestamp The time spent on the current session
 *
 *  @since v7.0.0
 */
- (void)setPsTime:(NSTimeInterval)timestamp;

/**
 *  Sets the duration of the last session
 *
 *  @param session The duration of the last session
 *
 *  @since v7.0.0
 */
- (void)setLastSession:(NSTimeInterval)session;

/**
 *  Sets the connection type used by the user
 *
 *  @param connectionType The connection type used by the user
 *
 *  @see FYBUserConnectionType
 *
 *  @since v7.0.0
 */
- (void)setConnectionType:(FYBUserConnectionType)connectionType;

/**
 *  Sets the device used by the user
 *
 *  @param device The device used by the user
 *
 *  @see FYBUserDevice
 *
 *  @since v7.0.0
 */
- (void)setDevice:(FYBUserDevice)device;

/**
 *  Sets the app version
 *
 *  @param version The version of the app currently executed
 *
 *  @since v7.0.0
 */
- (void)setVersion:(NSString *)version;

/**
 *  Sets custom parameters to be sent along with the standard parameters
 *
 *  @param parameters The custom parameters that must be set
 *
 *  @since v7.0.0
 */
- (void)setCustomParameters:(NSDictionary *)parameters;

/**
 *  Sets the consent status of the user. Fyber will only be able to show targeted advertising if the user consented.
 *
 *  @param gdprConsent The consent status of the user
 *
 *  @since v8.21.0
 */
- (void)setGDPRConsent:(BOOL)gdprConsent;

/**
 *  Sets the consent data.
 *
 *  @param gdprConsentData The consent data
 *
 *  @since v8.22.0
 */
- (void)setGDPRConsentData:(NSDictionary<NSString *, NSString *> *)gdprConsentData;

/**
 *  Clears the consent data of the user.
 *
 *  @since v8.22.0
 */
- (void)clearGDPRConsentData;

///-------------------------
/// Getters
///-------------------------

/**
 *  Returns the age of the user previously set using `-setAge:`
 *
 *  @return Age of the user. If the age is not added it returns `FYBEntryIgnore`
 *
 *  @since v7.0.0
 */
- (NSInteger)age;

/**
 *  Returns the date of birth of the user previously set using `-setBirthDate:`
 *
 *  @return Date of birth of the user. If the birthdate is not added it returns `nil`
 *
 *  @since v7.0.0
 */
- (NSDate *)birthdate;

/**
 *  Returns the gender of the user previously set using `-setGender:`
 *
 *  @return Gender of the user. If the gender is not added it returns `FYBUserGenderUndefined`
 *
 *  @since v7.0.0
 */
- (FYBUserGender)gender;

/**
 *  Returns the sexual orientation of the user previously set using `-setSexualOrientation:`
 *
 *  @return Sexual orientation of the user. If sexual orientation is not added it returns `FYBUserSexualOrientationUndefined`
 *
 *  @since v7.0.0
 */
- (FYBUserSexualOrientation)sexualOrientation;

/**
 *  Returns the ethnicity of the user previously set using `-setEthnicity:`
 *
 *  @return Ethnicity of the user. If the ethnicity is not added it returns `FYBUserEthnicityUndefined`
 *
 *  @since v7.0.0
 */
- (FYBUserEthnicity)ethnicity;

/**
 *  Returns the current location of the user
 *
 *  @return User's current location. If location is not added it returns nil
 *
 *  @since v7.0.0
 */
- (CLLocation *)location;

/**
 *  Returns the marital status of the user previously set using `-setMaritalStatus:`
 *
 *  @return FYBUserMaritalStatus type of user's marital status. If marital status is not added it returns `FYBUserMaritalStatusUndefined`
 *
 *  @since v7.0.0
 */
- (FYBUserMaritalStatus)maritalStatus;

/**
 *  Returns the number of childre of the user previously set using `-setNumberOfChildren:`
 *
 *  @return The number of children from the user
 */
- (NSInteger)numberOfChildren;

/**
 *  Returns the information about the annual household income of the user previously set using `-setAnnualHouseholdIncome:`
 *
 *  @return Annual household income of the user. If annual household income is not added it returns `FYBEntryIgnore`
 *
 *  @since v7.0.0
 */
- (NSInteger)annualHouseholdIncome;

/**
 *  Returns the education background of the user previously set using `-setEducation:`
 *
 *  @return FYBUserEducation type of user's educational status. If education is not added it returns `FYBUserEducationUndefined`
 *
 *  @since v7.0.0
 */
- (FYBUserEducation)education;

/**
 *  Returns the zipcode of the user previously set using `-setZipCode:`
 *
 *  @return Zipcode of the current living place of the user
 *
 *  @since v7.0.0
 */
- (NSString *)zipcode;

/**
 *  Returns the list of interests of the user previously set using `-setInterests:`
 *
 *  @return Array containing strings of interests of current user
 *
 *  @since v7.0.0
 */
- (NSArray *)interests;

/**
 *  Requests user values in dictionary.
 *
 *  @return Dictionary containing all set up values for current user
 *
 *  @since v7.0.0
 */
- (NSDictionary *)data;

/**
 *  Returns the user values in dictionary with current location
 *
 *  @param completionBlock The block to be executed on the completion of request. This block has no return value and takes 1 argument: the dictionary containing all set up values for the current user including their latest location. Location, however, is included ONLY if the user's app has `Core Location` service enabled or the location was not set manually by calling `-setLocation:`, otherwise the location is not included in returned dictionary
 *
 *  @discussion Setting manually location by calling `-setLocation:` overrides automatic location parameters
 *
 *  @see - setLocation:
 *
 *  @since v7.0.0
 */
- (void)dataWithCurrentLocation:(void (^)(NSDictionary *data))completionBlock;

/**
 *  Resets all user values
 *
 *  @since v7.0.0
 */
- (void)reset;

/**
 *  Returns the availability of in-app purchases previously set using `-setIap:`
 *
 *  @return YES if enable, NO if disabled
 *
 *  @since v7.0.0
 */
- (BOOL)iap;

/**
 *  Returns the amount the user has spent on in-app purchases previously set using `-setIapAmount:`
 *
 *  @return The amount spent on in-app purchases
 *
 *  @since v7.0.0
 */
- (CGFloat)iapAmount;

/**
 *  Returns the number of sessions previously set using `-setNumberOfSessions`
 *
 *  @return The number of sessions
 *
 *  @since v7.0.0
 */
- (NSInteger)numberOfSessions;

/**
 *  Returns the time of the current session previously set using `-setPsTime:`
 *
 *  @return The duration of the current session
 *
 *  @since v7.0.0
 */
- (NSTimeInterval)psTime;

/**
 *  Returns the duration of the last session previously set using `-setLastSession:`
 *
 *  @return The duration of the last session
 *
 *  @since v7.0.0
 */
- (NSTimeInterval)lastSession;

/**
 *  Returns the connection type previously set using `-setConnectionType:`
 *
 *  @return The connection type
 *
 *  @see FYBUserConnectionType
 *
 *  @since v7.0.0
 */
- (FYBUserConnectionType)connectionType;

/**
 *  Returns the device previously set by `-setDevice:`
 *
 *  @return The device model used by the user
 *
 *  @see FYBUserDevice
 *
 *  @since v7.0.0
 */
- (FYBUserDevice)device;

/**
 *  Returns the version of the app previously set by `-setVersion:`
 *
 *  @return The version of the app
 *
 *  @since v7.0.0
 */
- (NSString *)version;

/**
 *  Returns a copy of the custom parameters previously set by `-setCustomParameters:`
 *
 *  @return A copy of the custom parameters
 *
 *  @since v7.0.0
 */
- (NSDictionary *)customParameters;

@end
