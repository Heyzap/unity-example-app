//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

/**
 *  Enum for setting the log output level
 */
typedef NS_ENUM(NSUInteger, FYBLogLevel) {
    /**
     *  No logs
     */
    FYBLogLevelOff     = 0,

    /**
     *  Log debug statements
     */
    FYBLogLevelDebug   = 10,

    /**
     *  Log information about the SDK's behaviour
     */
    FYBLogLevelInfo    = 20,

    /**
     *  Log non-critical disfunctionment
     */
    FYBLogLevelWarn    = 30,

    /**
     *  Log critical error only
     */
    FYBLogLevelError   = 40
};
