//
//  HZDemographics.h
//  Heyzap
//
//  Created by Maximilian Tagher on 12/2/15.
//  Copyright © 2015 Heyzap. All rights reserved.
//

#import <Foundation/Foundation.h>

@class CLLocation;

typedef NS_ENUM(NSUInteger, HZUserGender) {
    HZUserGenderUnknown = 0,
    HZUserGenderMale,
    HZUserGenderFemale,
    HZUserGenderOther,
};

typedef NS_ENUM(NSUInteger, HZUserMaritalStatus) {
    HZUserMaritalStatusUnknown = 0,
    HZUserMaritalStatusSingle,
    HZUserMaritalStatusMarried,
};

typedef NS_ENUM(NSUInteger, HZUserEducation) {
    HZUserEducationUnknown = 0,
    /// stopped after / still attending 1st-8th grades
    HZUserEducationGradeSchool,
    /// stopped after / still attending 9th-12th grades
    HZUserEducationHighSchoolUnfinished,
    /// stopped after high school and is not attending college
    HZUserEducationHighSchoolFinished,
    /// stopped / still attending college with no college degree finished
    HZUserEducationCollegeUnfinished,
    /// already achieved an Associate degree
    HZUserEducationAssociateDegree,
    /// already achieved a Bachelor's degree
    HZUserEducationBachelorsDegree,
    /// already achieved a graduate-level degree (i.e.: Master's)
    HZUserEducationGraduateDegree,
    /// already achieved a post-graduate degree (i.e.: Doctorate)
    HZUserEducationPostGraduateDegree,
};


/**
 *  Set the properties on this class to pass information about the user to each of the mediated ad networks.
 * 
 *  Setting any/all of these values is optional. Many ad networks will use this information to serve better-targetted ads.
 */
@interface HZDemographics : NSObject

/**
 *  The user's current location.
 *
 *  Networks who use this information: AdColony, AdMob, AppLovin, Heyzap Exchange, InMobi
 */
@property (nonatomic, strong, nullable) CLLocation *location;

/**
 *  The user's birthdate.
 *
 *  Some networks will only use the age / birth year / age range of the user, and some will use the full birthdate, so you can set this as accurately as possible and we'll give what we can to each network that asks for it. For instance, if you only know that a user is 25, you can set the birthdate to 25 years from today and that will be sufficient.
 *
 *  Networks who use this information: AdColony, AdMob, AppLovin, Heyzap Exchange, InMobi, Leadbolt
 */
@property (nonatomic, strong, nullable) NSDate *userBirthDate;

/*
 * A list of the user's interests.
 *
 *  Networks who use this information: AdColony, AppLovin
 */
#if __has_feature(objc_generics)
@property (nonatomic, strong, nullable) NSArray<NSString *> *userInterests;
#else
@property (nonatomic, strong, nullable) NSArray *userInterests;
#endif

/*
 *  The user's gender.
 *
 *  Networks who use this information: AdColony, AdMob, AppLovin, Heyzap Exchange, InMobi, Leadbolt
 */
@property (nonatomic) HZUserGender userGender;


/**
 *  The user's Postal/ZIP code.
 *
 *  Networks who use this information: AdColony, Heyzap Exchange, InMobi
 */
@property (nonatomic, strong, nullable) NSString *userPostalCode;

/**
 *  The user's household income.
 *
 *  Networks who use this information: AdColony, Heyzap Exchange, InMobi
 */
@property (nonatomic, strong, nullable) NSNumber *userHouseholdIncome;

/**
 *  The user's marital status.
 *
 *  Networks who use this information: AdColony, Heyzap Exchange
 */
@property (nonatomic) HZUserMaritalStatus userMaritalStatus;

/**
 *  The user's highest-finished education level.
 *
 *  Networks who use this information: AdColony, Heyzap Exchange, InMobi
 */
@property (nonatomic) HZUserEducation userEducationLevel;

@end
