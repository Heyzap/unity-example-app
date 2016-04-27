using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions.Must;
using System.Collections;
using Heyzap;

public class AdManager : MonoBehaviour {

    [SerializeField]
    private InputField appIDInputField;
    [SerializeField]
    private InputField tokenInputField;
    [SerializeField]
    private Toggle autofetchToggle;

    [SerializeField]
    private GameObject preStartControls;
    
    [SerializeField]
    private Text adTagTextField;
    [SerializeField]
    private GameObject bannerControls;
    [SerializeField]
    private GameObject nonBannerControls;
    [SerializeField]
    private ScrollingTextArea console;

    [SerializeField]
    private Dropdown bannerPositionDropdown;

    private enum AdType {
        Interstitial,
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

    private const string appIDKey = "fyberAppIDKey";
    private const string tokenKey = "fyberTokenKey";
    private const string autofetchKey = "fyberAutofetchKey";

   

    void Awake() {
        this.console.MustNotBeNull();
        this.appIDInputField.MustNotBeNull();
		this.tokenInputField.MustNotBeNull();
		this.autofetchToggle.MustNotBeNull();
        this.console.MustNotBeNull();
		string defaultAppId = null;
		string defaultToken = null;

		#if UNITY_ANDROID
		defaultAppId = "40867";
		defaultToken = "419bcd37e8454aab3d49fa4df09d4a39";
		#endif

        // Restore UI state
		string previousAppID = PlayerPrefs.GetString(appIDKey, defaultAppId);
        if (previousAppID != null) {
            this.appIDInputField.text = previousAppID;
        }

		string previousToken = PlayerPrefs.GetString(tokenKey, defaultToken);
        if (previousToken != null) {
            this.tokenInputField.text = previousToken;
        }

        int previousAutofetch = PlayerPrefs.GetInt(autofetchKey, 1);
        this.autofetchToggle.isOn = previousAutofetch == 1;
    }

    public void StartButton() {
        string appID = this.appIDInputField.text;
        string token = this.tokenInputField.text;

        if (appID.Equals("") || token.Equals("")) {
			this.console.Append("A valid app ID and token must be provided");
			return;
        }

        // Store UI state for future launches
        PlayerPrefs.SetString(appIDKey, appID);
        PlayerPrefs.SetString(tokenKey, token);
        PlayerPrefs.SetInt(autofetchKey, this.autofetchToggle.isOn ? 1 : 0);

        this.console.Append("Starting SDK");

        int options = this.autofetchToggle.isOn ? HeyzapAds.FLAG_NO_OPTIONS : HeyzapAds.FLAG_DISABLE_AUTOMATIC_FETCHING;
        HeyzapAds.Start(appID, token, options); 

        this.preStartControls.SetActive(false);

//        HeyzapAds.Start("22051", "token", HeyzapAds.FLAG_NO_OPTIONS);

        HZInterstitialAd.SetDisplayListener(delegate(string adState) {
            this.console.Append("INTERSTITIAL: " + adState);
        });

        HZIncentivizedAd.SetDisplayListener(delegate(string adState) {
            this.console.Append("INCENTIVIZED: " + adState);
        });

        HZBannerAd.SetDisplayListener(delegate(string adState) {
            this.console.Append("BANNER: " + adState);
        });

//        this.bannerControls.SetActive(false);
//        this.nonBannerControls.SetActive(true);

        // UI defaults
        this.SelectedAdType = AdType.Interstitial;
    }

    public void InterstitialSelected(bool selected) {
        if (selected) {
            this.SelectedAdType = AdType.Interstitial;
        }
    }

    public void VideoSelected(bool selected) {
        if (selected) {
            
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

    public void DestroyBanner() {
        this.console.Append("Destroying Banner");
        HZBannerAd.Destroy();

    }

    public void HideBanner() {
        this.console.Append("Destroying Banner");
        HZBannerAd.Hide();
    }

    public void ShowBanner() {
        this.console.Append("Showing Banner");
        HZBannerShowOptions showOptions = new HZBannerShowOptions();
        if (this.bannerPositionDropdown.value == 0) {
            showOptions.Position = HZBannerShowOptions.POSITION_TOP;
        } else {
            showOptions.Position = HZBannerShowOptions.POSITION_BOTTOM;
        }

        HZBannerAd.ShowWithOptions(showOptions);
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
            // Should never happen b/c "Available?" should be hidden when banners is selected
            return;
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
                HZIncentivizedAd.Show();
                break;
            case AdType.Banner:
                // Should never happen b/c "Show?" should be hidden when banners is selected
                return;
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
            case AdType.Banner:
                // Should never happen b/c "Available?" should be hidden when banners is selected
                return;
    
        }
    }

    public void HideButton() {
        // was for banners
    }

    public void DestroyButton() {
        // was for banners
    }

    public void RemoteDataButton() {
        // todo delete me
//        this.console.Append("Remote data: " + HeyzapAds.GetRemoteData());
    }

    public void DebugLogSwitch(bool on) {
        
    }

    public void BannerPositionTop(bool selected) {
        
    }

    public void BannerPositionBottom(bool selected) {
        
    }

    public void ShowMediationTest() {
        
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

    private string adTag() {
        return null;
    }
}
