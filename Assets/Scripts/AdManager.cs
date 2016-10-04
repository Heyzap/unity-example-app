using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions.Must;
using System.Collections;
using System;
using Heyzap;
using FyberPlugin;

public class AdManager : MonoBehaviour {
    
    [SerializeField]
    private Text adTagTextField;
    [SerializeField]
    private GameObject bannerControls;
    [SerializeField]
    private GameObject fullscreenControls;
    [SerializeField]
    private ScrollingTextArea console;

    private enum AdType {
        Interstitial,
        // Video,
        Incentivized,
        Banner,
        OfferWall
    }

    private AdType _selectedAdType;
    private AdType SelectedAdType {
        get { return _selectedAdType; }
        set {
            _selectedAdType = value;
            this.console.Append("AdType: " + value.ToString());
            ShowAdTypeControls();
        }
    }

    private string bannerPosition;

    private FyberPlugin.Ad offerWall;

    void Awake() {
        this.adTagTextField.MustNotBeNull();
        this.bannerControls.MustNotBeNull();
        this.fullscreenControls.MustNotBeNull();
        this.console.MustNotBeNull();
    }

    void Start () {
        // HeyzapAds.NetworkCallbackListener networkCallbackListner = delegate(string network, string callback) {
        //     this.console.Append("[" + network + "]: " + callback);
        // };

        // HeyzapAds.SetNetworkCallbackListener(networkCallbackListner);

        #if UNITY_ANDROID
        if(Application.platform == RuntimePlatform.Android){
            AndroidJNIHelper.debug = false;
            using (AndroidJavaClass jc = new AndroidJavaClass("com.heyzap.sdk.extensions.unity3d.UnityHelper")) {
                jc.CallStatic("forceTestDevice");
            }
        }
        #endif

        HeyzapAds.Start("ENTER_YOUR_PUBLISHER_ID_HERE", HeyzapAds.FLAG_NO_OPTIONS);

        HZBannerAd.SetDisplayListener(delegate(string adState, string adTag) {
            this.console.Append("BANNER: " + adState + " Tag : " + adTag);
            if (adState == "loaded") {
                Rect dimensions = new Rect();
                HZBannerAd.GetCurrentBannerDimensions(out dimensions);
                this.console.Append(string.Format("    (x,y): ({0},{1}) - WxH: {2}x{3}", dimensions.x, dimensions.y, dimensions.width, dimensions.height));
            }
        });

        HZInterstitialAd.SetDisplayListener(delegate(string adState, string adTag) {
            this.console.Append("INTERSTITIAL: " + adState + " Tag : " + adTag);
        });

        HZIncentivizedAd.SetDisplayListener(delegate(string adState, string adTag) {
            this.console.Append("INCENTIVIZED: " + adState + " Tag : " + adTag);
        });

        // HZVideoAd.SetDisplayListener(delegate(string adState, string adTag) {
        //     this.console.Append("VIDEO: " + adState + " Tag : " + adTag);
        // });

        // UI defaults
        this.bannerPosition = HZBannerShowOptions.POSITION_TOP;
        this.SelectedAdType = AdType.Interstitial;
        HeyzapAds.HideDebugLogs(); // no-op
    }

    void OnEnable() {
        this.console.Append("OFFERWALL: registering for notifications");

        FyberCallback.AdAvailable += OnAdAvailable;
        FyberCallback.AdNotAvailable += OnAdNotAvailable;   
        FyberCallback.RequestFail += OnRequestFail; 

        FyberCallback.VirtualCurrencySuccess += OnCurrencyResponse;
        FyberCallback.VirtualCurrencyError += OnCurrencyErrorResponse;
    }

    void OnDisable() {
        this.console.Append("OFFERWALL: UNregistering for notifications");

        FyberCallback.AdAvailable -= OnAdAvailable;
        FyberCallback.AdNotAvailable -= OnAdNotAvailable;   
        FyberCallback.RequestFail -= OnRequestFail; 
    
        FyberCallback.VirtualCurrencySuccess -= OnCurrencyResponse;
        FyberCallback.VirtualCurrencyError -= OnCurrencyErrorResponse;
    }

    private void OnAdAvailable(FyberPlugin.Ad ad) {
        switch(ad.AdFormat) {
            case AdFormat.OFFER_WALL:
                this.console.Append("OFFERWALL: available");
                this.offerWall = ad;
                break;
        }
    }

    private void OnAdNotAvailable(FyberPlugin.AdFormat adFormat) {
        switch(adFormat) {
            case AdFormat.OFFER_WALL:
                this.console.Append("OFFERWALL: not available");
                this.offerWall = null;
                break;
        }
    }

    private void OnRequestFail(FyberPlugin.RequestError error) {
        this.console.Append("OFFERWALL: request failed: " + error.Description);
    }

    private void OnCurrencyResponse(VirtualCurrencyResponse response) {
        this.console.Append("Delta of coins: " + response.DeltaOfCoins.ToString() +
                                ". Transaction ID: " + response.LatestTransactionId +
                                ".\nCurreny ID: " + response.CurrencyId +
                                ". Currency Name: " + response.CurrencyName);
    }

    private void OnCurrencyErrorResponse(VirtualCurrencyErrorResponse vcsError) {
        this.console.Append(String.Format("Delta of coins request failed.\n" +
                            "Error Type: {0}\nError Code: {1}\nError Message: {2}",
                            vcsError.Type, vcsError.Code, vcsError.Message));
    }



    public void InterstitialSelected(bool selected) {
        if (selected) {
            this.SelectedAdType = AdType.Interstitial;
        }
    }

    // public void VideoSelected(bool selected) {
    //     if (selected) {
    //         this.SelectedAdType = AdType.Video;
    //     }
    // }

    public void IncentivizedSelected(bool selected) {
        if (selected) {
            this.SelectedAdType = AdType.Incentivized;
        }
    }

    public void BannerSelected(bool selected) {
        if (selected) {
            this.SelectedAdType = AdType.Banner;
        }
    }

    public void OfferWallSelected(bool selected) {
        if (selected) {
            this.SelectedAdType = AdType.OfferWall;
        }
    }

    public void IsAvailableButton() {
        string tag = this.adTag();
        bool available = false;

        switch (this.SelectedAdType) {
        case AdType.Interstitial:
            available = HZInterstitialAd.IsAvailable(tag);
            break;
        // case AdType.Video:
        //     available = HZVideoAd.IsAvailable(tag);
        //     break;
        case AdType.Incentivized:
            available = HZIncentivizedAd.IsAvailable(tag);
            break;
        case AdType.Banner:
            // Not applicable
            break;
        case AdType.OfferWall:
            available = (this.offerWall != null);
            break;
        }

        string availabilityMessage = available ? "available" : "not available";
        this.console.Append(this.SelectedAdType.ToString() + " with tag: " + tag + " is " + availabilityMessage);
    }

    public void ShowButton() {
        string tag = this.adTag();

        HZShowOptions showOptions = new HZShowOptions();
        showOptions.Tag = tag;

        HZIncentivizedShowOptions incentivizedOptions = new HZIncentivizedShowOptions();
        incentivizedOptions.Tag = tag;
        // incentivizedOptions.IncentivizedInfo = "test app incentivized info!";

        HZBannerShowOptions bannerOptions = new HZBannerShowOptions();
        bannerOptions.Tag = tag;
        bannerOptions.Position = this.bannerPosition;

        this.console.Append("Showing " + this.SelectedAdType.ToString() + " with tag: " + tag);
        switch (this.SelectedAdType) {
            case AdType.Interstitial:
                HZInterstitialAd.ShowWithOptions(showOptions);
                break;
            // case AdType.Video:
            //     HZVideoAd.ShowWithOptions(showOptions);
            //     break;
            case AdType.Incentivized:
                HZIncentivizedAd.ShowWithOptions(incentivizedOptions);
                break;
            case AdType.Banner:
                HZBannerAd.ShowWithOptions(bannerOptions);
                break;
            case AdType.OfferWall:
                if (this.offerWall != null) {
                    this.offerWall.Start();
                    this.offerWall = null;
                } else {
                    this.console.Append("OfferWall needs to be fetched still.");
                }
                break;
        }
    }

    public void RequestOfferWall() {
        OfferWallRequester.Create()
            // optional method chaining
            //.AddParameter("key", "value")
            //.AddParameters(dictionary)
            //.WithPlacementId(placementId)
            // configure ofw behaviour:
            //.CloseOnRedirect(true)
            // you don't need to add a callback if you are using delegates
            //.WithCallback(requestCallback)
            //requesting the ad
            .Request();
    }

    public void FetchButton() {
        string tag = this.adTag();
        this.console.Append("Fetching " + this.SelectedAdType.ToString() + " with tag: " + tag);
        switch(this.SelectedAdType) {
            case AdType.Interstitial:
                HZInterstitialAd.Fetch(tag);
                break;
            // case AdType.Video:
            //     HZVideoAd.Fetch(tag);
            //     break;
            case AdType.Incentivized:
                HZIncentivizedAd.Fetch(tag);
                break;
            case AdType.OfferWall:
                RequestOfferWall();
                break;
        }
    }

    public void HideButton() {
        if (this.SelectedAdType == AdType.Banner) {
            this.console.Append("Hiding Banner");
            HZBannerAd.Hide();
        }
    }

    public void DestroyButton() {
        if (this.SelectedAdType == AdType.Banner) {
            this.console.Append("Destroying Banner");
            HZBannerAd.Destroy();
        }
    }

    public void VCSButton() {
        VirtualCurrencyRequester.Create()
        // Overrideing currency Id (when you have more than one currency)
        //.ForCurrencyId(currencyId)
        // Changing the GUI notification behaviour for when the user is rewarded
        //.NotifyUserOnReward(true)
        .Request();
    }

    public void DebugLogSwitch(bool on) {
        if (on) {
            this.console.Append("Enabling debug logging");
            HeyzapAds.ShowDebugLogs();
        } else {
            this.console.Append("Disabling debug logging");
            HeyzapAds.HideDebugLogs();
        }
    }

    public void BannerPositionTop(bool selected) {
        if (selected) {
            this.bannerPosition = HZBannerShowOptions.POSITION_TOP;
        }
    }

    public void BannerPositionBottom(bool selected) {
        if (selected) {
            this.bannerPosition = HZBannerShowOptions.POSITION_BOTTOM;
        }
    }

    public void ShowMediationTest() {
        this.console.Append("Showing mediation test suite");
        HeyzapAds.ShowMediationTestSuite();
    }

    private void ShowAdTypeControls() {
        if (this.SelectedAdType == AdType.Banner) {
            this.bannerControls.SetActive(true);
            this.fullscreenControls.SetActive(false);
        } else if (this.SelectedAdType == AdType.OfferWall) {
            this.fullscreenControls.SetActive(true);
            this.bannerControls.SetActive(false);
        } else {
            this.fullscreenControls.SetActive(true);
            this.bannerControls.SetActive(false);
        }
    }

    private string adTag() {
        string tag = this.adTagTextField.text;
        if (tag == null || tag.Trim().Length == 0) {
            return "default";
        } else {
            return tag;
        }
    }
}
