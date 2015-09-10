using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions.Must;
using System.Collections;

public class AdManager : MonoBehaviour 
{

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
        get{ return _selectedAdType;}
        set {
            _selectedAdType = value;
            this.console.Append("AdType: " + value.ToString());
            ShowAdTypeControls();
        }
    }

    private string bannerPosition;

	void Awake()
	{
        this.adTagTextField.MustNotBeNull();
        this.bannerControls.MustNotBeNull();
        this.nonBannerControls.MustNotBeNull();
        this.console.MustNotBeNull();
	}
	
	void Start () 
	{
		HeyzapAds.NetworkCallbackListener networkCallbackListner = delegate(string network, string callback)
		{
			this.console.Append("[" + network + "]: " + callback);
		};

		HeyzapAds.setNetworkCallbackListener(networkCallbackListner);
		HeyzapAds.start("ENTER_YOUR_PUBLISHER_ID_HERE",HeyzapAds.FLAG_NO_OPTIONS);

        HZBannerAd.setDisplayListener(delegate(string adState, string adTag)
        {
            this.console.Append("BANNER: " + adState + " Tag : " + adTag);
        });

        HZInterstitialAd.setDisplayListener(delegate(string adState, string adTag)
        {
            this.console.Append("INTERSTITIAL: " + adState + " Tag : " + adTag);
        });

        HZIncentivizedAd.setDisplayListener(delegate(string adState, string adTag)
                                      {
            this.console.Append("INCENTIVIZED: " + adState + " Tag : " + adTag);
        });
        
        HZVideoAd.setDisplayListener(delegate(string adState, string adTag)
                                            {
            this.console.Append("VIDEO: " + adState + " Tag : " + adTag);
        });

        this.bannerControls.SetActive(false);
        this.nonBannerControls.SetActive(true);

        // UI defaults
        this.bannerPosition = HZBannerAd.POSITION_TOP;
        this.SelectedAdType = AdType.Interstitial;
        HeyzapAds.hideDebugLogs();
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

	public void ShowButton()
	{
        string tag = this.adTag();
        this.console.Append("Showing " + this.SelectedAdType.ToString() + " with tag: " + tag);
		switch (this.SelectedAdType) {
    		case AdType.Interstitial:
    			HZInterstitialAd.show(tag);
    			break;
    		case AdType.Video:
    			HZVideoAd.show(tag);
    			break;
    		case AdType.Incentivized:
    			HZIncentivizedAd.show(tag);
    			break;
    		case AdType.Banner:
    			HZBannerAd.showWithTag(this.bannerPosition, tag);
    			break;
		}
	}

	public void FetchButton() {
        string tag = this.adTag();
        this.console.Append("Fetching " + this.SelectedAdType.ToString() + " with tag: " + tag);
        switch(this.SelectedAdType) {
    		case AdType.Interstitial:
    			HZInterstitialAd.fetch(tag);
    			break;
    		case AdType.Video:
    			HZVideoAd.fetch(tag);
    			break;
    		case AdType.Incentivized:
    			HZIncentivizedAd.fetch(tag);
    			break;
		}
	}

	public void HideButton() {
		if (this.SelectedAdType == AdType.Banner) {
            this.console.Append("Hiding Banner");
			HZBannerAd.hide();
		}
	}

    public void DestroyButton() {
        if (this.SelectedAdType == AdType.Banner) {
            this.console.Append("Destroying Banner");
            HZBannerAd.destroy();
        }
    }

    public void DebugLogSwitch(bool on) {
        if (on) {
            this.console.Append("Enabling debug logging");
            HeyzapAds.showDebugLogs();
        } else {
            this.console.Append("Disabling debug logging");
            HeyzapAds.hideDebugLogs();
        }
    }

    public void BannerPositionTop(bool selected){
        if (selected) {
            this.bannerPosition = HZBannerAd.POSITION_TOP;
        }
    }

    public void BannerPositionBottom(bool selected){
        if (selected) {
            this.bannerPosition = HZBannerAd.POSITION_BOTTOM;
        }
    }

	public void ShowMediationTest()
	{
        this.console.Append("Showing mediation test suite");
		HeyzapAds.showMediationTestSuite();
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

    private string adTag(){
        string tag = this.adTagTextField.text;
        if (tag == null || tag.Trim().Length == 0) {
            return "default";
        } else {
            return tag;
        }
    }
}
