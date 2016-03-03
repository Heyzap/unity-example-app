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

    private enum AdType {
        Interstitial,
        Incentivized
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
        this.console.MustNotBeNull();

        // Restore UI state
        string previousAppID = PlayerPrefs.GetString(appIDKey, null);
        if (previousAppID != null) {
            this.appIDInputField.text = previousAppID;
        }
        string previousToken = PlayerPrefs.GetString(tokenKey, null);
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
    }

    void Start () {
        

//        HeyzapAds.Start("22051", "token", HeyzapAds.FLAG_NO_OPTIONS);

        HZInterstitialAd.SetDisplayListener(delegate(string adState, string adTag) {
            this.console.Append("INTERSTITIAL: " + adState);
        });

        HZIncentivizedAd.SetDisplayListener(delegate(string adState, string adTag) {
            this.console.Append("INCENTIVIZED: " + adState);
        });

//        this.bannerControls.SetActive(false);
//        this.nonBannerControls.SetActive(true);

        // UI defaults
//        this.SelectedAdType = AdType.Interstitial;
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
        
    }

    private string adTag() {
        return null;
    }
}
