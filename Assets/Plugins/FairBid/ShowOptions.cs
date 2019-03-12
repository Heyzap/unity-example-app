//  Copyright 2019 Fyber. All Rights Reserved
//
//  Permission is hereby granted, free of charge, to any person
//  obtaining a copy of this software and associated documentation
//  files (the "Software"), to deal in the Software without
//  restriction, including without limitation the rights to use,
//  copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following
//  conditions:
//
//  The above copyright notice and this permission notice shall be
//  included in all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//  OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
//  HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//  WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//  OTHER DEALINGS IN THE SOFTWARE.
//

namespace FairBid {
    /// <summary>
    /// A set of options that describes how to show an ad.
    /// </summary>
    public class ShowOptions {

        /// <summary>
        /// An identifier for the location of the ad, which you can use to disable the ad from your dashboard.
        /// </summary>
        /// <value>The placement name.</value>
        public string Placement { get; set; }
    }

    /// <summary>
    /// A set of options that describes how to show an rewarded ad.
    /// </summary>
    public class RewardedShowOptions : ShowOptions {
        private const string DEFAULT_REWARDED_INFO = "";

        /// <summary>
        /// When an rewarded video is completed, this string will be sent to your server via our server-to-server callbacks. Set it to anything you want to pass to your server regarding this rewarded video view (i.e.: a username, user ID, level name, etc.), or leave it empty if you don't need to use it / aren't using server callbacks for rewarded video.
        /// </summary>
        public string RewardedInfo {
            get {
                return rewardedInfo;
            }
            set {
                if (value != null) {
                    rewardedInfo = value;
                } else {
                    rewardedInfo = DEFAULT_REWARDED_INFO;
                }
            }
        }
        private string rewardedInfo = DEFAULT_REWARDED_INFO;
    }

    /// <summary>
    /// A set of options that describes how to show a banner ad.
    /// </summary>
    public class BannerShowOptions : ShowOptions {

        /// <summary>
        /// Set `BannerShowOptions.Position` to this value to show ads at the top of the screen.
        /// </summary>
        public const string POSITION_TOP = "top";
        /// <summary>
        /// Set `BannerShowOptions.Position` to this value to show ads at the bottom of the screen.
        /// </summary>
        public const string POSITION_BOTTOM = "bottom";

        private const string DEFAULT_POSITION = POSITION_BOTTOM;

        /// <summary>
        /// Gets or sets the position for a banner ad. Can only be set to `BannerShowOptions.POSITION_TOP` or `BannerShowOptions.POSITION_BOTTOM`.
        /// Guaranteed to be non-null.
        /// </summary>
        /// <value>The position.</value>
        public string Position {
            get {
                return position;
            }
            set {
                if (value == POSITION_TOP || value == POSITION_BOTTOM) {
                    position = value;
                }
            }
        }
        private string position = DEFAULT_POSITION;
    }
}
