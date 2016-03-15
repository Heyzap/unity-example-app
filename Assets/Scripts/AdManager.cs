using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions.Must;
using System.Collections;
using Heyzap;

public class AdManager : MonoBehaviour {
    
    [SerializeField]
    private Text adTagTextField;
    [SerializeField]
    private GameObject bannerControls;
    [SerializeField]
    private GameObject nonBannerControls;
    [SerializeField]
    private ScrollingTextArea console;

    private enum AdType {
        Interstitial,
        Video,
        Incentivized,
        Banner
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

    void Awake() {
        this.adTagTextField.MustNotBeNull();
        this.bannerControls.MustNotBeNull();
        this.nonBannerControls.MustNotBeNull();
        this.console.MustNotBeNull();
    }

    void Start () {
        
		HeyzapAds.Start("40867", "419bcd37e8454aab3d49fa4df09d4a39", HeyzapAds.FLAG_NO_OPTIONS);


        HZInterstitialAd.SetDisplayListener(delegate(string adState) {
            this.console.Append("INTERSTITIAL: " + adState);
        });

        HZIncentivizedAd.SetDisplayListener(delegate(string adState) {
            this.console.Append("INCENTIVIZED: " + adState);
        });

        this.bannerControls.SetActive(false);
        this.nonBannerControls.SetActive(true);

        // UI defaults
        this.bannerPosition = HZBannerShowOptions.POSITION_TOP;
        this.SelectedAdType = AdType.Interstitial;
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

    public void IsAvailableButton() {
        bool available = false;

        switch (this.SelectedAdType) {
        case AdType.Interstitial:
            available = HZInterstitialAd.IsAvailable();
            break;
        case AdType.Incentivized:
            available = HZIncentivizedAd.IsAvailable();
            break;
        case AdType.Banner:
            // Not applicable
            break;
        }

        string availabilityMessage = available ? "available" : "not available";
        this.console.Append(this.SelectedAdType.ToString() + " is " + availabilityMessage);
    }

	public void ShowButton() {
        this.console.Append("Showing " + this.SelectedAdType.ToString());
        switch (this.SelectedAdType) {
            case AdType.Interstitial:
                HZInterstitialAd.Show();
                break;
			case AdType.Incentivized:
				HZIncentivizedAd.Show ();
                break;
        }
    }

    public void FetchButton() {
        this.console.Append("Fetching " + this.SelectedAdType.ToString());
        switch(this.SelectedAdType) {
            case AdType.Interstitial:
                HZInterstitialAd.Fetch();
                break;
            case AdType.Incentivized:
                HZIncentivizedAd.Fetch();
                break;
        }
    }

    public void HideButton() {
        
    }

    public void DestroyButton() {
        
    }

    public void RemoteDataButton() {
    
    }

    public void DebugLogSwitch(bool on) {
        
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
       
    }

    private void ShowAdTypeControls() {
        if (this.SelectedAdType == AdType.Banner) {
            this.bannerControls.SetActive(true);
            this.nonBannerControls.SetActive(false);
        } else {
            this.bannerControls.SetActive(false);
            this.nonBannerControls.SetActive(true);
        }
    }
}
