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
        /// <value>The tag.</value>
        public string Tag {
            get {
                return tag;
            }
            set {
                tag = value;
            }
        }
        private string tag;
    }

    /// <summary>
    /// A set of options that describes how to show an incentivized ad.
    /// </summary>
    public class HZIncentivizedShowOptions : HZShowOptions {
        private const string DEFAULT_INCENTIVIZED_INFO = "";

        /// <summary>
        /// When an incentivized video is completed, this string will be sent to your server via our server-to-server callbacks. Set it to anything you want to pass to your server regarding this incentivized video view (i.e.: a username, user ID, level name, etc.), or leave it empty if you don't need to use it / aren't using server callbacks for incentivized video.
        /// More information about using this feature can be found at https://developers.heyzap.com/docs/advanced-publishing .
        /// </summary>
        public string IncentivizedInfo {
            get {
                return incentivizedInfo;
            }
            set {
                if (value != null) {
                    incentivizedInfo = value;
                } else {
                    incentivizedInfo = HZIncentivizedShowOptions.DEFAULT_INCENTIVIZED_INFO;
                }
            }
        }
        private string incentivizedInfo = HZIncentivizedShowOptions.DEFAULT_INCENTIVIZED_INFO;
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

    /// <summary>
    /// A set of options that describes how to show an offerwall ad.
    /// </summary>
    public class HZOfferWallShowOptions : HZShowOptions {
        private const bool DEFAULT_SHOULD_CLOSE_AFTER_FIRST_CLICK = true;

        /// <summary>
        /// When an incentivized video is completed, this string will be sent to your server via our server-to-server callbacks. Set it to anything you want to pass to your server regarding this incentivized video view (i.e.: a username, user ID, level name, etc.), or leave it empty if you don't need to use it / aren't using server callbacks for incentivized video.
        /// More information about using this feature can be found at https://developers.heyzap.com/docs/advanced-publishing .
        /// </summary>
        public bool ShouldCloseAfterFirstClick {
            get {
                return shouldCloseAfterFirstClick;
            }
            set {
                shouldCloseAfterFirstClick = value;
            }
        }
        private bool shouldCloseAfterFirstClick = HZOfferWallShowOptions.DEFAULT_SHOULD_CLOSE_AFTER_FIRST_CLICK;
    }
}
