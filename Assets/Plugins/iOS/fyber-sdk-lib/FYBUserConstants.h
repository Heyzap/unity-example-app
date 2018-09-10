//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

static const NSInteger FYBEntryIgnore = NSNotFound;

///-------------------------
/// Basic data
///-------------------------

/**
 *  The gender of the user
 */
typedef NS_ENUM(NSInteger, FYBUserGender) {
    /**
     *  Gender: undefined
     */
    FYBUserGenderUndefined = -1,
    /**
     *  Gender: male
     */
    FYBUserGenderMale,
    /**
     *  Gender: female
     */
    FYBUserGenderFemale,
    /**
     *  Gender: other
     */
    FYBUserGenderOther
};

/**
 *  The sexual orientation of the user
 */
typedef NS_ENUM(NSInteger, FYBUserSexualOrientation) {
    /**
     *  Sexual orientation: undefined
     */
    FYBUserSexualOrientationUndefined = -1,
    /**
     *  Sexual orientation: straight
     */
    FYBUserSexualOrientationStraight,
    /**
     *  Sexual orientation: bisexual
     */
    FYBUserSexualOrientationBisexual,
    /**
     *  Sexual orientation: gay
     */
    FYBUserSexualOrientationGay,
    /**
     *  Sexual orientation: unknown
     */
    FYBUserSexualOrientationUnknown
};

/**
 *  The ethnicity of the user
 */
typedef NS_ENUM(NSInteger, FYBUserEthnicity) {
    /**
     *  Ethnicity: undefined
     */
    FYBUserEthnicityUndefined = -1,
    /**
     *  Ethnicity: asian
     */
    FYBUserEthnicityAsian,
    /**
     *  Ethnicity: black
     */
    FYBUserEthnicityBlack,
    /**
     *  Ethnicity: hispanic
     */
    FYBUserEthnicityHispanic,
    /**
     *  Ethnicity: indian
     */
    FYBUserEthnicityIndian,
    /**
     *  Ethnicity: middle eastern
     */
    FYBUserEthnicityMiddleEastern,
    /**
     *  Ethnicity: native american
     */
    FYBUserEthnicityNativeAmerican,
    /**
     *  Ethnicity: pacific islander
     */
    FYBUserEthnicityPacificIslander,
    /**
     *  Ethnicity: white
     */
    FYBUserEthnicityWhite,
    /**
     *  Ethnicity: other
     */
    FYBUserEthnicityOther
};

/**
 *  The marital status of the user
 */
typedef NS_ENUM(NSInteger, FYBUserMaritalStatus) {
    /**
     *  Marital status: undefined
     */
    FYBUserMaritalStatusUndefined = -1,
    /**
     *  Marital status: single
     */
    FYBUserMartialStatusSingle,
    /**
     *  Marital status: in a relationship
     */
    FYBUserMartialStatusRelationship,
    /**
     *  Marital status: married
     */
    FYBUserMartialStatusMarried,
    /**
     *  Marital status: divorced
     */
    FYBUserMartialStatusDivorced,
    /**
     *  Marital status: engaged
     */
    FYBUserMartialStatusEngaged
};

/**
 *  The education of the user
 */
typedef NS_ENUM(NSInteger, FYBUserEducation) {
    /**
     *  Education: undefined
     */
    FYBUserEducationUndefined = -1,
    /**
     *  Education: other
     */
    FYBUserEducationOther,
    /**
     *  Education: none
     */
    FYBUserEducationNone,
    /**
     *  Education: highschool
     */
    FYBUserEducationHighSchool,
    /**
     *  Education: in college
     */
    FYBUserEducationInCollege,
    /**
     *  Education: some college
     */
    FYBUserEducationSomeCollege,
    /**
     *  Education: associates
     */
    FYBUserEducationAssociates,
    /**
     *  Education: bachelors
     */
    FYBUserEducationBachelors,
    /**
     *  Education: masters
     */
    FYBUserEducationMasters,
    /**
     *  Education: doctorate
     */
    FYBUserEducationDoctorate
};

///-------------------------
/// Extra
///-------------------------

/**
 *  The connection type of the user
 */
typedef NS_ENUM(NSInteger, FYBUserConnectionType) {
    /**
     *  Connection type: undefined
     */
    FYBUserConnectionTypeUndefined = -1,
    /**
     *  Connection type: Wifi
     */
    FYBUserConnectionTypeWiFi,
    /**
     *  Connection type:  3G
     */
    FYBUserConnectionType3G,
    /**
     *  Connection type: LTE
     */
    FYBUserConnectionTypeLTE,
    /**
     *  Connection type: Edge
     */
    FYBUserConnectionTypeEdge
};

/**
 *  The device of the user
 */
typedef NS_ENUM(NSInteger, FYBUserDevice) {
    /**
     *  Device: undefined
     */
    FYBUserDeviceUndefined = -1,
    /**
     *  Device: iPhone
     */
    FYBUserDeviceIPhone,
    /**
     *  Device: iPad
     */
    FYBUserDeviceIPad,
    /**
     *  Device: iPod
     */
    FYBUserDeviceIPod
};
