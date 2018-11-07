using UnityEngine;
using UnityEngine.UI;
using Heyzap;
using System.Collections;
using System.Collections.Generic;

public class AdManager : MonoBehaviour
{
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

    private enum AdType
    {
        Interstitial,
        Video,
        Incentivized,
        Banner,
        Offerwall
    }

    private AdType _selectedAdType;
    private AdType SelectedAdType
    {
        get { return _selectedAdType; }
        set
        {
            _selectedAdType = value;
            console.Append("AdType: " + value.ToString());
            ShowAdTypeControls();
        }
    }

    private string bannerPosition;

    public void UpdateLocation(float latitude, float longitude, float horizAcc, float vertAcc, float alt, double timestamp)
    {
        //HZDemographics.SetUserLocation(latitude, longitude, horizAcc, vertAcc, alt, timestamp);
    }

    void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(adTagTextField);
        UnityEngine.Assertions.Assert.IsNotNull(bannerControls);
        UnityEngine.Assertions.Assert.IsNotNull(standardControls);
        UnityEngine.Assertions.Assert.IsNotNull(offerwallControls);
        UnityEngine.Assertions.Assert.IsNotNull(console);
    }

    void Start()
    {
        HeyzapAds.NetworkCallbackListener networkCallbackListener = delegate (string network, string callback)
        {
            console.Append("[" + network + "]: " + callback);
        };

        // HZDemographics.SetUserGender(HZDemographics.Gender.MALE);
        // HZDemographics.SetUserPostalCode("94103");
        // HZDemographics.SetUserHouseholdIncome(100000);
        // HZDemographics.SetUserMaritalStatus(HZDemographics.MaritalStatus.SINGLE);
        // HZDemographics.SetUserEducationLevel(HZDemographics.EducationLevel.BACHELORS_DEGREE);
        // HZDemographics.SetUserBirthDate("1990-08-05");

        // UnityEngine.Debug.Log ("calling loc service");
        // TestLocationService locServ = new TestLocationService();
        // locServ.Start(console);

        HeyzapAds.SetNetworkCallbackListener(networkCallbackListener);
        HeyzapAds.ShowDebugLogs();
        HeyzapAds.Start("ENTER_YOUR_PUBLISHER_ID_HERE", HeyzapAds.FLAG_NO_OPTIONS);

        HZBannerAd.SetDisplayListener(delegate (string adState, string adTag)
        {
            console.Append("BANNER: " + adState + " Tag : " + adTag);
            if (adState == "loaded")
            {
                Rect dimensions = new Rect();
                HZBannerAd.GetCurrentBannerDimensions(out dimensions);
                console.Append(string.Format("    (x,y): ({0},{1}) - WxH: {2}x{3}", dimensions.x, dimensions.y, dimensions.width, dimensions.height));
            }
        });

        HZInterstitialAd.SetDisplayListener(delegate (string adState, string adTag)
        {
            console.Append("INTERSTITIAL: " + adState + " Tag : " + adTag);
        });

        HZIncentivizedAd.SetDisplayListener(delegate (string adState, string adTag)
        {
            console.Append("INCENTIVIZED: " + adState + " Tag : " + adTag);
        });

        HZVideoAd.SetDisplayListener(delegate (string adState, string adTag)
        {
            console.Append("VIDEO: " + adState + " Tag : " + adTag);
        });

        HZOfferWallAd.SetDisplayListener(delegate (string adState, string adTag)
        {
            console.Append("OFFERWALL: " + adState + " Tag : " + adTag);
        });

        HZOfferWallAd.SetVirtualCurrencyResponseListener(delegate (VirtualCurrencyResponse response)
        {
            console.Append("OFFERWALL VCS Response: id:" + response.CurrencyID + " name: '" + response.CurrencyName + "' amount : " + response.DeltaOfCurrency + " trans: " + response.LatestTransactionID);
        });

        HZOfferWallAd.SetVirtualCurrencyErrorListener(delegate (string errorMsg)
        {
            console.Append("OFFERWALL VCS Error: " + errorMsg);
        });

        // UI defaults
        bannerPosition = HZBannerShowOptions.POSITION_TOP;
        SelectedAdType = AdType.Interstitial;

        ShowAdTypeControls();
    }

    public void InterstitialSelected(bool selected)
    {
        if (selected)
        {
            SelectedAdType = AdType.Interstitial;
        }
    }

    public void VideoSelected(bool selected)
    {
        if (selected)
        {
            SelectedAdType = AdType.Video;
        }
    }

    public void IncentivizedSelected(bool selected)
    {
        if (selected)
        {
            SelectedAdType = AdType.Incentivized;
        }
    }

    public void BannerSelected(bool selected)
    {
        if (selected)
        {
            SelectedAdType = AdType.Banner;
        }
    }

    public void OfferwallSelected(bool selected)
    {
        if (selected)
        {
            SelectedAdType = AdType.Offerwall;
        }
    }

    public void IsAvailableButton()
    {
        string tagText = getTagOrNull(adTagTextField.text);
        bool available = false;

        switch (SelectedAdType)
        {
            case AdType.Interstitial:
                available = HZInterstitialAd.IsAvailable(tagText);
                break;
            case AdType.Video:
                available = HZVideoAd.IsAvailable(tagText);
                break;
            case AdType.Incentivized:
                available = HZIncentivizedAd.IsAvailable(tagText);
                break;
            case AdType.Banner:
                // Not applicable
                break;
            case AdType.Offerwall:
                available = HZOfferWallAd.IsAvailable(tagText);
                break;
        }

        string availabilityMessage = available ? "available" : "not available";
        console.Append(SelectedAdType.ToString() + " with tag: " + tagText + " is " + availabilityMessage);
    }

    public void ShowButton()
    {
        string tagText = getTagOrNull(adTagTextField.text);

        HZShowOptions showOptions = new HZShowOptions
        {
            Tag = tagText
        };

        HZIncentivizedShowOptions incentivizedOptions = new HZIncentivizedShowOptions
        {
            Tag = tagText,
            IncentivizedInfo = "test app incentivized info!"
        };

        HZBannerShowOptions bannerOptions = new HZBannerShowOptions
        {
            Tag = tagText,
            Position = bannerPosition
        };

        HZOfferWallShowOptions offerwallOptions = new HZOfferWallShowOptions
        {
            ShouldCloseAfterFirstClick = offerwallCloseOnFirstClickToggle.isOn,
            Tag = tag
        };

        console.Append("Showing " + SelectedAdType.ToString() + " with tag: " + tagText);
        switch (SelectedAdType)
        {
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

    public void FetchButton()
    {
        string tagText = getTagOrNull(adTagTextField.text);
        console.Append("Fetching " + SelectedAdType.ToString() + " with tag: " + tagText);
        switch (SelectedAdType)
        {
            case AdType.Interstitial:
                HZInterstitialAd.Fetch(tagText);
                break;
            case AdType.Video:
                HZVideoAd.Fetch(tagText);
                break;
            case AdType.Incentivized:
                HZIncentivizedAd.Fetch(tagText);
                break;
            case AdType.Offerwall:
                HZOfferWallAd.Fetch(tagText);
                break;
        }
    }

    private String getTagOrNull(string tagText)
    {
        return String.IsNullOrEmpty(tagText) ? null : tagText;
    }

    public void HideButton()
    {
        if (SelectedAdType == AdType.Banner)
        {
            console.Append("Hiding Banner");
            HZBannerAd.Hide();
        }
    }

    public void DestroyButton()
    {
        if (SelectedAdType == AdType.Banner)
        {
            console.Append("Destroying Banner");
            HZBannerAd.Destroy();
        }
    }

    public void RemoteDataButton()
    {
        console.Append("Remote data: " + HeyzapAds.GetRemoteData());
    }

    public void DebugLogSwitch(bool on)
    {
        if (on)
        {
            console.Append("Enabling debug logging");
            HeyzapAds.ShowDebugLogs();
        }
        else
        {
            console.Append("Disabling debug logging");
            HeyzapAds.HideDebugLogs();
        }
    }

    public void AcceptGdprButton()
    {
        console.Append("Accepting GDPR");
        HeyzapAds.SetGdprConsent(true);
    }

    public void RejectGdprButton()
    {
        console.Append("Rejecting GDPR");
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

    public void BannerPositionTop(bool selected)
    {
        if (selected)
        {
            this.bannerPosition = HZBannerShowOptions.POSITION_TOP;
        }
    }

    public void BannerPositionBottom(bool selected)
    {
        if (selected)
        {
            bannerPosition = HZBannerShowOptions.POSITION_BOTTOM;
        }
    }

    public void VCSRequestButton()
    {
        HZOfferWallAd.RequestDeltaOfCurrency(CurrencyId());
    }

    public void ShowMediationTest()
    {
        console.Append("Showing mediation test suite");
        HeyzapAds.ShowMediationTestSuite();
    }

    private void ShowAdTypeControls()
    {
        if (SelectedAdType == AdType.Banner)
        {
            bannerControls.SetActive(true);
            standardControls.SetActive(false);
            offerwallControls.SetActive(false);
        }
        else if (SelectedAdType == AdType.Offerwall)
        {
            bannerControls.SetActive(false);
            offerwallControls.SetActive(true);
            standardControls.SetActive(false);
        }
        else
        {
            bannerControls.SetActive(false);
            offerwallControls.SetActive(false);
            standardControls.SetActive(true);
        }
    }

    private string CurrencyId()
    {
        string currencyId = offerwallCurrencyIdTextField.text;
        if (currencyId == null || tag.Trim().Length == 0)
        {
            return null;
        }
        return currencyId;
    }
}
