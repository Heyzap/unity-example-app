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

        /// <summary>
        /// Set `HZBannerShowOptions.SelectedAdMobSize` to one of these to show ads of the selected size when AdMob is chosen by our mediation to show a banner ad.
        /// </summary>
        public enum AdMobSize {
          /// <summary>
          /// Portrait-specific on iOS, same as SMART_BANNER_LANDSCAPE on Android
          /// </summary>
          SMART_BANNER,
          /// <summary>
          /// Landscape-specific on iOS, same as SMART_BANNER on Android
          /// </summary>
          SMART_BANNER_LANDSCAPE,
          BANNER,
          FULL_BANNER,
          LARGE_BANNER,
          /// <summary>
          /// Not available on iOS, will use LARGE_BANNER instead internally
          /// </summary>
          MEDIUM_RECTANGLE,
          LEADERBOARD,
          /// <summary>
          /// Not available on iOS, will use FULL_BANNER instead internally
          /// </summary>
          WIDE_SKYSCRAPER
        }

        /// <summary>
        /// Set `HZBannerShowOptions.SelectedFacebookSize` to one of these to show ads of the selected size when AdMob is chosen by our mediation to show a banner ad.
        /// </summary>
        public enum FacebookSize {
          BANNER_320_50,
          BANNER_HEIGHT_50,
          BANNER_HEIGHT_90,
          /// <summary>
          /// Not available on iOS, will use BANNER_HEIGHT_90 instead internally
          /// </summary>
          BANNER_RECTANGLE_250
        }

        /// <summary>
        /// Set `HZBannerShowOptions.SelectedInmobiSize` to one of these to show ads of the selected size when Inmobi is chosen by our mediation to show a banner ad.
        /// </summary>
        public enum InmobiSize {
          BANNER_320_50,
          BANNER_320_48,
          BANNER_300_250,
          BANNER_120_600,
          BANNER_468_60,
          BANNER_728_90,
          BANNER_1024_768
        }
        
        private const string        DEFAULT_POSITION            = HZBannerShowOptions.POSITION_BOTTOM;
        private const AdMobSize     DEFAULT_BANNER_SIZE_ADMOB   = HZBannerShowOptions.AdMobSize.SMART_BANNER;
        private const FacebookSize  DEFAULT_BANNER_SIZE_FAN     = HZBannerShowOptions.FacebookSize.BANNER_HEIGHT_50;
        private const InmobiSize    DEFAULT_BANNER_SIZE_INMOBI  = HZBannerShowOptions.InmobiSize.BANNER_320_50;

        
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

        public AdMobSize SelectedAdMobSize {
          get {
            return selectedAdMobSize;
          }
          set {
            if (System.Enum.IsDefined(typeof(AdMobSize), value)) {
              selectedAdMobSize = value;
            }
          }
        }
        private AdMobSize selectedAdMobSize = HZBannerShowOptions.DEFAULT_BANNER_SIZE_ADMOB;

        public FacebookSize SelectedFacebookSize {
          get {
            return selectedFacebookSize;
          }
          set {
            if (System.Enum.IsDefined(typeof(FacebookSize), value)) {
              selectedFacebookSize = value;
            }
          }
        }
        private FacebookSize selectedFacebookSize = HZBannerShowOptions.DEFAULT_BANNER_SIZE_FAN;

        public InmobiSize SelectedInmobiSize {
          get {
            return selectedInmobiSize;
          }
          set {
            if (System.Enum.IsDefined(typeof(InmobiSize), value)) {
              selectedInmobiSize = value;
            }
          }
        }
        private InmobiSize selectedInmobiSize = HZBannerShowOptions.DEFAULT_BANNER_SIZE_INMOBI;
    }
}
