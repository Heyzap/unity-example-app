using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions.Must;
using Heyzap;
using System.Collections;
using System.Collections.Generic;

public class AdManager : MonoBehaviour {
    
    [SerializeField]
    private Text adTagTextField;
    [SerializeField]
    private GameObject bannerControls;
    [SerializeField]
    private GameObject offerwallControls;
    [SerializeField]
    private GameObject standardControls;
    [SerializeField]
    private ScrollingTextArea console;
    [SerializeField]
    private Toggle offerwallCloseOnFirstClickToggle;
    [SerializeField]
    private Text offerwallCurrencyIdTextField;

    private enum AdType {
        Interstitial,
        Video,
        Incentivized,
        Banner,
        Offerwall
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

    public void UpdateLocation(float latitude, float longitude, float horizAcc, float vertAcc, float alt, double timestamp) {
        //HZDemographics.SetUserLocation(latitude, longitude, horizAcc, vertAcc, alt, timestamp);
    }

    void Awake() {
        this.adTagTextField.MustNotBeNull();
        this.bannerControls.MustNotBeNull();
        this.standardControls.MustNotBeNull();
        this.offerwallControls.MustNotBeNull();
        this.console.MustNotBeNull();
    }

    void Start () {
        HeyzapAds.NetworkCallbackListener networkCallbackListener = delegate(string network, string callback) {
            this.console.Append("[" + network + "]: " + callback);
        };

        // HZDemographics.SetUserGender(HZDemographics.Gender.MALE);
        // HZDemographics.SetUserPostalCode("94103");
        // HZDemographics.SetUserHouseholdIncome(100000);
        // HZDemographics.SetUserMaritalStatus(HZDemographics.MaritalStatus.SINGLE);
        // HZDemographics.SetUserEducationLevel(HZDemographics.EducationLevel.BACHELORS_DEGREE);
        // HZDemographics.SetUserBirthDate("1990-08-05");

        // UnityEngine.Debug.Log ("calling loc service");
        // TestLocationService locServ = new TestLocationService();
        // locServ.Start(this.console);

		HeyzapAds.SetNetworkCallbackListener(networkCallbackListener);
		HeyzapAds.ShowDebugLogs();
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

        HZVideoAd.SetDisplayListener(delegate(string adState, string adTag) {
            this.console.Append("VIDEO: " + adState + " Tag : " + adTag);
        });

        HZOfferWallAd.SetDisplayListener(delegate(string adState, string adTag) {
            this.console.Append("OFFERWALL: " + adState + " Tag : " + adTag);
        });

        HZOfferWallAd.SetVirtualCurrencyResponseListener(delegate(VirtualCurrencyResponse response) {
            this.console.Append("OFFERWALL VCS Response: id:" + response.CurrencyID + " name: '" + response.CurrencyName + "' amount : " + response.DeltaOfCurrency + " trans: " + response.LatestTransactionID);
        });

        HZOfferWallAd.SetVirtualCurrencyErrorListener(delegate(string errorMsg) {
            this.console.Append("OFFERWALL VCS Error: " + errorMsg);
        });

        // UI defaults
        this.bannerPosition = HZBannerShowOptions.POSITION_TOP;
        this.SelectedAdType = AdType.Interstitial;

        this.ShowAdTypeControls();
    }

    public void InterstitialSelected(bool selected) {
        if (selected) {
            this.SelectedAdType = AdType.Interstitial;
        }
    }

    public void VideoSelected(bool selected) {
        if (selected) {
            this.SelectedAdType = AdType.Video;
        }
    }

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

    public void OfferwallSelected(bool selected) {
        if (selected) {
            this.SelectedAdType = AdType.Offerwall;
        }
    }

    public void IsAvailableButton() {
        string tag = this.adTag();
        bool available = false;

        switch (this.SelectedAdType) {
        case AdType.Interstitial:
            available = HZInterstitialAd.IsAvailable(tag);
            break;
        case AdType.Video:
            available = HZVideoAd.IsAvailable(tag);
            break;
        case AdType.Incentivized:
            available = HZIncentivizedAd.IsAvailable(tag);
            break;
        case AdType.Banner:
            // Not applicable
            break;
        case AdType.Offerwall:
            available = HZOfferWallAd.IsAvailable(tag);
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
        incentivizedOptions.IncentivizedInfo = "test app incentivized info!";

        HZBannerShowOptions bannerOptions = new HZBannerShowOptions();
        bannerOptions.Tag = tag;
        bannerOptions.Position = this.bannerPosition;

        HZOfferWallShowOptions offerwallOptions = new HZOfferWallShowOptions();
        offerwallOptions.ShouldCloseAfterFirstClick = offerwallCloseOnFirstClickToggle.isOn;
        offerwallOptions.Tag = tag;

        this.console.Append("Showing " + this.SelectedAdType.ToString() + " with tag: " + tag);
        switch (this.SelectedAdType) {
            case AdType.Interstitial:
                HZInterstitialAd.ShowWithOptions(showOptions);
                break;
            case AdType.Video:
                HZVideoAd.ShowWithOptions(showOptions);
                break;
            case AdType.Incentivized:
                HZIncentivizedAd.ShowWithOptions(incentivizedOptions);
                break;
            case AdType.Banner:
                HZBannerAd.ShowWithOptions(bannerOptions);
                break;
            case AdType.Offerwall:
                HZOfferWallAd.ShowWithOptions(offerwallOptions);
                break;
        }
    }

    public void FetchButton() {
        string tag = this.adTag();
        this.console.Append("Fetching " + this.SelectedAdType.ToString() + " with tag: " + tag);
        switch(this.SelectedAdType) {
            case AdType.Interstitial:
                HZInterstitialAd.Fetch(tag);
                break;
            case AdType.Video:
                HZVideoAd.Fetch(tag);
                break;
            case AdType.Incentivized:
                HZIncentivizedAd.Fetch(tag);
                break;
            case AdType.Offerwall:
                HZOfferWallAd.Fetch(tag);
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

    public void RemoteDataButton() {
        this.console.Append("Remote data: " + HeyzapAds.GetRemoteData());
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

    public void AcceptGdprButton()
	{
		this.console.Append("Accepting GDPR");
		HeyzapAds.SetGdprConsent(true);
	}

    public void RejectGdprButton()
    {
        this.console.Append("Rejecting GDPR");
        HeyzapAds.SetGdprConsent(false);
    }

    public void SetGdprDataA()
    {
        this.console.Append("Set GDPR Data A: ");
        SetGdprData(GetGdprConsentDataA());
    }

    public void SetGdprDataB()
    {
        this.console.Append("Set GDPR Data B: ");
        SetGdprData(GetGdprConsentDataB());
    }

    public void ClearGdprData()
    {
        this.console.Append("Clearing GDPR Data");
        HeyzapAds.ClearGdprConsentData();
    }

    private void SetGdprData(Dictionary<string, string> gdprConsentData)
    {
        string gdprConsentDataAsString = gdprConsentData != null ? gdprConsentData.ToString() : "null";
        this.console.Append(gdprConsentDataAsString);
        HeyzapAds.SetGdprConsentData(gdprConsentData);
    }

    private Dictionary<string, string> GetGdprConsentDataA()
    {

        // return default data a dicitonary
        var data = new Dictionary<string, string>();

        data["key_1"] = "value_1";
        data["key_2"] = "value_2";
        data["key_3"] = null;
        return data;
    }

    private Dictionary<string, string> GetGdprConsentDataB()
    {
        
        // return default data a dicitonary
        var data = new Dictionary<string, string>();
        data["key_1"] = "value_1";
        return data;
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

    public void VCSRequestButton() {
        HZOfferWallAd.RequestDeltaOfCurrency(this.currencyId());
    }

    public void ShowMediationTest() {
        this.console.Append("Showing mediation test suite");
        HeyzapAds.ShowMediationTestSuite();
    }

    private void ShowAdTypeControls() {
        if (this.SelectedAdType == AdType.Banner) {
            this.bannerControls.SetActive(true);
            this.standardControls.SetActive(false);
            this.offerwallControls.SetActive(false);
        } else if (this.SelectedAdType == AdType.Offerwall) {
            this.bannerControls.SetActive(false);
            this.offerwallControls.SetActive(true);
            this.standardControls.SetActive(false);
        } else {
            this.bannerControls.SetActive(false);
            this.offerwallControls.SetActive(false);
            this.standardControls.SetActive(true);
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

    private string currencyId() {
        string currencyId = this.offerwallCurrencyIdTextField.text;
        if (currencyId == null || tag.Trim().Length == 0) {
            return null;
        } else {
            return currencyId;
        }
    }
}
