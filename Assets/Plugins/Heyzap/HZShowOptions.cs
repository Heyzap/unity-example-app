using UnityEngine;
using System.Collections;

namespace Heyzap {
    /// <summary>
    /// A set of options that describes how to show an ad.
    /// </summary>
    public class HZShowOptions {
        
        /// <summary>
        /// An identifier for the location of the ad, which you can use to disable the ad from your dashboard. If not specified the tag "default" is always used.
        /// Guaranteed to be non-null - will be set to the default value if the setter is called with `null`.
        /// </summary>
        /// <value>The tag.</value>
        public string Tag {
            get {
                return tag;
            }
            set {
                if (value != null) {
                    tag = value;
                } else {
                    tag = HeyzapAds.DEFAULT_TAG;
                }
            }
        }
        private string tag = HeyzapAds.DEFAULT_TAG;     
    }

    /// <summary>
    /// A set of options that describes how to show an incentivized ad.
    /// </summary>
    public class HZIncentivizedShowOptions : HZShowOptions {
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