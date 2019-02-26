using FairBid;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
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
            console.Append("AdType: " + value.ToString());
            ShowAdTypeControls();
        }
    }

    private string bannerPosition;

    public void UpdateLocation(float latitude, float longitude, float horizAcc, float vertAcc, float alt, double timestamp) {
        //Demographics.SetUserLocation(latitude, longitude, horizAcc, vertAcc, alt, timestamp);
    }

    void Awake() {
        Assert.IsNotNull(placementTextField);
        Assert.IsNotNull(bannerControls);
        Assert.IsNotNull(standardControls);
        Assert.IsNotNull(console);
    }

    void Start () {
        FairBidSDK.NetworkCallbackListener networkCallbackListener = delegate(string network, string callback) {
            console.Append("[" + network + "]: " + callback);
        };

        // Demographics.SetUserGender(Demographics.Gender.MALE);
        // Demographics.SetUserPostalCode("94103");
        // Demographics.SetUserHouseholdIncome(100000);
        // Demographics.SetUserMaritalStatus(Demographics.MaritalStatus.SINGLE);
        // Demographics.SetUserEducationLevel(Demographics.EducationLevel.BACHELORS_DEGREE);
        // Demographics.SetUserBirthDate("1990-08-05");

        // UnityEngine.Debug.Log ("calling loc service");
        // TestLocationService locServ = new TestLocationService();
        // locServ.Start(console);

        FairBidSDK.SetNetworkCallbackListener(networkCallbackListener);
        FairBidSDK.ShowDebugLogs();
        FairBidSDK.Start("ENTER_YOUR_PUBLISHER_ID_HERE", FairBidSDK.FLAG_NO_OPTIONS);

        Banner.SetDisplayListener(delegate(string adState, string placement) {
            console.Append("BANNER: " + adState + " Placement : " + placement);
            if (adState == "loaded") {
                Rect dimensions = new Rect();
                Banner.GetCurrentBannerDimensions(out dimensions);
                console.Append(string.Format("    (x,y): ({0},{1}) - WxH: {2}x{3}", dimensions.x, dimensions.y, dimensions.width, dimensions.height));
            }
        });

        Interstitial.SetDisplayListener(delegate(string adState, string placement) {
            console.Append("INTERSTITIAL: " + adState + " Placement : " + placement);
        });

        Rewarded.SetDisplayListener(delegate(string adState, string placement) {
            console.Append("REWARDED: " + adState + " Placement : " + placement);
        });

        // UI defaults
        bannerPosition = BannerShowOptions.POSITION_TOP;
        SelectedAdType = AdType.Interstitial;

        ShowAdTypeControls();
    }

    public void InterstitialSelected(bool selected) {
        if (selected) {
            SelectedAdType = AdType.Interstitial;
        }
    }

    public void RewardedSelected(bool selected) {
        if (selected) {
            SelectedAdType = AdType.Rewarded;
        }
    }

    public void BannerSelected(bool selected) {
        if (selected) {
            SelectedAdType = AdType.Banner;
        }
    }

    public void IsAvailableButton() {
        string placementName = GetPlacementName();
        bool available = false;

        switch (SelectedAdType) {
            case AdType.Interstitial:
                available = Interstitial.IsAvailable(placementName);
                break;
            case AdType.Rewarded:
                available = Rewarded.IsAvailable(placementName);
                break;
            case AdType.Banner:
                // Not applicable
                break;
        }

        string availabilityMessage = available ? "available" : "not available";
        console.Append(SelectedAdType.ToString() + " with placement: " + placementName + " is " + availabilityMessage);
    }

    public void ShowButton() {
        string placementName = GetPlacementName();
        if (placementName == "") {
            console.Append("Cannot show without providing a valid placement name");
        } else {
            ShowOptions showOptions = new ShowOptions
            {
                Placement = placementName
            };

            RewardedShowOptions rewardedOptions = new RewardedShowOptions
            {
                Placement = placementName,
                RewardedInfo = "test app rewarded info!"
            };

            BannerShowOptions bannerOptions = new BannerShowOptions
            {
                Placement = placementName,
                Position = bannerPosition
            };

            console.Append("Showing " + SelectedAdType.ToString() + " with placement: " + placementName);
            switch (SelectedAdType) {
                case AdType.Interstitial:
                    Interstitial.ShowWithOptions(showOptions);
                    break;
                case AdType.Rewarded:
                    Rewarded.ShowWithOptions(rewardedOptions);
                    break;
                case AdType.Banner:
                    Banner.ShowWithOptions(bannerOptions);
                    break;
            }
        }
    }

    public void RequestButton() {
        string placementName = GetPlacementName();
        if (placementName == "") {
            console.Append("Cannot request without providing a valid placement name");
        } else {
            console.Append("Requesting " + SelectedAdType.ToString() + " with placement : " + placementName);
            switch (SelectedAdType) {
                case AdType.Interstitial:
                    Interstitial.Request(placementName);
                    break;
                case AdType.Rewarded:
                    Rewarded.Request(placementName);
                    break;
            }
        }
    }

    public void HideButton() {
        if (SelectedAdType == AdType.Banner) {
            console.Append("Hiding Banner");
            Banner.Hide();
        }
    }

    public void DestroyButton() {
        if (SelectedAdType == AdType.Banner) {
            console.Append("Destroying Banner");
            Banner.Destroy();
        }
    }

    public void DebugLogSwitch(bool on) {
        if (on) {
            console.Append("Enabling debug logging");
            FairBidSDK.ShowDebugLogs();
        } else {
            console.Append("Disabling debug logging");
            FairBidSDK.HideDebugLogs();
        }
	}

    public void AcceptGdprButton() {
        console.Append("Accepting GDPR");
        FairBidSDK.SetGdprConsent(true);
	}

    public void RejectGdprButton() {
        console.Append("Rejecting GDPR");
        FairBidSDK.SetGdprConsent(false);
    }

    public void SetGdprDataA() {
        console.Append("Set GDPR Data A: ");
        SetGdprData(GetGdprConsentDataA());
    }

    public void SetGdprDataB() {
        console.Append("Set GDPR Data B: ");
        SetGdprData(GetGdprConsentDataB());
    }

    public void ClearGdprData() {
        console.Append("Clearing GDPR Data");
        FairBidSDK.ClearGdprConsentData();
    }

    private void SetGdprData(Dictionary<string, string> gdprConsentData) {
        string gdprConsentDataAsString = gdprConsentData != null ? gdprConsentData.ToString() : "null";
        console.Append(gdprConsentDataAsString);
        FairBidSDK.SetGdprConsentData(gdprConsentData);
    }

    private Dictionary<string, string> GetGdprConsentDataA() {
        var data = new Dictionary<string, string>();

        data["key_A1"] = "value_A1";
        data["key_A2"] = "value_A2";
        data["key_A3"] = null;
        return data;
    }

    private Dictionary<string, string> GetGdprConsentDataB() {
        var data = new Dictionary<string, string>();
        data["key_B1"] = "value_B1";
        return data;
    }    

    public void BannerPositionTop(bool selected) {
        if (selected) {
            bannerPosition = BannerShowOptions.POSITION_TOP;
        }
    }

    public void BannerPositionBottom(bool selected) {
        if (selected) {
            bannerPosition = BannerShowOptions.POSITION_BOTTOM;
        }
    }

    public void ShowTestSuite() {
        console.Append("Showing Test Suite");
        FairBidSDK.ShowTestSuite();
    }

    private void ShowAdTypeControls() {
        if (SelectedAdType == AdType.Banner) {
            bannerControls.SetActive(true);
            standardControls.SetActive(false);
        } else {
            bannerControls.SetActive(false);
            standardControls.SetActive(true);
        }
    }

    private string GetPlacementName() {
        string placementName = placementTextField.text;
        if (placementName == null || placementName.Trim().Length == 0) {
            switch (SelectedAdType) {
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
