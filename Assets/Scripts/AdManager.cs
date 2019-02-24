using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using Heyzap;
using System.Collections.Generic;

public class AdManager : MonoBehaviour {
    
    [SerializeField]
    private Text placementTextField;
    [SerializeField]
    private GameObject bannerControls;
    [SerializeField]
    private GameObject standardControls;
    [SerializeField]
    private ScrollingTextArea console;

    private enum AdType {
        Interstitial,
        Rewarded,
        Banner
    }

    private AdType selectedAdType;
    private AdType SelectedAdType {
        get { return selectedAdType; }
        set {
            selectedAdType = value;
            this.console.Append("AdType: " + value.ToString());
            ShowAdTypeControls();
        }
    }

    private string bannerPosition;

    public void UpdateLocation(float latitude, float longitude, float horizAcc, float vertAcc, float alt, double timestamp) {
        //HZDemographics.SetUserLocation(latitude, longitude, horizAcc, vertAcc, alt, timestamp);
    }

    void Awake()
    {
        Assert.IsNotNull(placementTextField);
        Assert.IsNotNull(bannerControls);
        Assert.IsNotNull(standardControls);
        Assert.IsNotNull(console);
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

        HZBannerAd.SetDisplayListener(delegate(string adState, string placement) {
            this.console.Append("BANNER: " + adState + " Placement : " + placement);
            if (adState == "loaded") {
                Rect dimensions = new Rect();
                HZBannerAd.GetCurrentBannerDimensions(out dimensions);
                this.console.Append(string.Format("    (x,y): ({0},{1}) - WxH: {2}x{3}", dimensions.x, dimensions.y, dimensions.width, dimensions.height));
            }
        });

        HZInterstitialAd.SetDisplayListener(delegate(string adState, string placement) {
            this.console.Append("INTERSTITIAL: " + adState + " Placement : " + placement);
        });

        HZRewardedAd.SetDisplayListener(delegate(string adState, string placement) {
            this.console.Append("REWARDED: " + adState + " Placement : " + placement);
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

    public void RewardedSelected(bool selected) {
        if (selected) {
            this.SelectedAdType = AdType.Rewarded;
        }
    }

    public void BannerSelected(bool selected) {
        if (selected) {
            this.SelectedAdType = AdType.Banner;
        }
    }

    public void IsAvailableButton() {
        string placementName = this.GetPlacementName();
        bool available = false;

        switch (this.SelectedAdType) {
            case AdType.Interstitial:
                available = HZInterstitialAd.IsAvailable(placementName);
                break;
            case AdType.Rewarded:
                available = HZRewardedAd.IsAvailable(placementName);
                break;
            case AdType.Banner:
                // Not applicable
                break;
        }

        string availabilityMessage = available ? "available" : "not available";
        console.Append(this.SelectedAdType.ToString() + " with placement: " + placementName + " is " + availabilityMessage);
    }

    public void ShowButton() {
        string placementName = this.GetPlacementName();
        if (placementName == "") {
            console.Append("Cannot show without providing a valid placement name");
        } else {
            HZShowOptions showOptions = new HZShowOptions
            {
                Placement = placementName
            };

            HZRewardedShowOptions rewardedOptions = new HZRewardedShowOptions
            {
                Placement = placementName,
                RewardedInfo = "test app rewarded info!"
            };

            HZBannerShowOptions bannerOptions = new HZBannerShowOptions
            {
                Placement = placementName,
                Position = this.bannerPosition
            };

            console.Append("Showing " + this.SelectedAdType.ToString() + " with placement: " + placementName);
            switch (this.SelectedAdType)
            {
                case AdType.Interstitial:
                    HZInterstitialAd.ShowWithOptions(showOptions);
                    break;
                case AdType.Rewarded:
                    HZRewardedAd.ShowWithOptions(rewardedOptions);
                    break;
                case AdType.Banner:
                    HZBannerAd.ShowWithOptions(bannerOptions);
                    break;
            }
        }
    }

    public void RequestButton() {
        string placementName = this.GetPlacementName();
        if (placementName == "") {
            console.Append("Cannot request without providing a valid placement name");
        } else {
            console.Append("Requesting " + this.SelectedAdType.ToString() + " with placement : " + placementName);
            switch (this.SelectedAdType) {
                case AdType.Interstitial:
                    HZInterstitialAd.Fetch(placementName);
                    break;
                case AdType.Rewarded:
                    HZRewardedAd.Request(placementName);
                    break;
            }
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

        data["key_A1"] = "value_A1";
        data["key_A2"] = "value_A2";
        data["key_A3"] = null;
        return data;
    }

    private Dictionary<string, string> GetGdprConsentDataB()
    {
        // return default data a dicitonary
        var data = new Dictionary<string, string>();
        data["key_B1"] = "value_B1";
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

    public void ShowTestSuite() {
        this.console.Append("Showing Test Suite");
        HeyzapAds.ShowTestSuite();
    }

    private void ShowAdTypeControls() {
        if (this.SelectedAdType == AdType.Banner) {
            this.bannerControls.SetActive(true);
            this.standardControls.SetActive(false);
        } else {
            this.bannerControls.SetActive(false);
            this.standardControls.SetActive(true);
        }
    }

    private string GetPlacementName() {
        string placementName = this.placementTextField.text;
        if (placementName == null || placementName.Trim().Length == 0) {
            switch (this.SelectedAdType)
            {
                case AdType.Interstitial:
                    return "interstitial";
                case AdType.Rewarded:
                    return "rewarded";
                case AdType.Banner:
                    return "banner";
            }
            return "";
        } else {
            return placementName;
        }
    }
}
