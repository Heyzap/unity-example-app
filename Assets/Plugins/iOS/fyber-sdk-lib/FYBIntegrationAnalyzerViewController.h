//
//
// Copyright (c) 2017 Fyber. All rights reserved.
//
//

#import <UIKit/UIKit.h>

/**
 * A view controller which displays information about the FyberSDK and the integrated bundles
 */
@interface FYBIntegrationAnalyzerViewController : UIViewController

/**
 *  Presents a full-screen view controller with information about the FyberSDK and the integrated bundles
 *
 *  @discussion The view controller presented by this method is a visual representation of the FYBIntegrationReport object returned by [FYBIntegrationAnalyzer analyzeWithCompletionHandler:]
 *
 *  @param viewController The view controller on top of which the controller is presented
 */
+ (void)presentFromViewController:(nonnull UIViewController *)viewController;

@end
