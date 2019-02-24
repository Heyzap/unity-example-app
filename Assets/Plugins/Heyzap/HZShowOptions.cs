using UnityEngine;
using System.Collections;

namespace Heyzap {
    /// <summary>
    /// A set of options that describes how to show an ad.
    /// </summary>
    public class HZShowOptions {

        /// <summary>
        /// An identifier for the location of the ad, which you can use to disable the ad from your dashboard.
        /// </summary>
        /// <value>The placement name.</value>
        public string Placement {
            get {
                return placement;
            }
            set {
                placement = value;
            }
        }
        private string placement;
    }

    /// <summary>
    /// A set of options that describes how to show an rewarded ad.
    /// </summary>
    public class HZRewardedShowOptions : HZShowOptions {
        private const string DEFAULT_REWARDED_INFO = "";

        /// <summary>
        /// When an rewarded video is completed, this string will be sent to your server via our server-to-server callbacks. Set it to anything you want to pass to your server regarding this rewarded video view (i.e.: a username, user ID, level name, etc.), or leave it empty if you don't need to use it / aren't using server callbacks for rewarded video.
        /// More information about using this feature can be found at https://developers.heyzap.com/docs/advanced-publishing .
        /// </summary>
        public string RewardedInfo {
            get {
                return rewardedInfo;
            }
            set {
                if (value != null) {
                    rewardedInfo = value;
                } else {
                    rewardedInfo = HZRewardedShowOptions.DEFAULT_REWARDED_INFO;
                }
            }
        }
        private string rewardedInfo = HZRewardedShowOptions.DEFAULT_REWARDED_INFO;
    }

    /// <summary>
    /// A set of options that describes how to show a banner ad.
    /// </summary>
    public class HZBannerShowOptions : HZShowOptions {
        
        /// <summary>
        /// Set `HZBannerShowOptions.Position` to this value to show ads at the top of the screen.
        /// </summary>
        public const string POSITION_TOP = "top";
        /// <summary>
        /// Set `HZBannerShowOptions.Position` to this value to show ads at the bottom of the screen.
        /// </summary>
        public const string POSITION_BOTTOM = "bottom";

        private const string DEFAULT_POSITION = HZBannerShowOptions.POSITION_BOTTOM;

        /// <summary>
        /// Gets or sets the position for a banner ad. Can only be set to `HZBannerShowOptions.POSITION_TOP` or `HZBannerShowOptions.POSITION_BOTTOM`.
        /// Guaranteed to be non-null.
        /// </summary>
        /// <value>The position.</value>
        public string Position {
            get {
                return position;
            }
            set {
                if (value == HZBannerShowOptions.POSITION_TOP || value == HZBannerShowOptions.POSITION_BOTTOM) {
                    position = value;
                }
            }
        }
        private string position = HZBannerShowOptions.DEFAULT_POSITION;
    }
}
