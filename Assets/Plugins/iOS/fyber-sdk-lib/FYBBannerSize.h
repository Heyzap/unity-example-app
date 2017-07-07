//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <UIKit/UIKit.h>

/**
 *  Banner size constraints
 */
typedef NS_ENUM(NSInteger, FYBBannerSizeConstraint) {
    /**
     *  the banner has a fixed width/height (default)
     */
    FYBBannerSizeConstraintFixed = 0,

    /**
     *  the banner has a smart width/height that fills the screen
     */
    FYBBannerSizeConstraintSmart
};


/**
 *  Banner orientation constraints
 *  Some smart banners behave differently depending on if they are meant to be used in portrait or landscape mode
 */
typedef NS_ENUM(NSInteger, FYBBannerOrientationConstraint) {
    /**
     *  no orientation specified, no constraint
     */
    FYBBannerOrientationConstraintNone = 0,

    /**
     *  Banner is optimised for portrait orientation
     */
    FYBBannerOrientationConstraintPortrait,

    /**
     *  Banner is optimised for landscape orientation
     */
    FYBBannerOrientationConstraintLandscape
};


/**
 *  Banner size with constraints
 */
@interface FYBBannerSize : NSObject

/**
 *  The base width and height of the banner (the final size will be subject to constraints)
 */
@property (nonatomic, assign) CGSize size;

/**
 *  The width constraint
 */
@property (nonatomic, assign) FYBBannerSizeConstraint widthConstraint;

/**
 *  The height constraint
 */
@property (nonatomic, assign) FYBBannerSizeConstraint heightConstraint;

/**
 *  The orientation constraint
 */
@property (nonatomic, assign) FYBBannerOrientationConstraint orientationConstraint;


#pragma mark - Initializer

/**
 *  Convenience initializer for a custom FYBBannerSize for the provided CGSize width fixed size constraints.
 *
 *  @param size The CGSize banner size
 *
 *  @return a FYBBannerSize
 */
- (instancetype)initWithFixedSize:(CGSize)size;

/**
 *  Convenience initializer for a custom FYBBannerSize that spans the full width of the application with a fixed height provided.
 *
 *  @param height The fixed height of the banner
 *
 *  @return a FYBBannerSize
 */
- (instancetype)initWithSmartWidthAndFixedHeight:(CGFloat)height;

/**
 *  Convenience initializer for a custom FYBBannerSize with width and height constraints
 *
 *  @param size The CGSize banner size
 *  @param widthConstraint  The width constraint
 *  @param heightConstraint The height constraint
 *
 *  @return a FYBBannerSize
 */
- (instancetype)initWithSize:(CGSize)size
             widthConstraint:(FYBBannerSizeConstraint)widthConstraint
            heightConstraint:(FYBBannerSizeConstraint)heightConstraint;

/**
 *  Designated initializer for a custom FYBBannerSize with width, height and orientation constraints
 *
 *  @param size The CGSize banner size
 *  @param widthConstraint  The width constraint
 *  @param heightConstraint The height constraint
 *  @param orientationConstraint The orientation constraint
 *
 *  @return a FYBBannerSize
 */
- (instancetype)initWithSize:(CGSize)size
             widthConstraint:(FYBBannerSizeConstraint)widthConstraint
            heightConstraint:(FYBBannerSizeConstraint)heightConstraint
       orientationConstraint:(FYBBannerOrientationConstraint)orientationConstraint NS_DESIGNATED_INITIALIZER;

@end
