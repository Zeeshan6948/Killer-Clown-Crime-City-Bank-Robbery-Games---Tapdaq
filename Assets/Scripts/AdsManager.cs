 using UnityEngine;
using Tapdaq;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;
    public TDBannerPosition bannerPosition;
    public TDMBannerSize bannerSize;


    private GameObject RefObject;
    private string RefFunction = "";
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        //AdManager.SetUserSubjectToGDPR(TDStatus.FALSE);
        //AdManager.SetConsentGiven(true);
        //AdManager.SetIsAgeRestrictedUser(true);
        //AdManager.Init(); //Unknown User.
        AdManager.Init(TDStatus.FALSE, TDStatus.FALSE, TDStatus.FALSE);//IsUserSubjectToGDPR, UserConsent, isAgeRestrictedUser

    }

    private void OnEnable()
    {
        TDCallbacks.TapdaqConfigLoaded += OnTapdaqConfigLoaded;
        TDCallbacks.TapdaqConfigFailedToLoad += OnTapdaqConfigFailToLoad;
        TDCallbacks.AdAvailable += OnAdAvailable;
        TDCallbacks.AdNotAvailable += OnAdNotAvailable;
        TDCallbacks.AdClicked += OnAdClicked;
        TDCallbacks.AdClosed += OnAdClosed;
        TDCallbacks.RewardVideoValidated += OnRewardVideoValidated;
    }

    private void OnDisable()
    {
        TDCallbacks.TapdaqConfigLoaded -= OnTapdaqConfigLoaded;
        TDCallbacks.TapdaqConfigFailedToLoad -= OnTapdaqConfigFailToLoad;
        TDCallbacks.AdAvailable -= OnAdAvailable;
        TDCallbacks.AdNotAvailable -= OnAdNotAvailable;
        TDCallbacks.AdClicked -= OnAdClicked;
        TDCallbacks.AdClosed -= OnAdClosed;
        TDCallbacks.RewardVideoValidated -= OnRewardVideoValidated;
    }

    private void OnTapdaqConfigLoaded()
    {
        Debug.Log("TapDaq Successfull");
        AdManager.RequestBanner(bannerSize);
        AdManager.LoadInterstitial("main_menu");
        AdManager.LoadVideo("main_menu");
        AdManager.LoadRewardedVideo("game_over");
    }

    private void OnTapdaqConfigFailToLoad(TDAdError error)
    {
        Debug.LogError("TapDaq Unsuccessfull");
    }

    private void OnAdAvailable(TDAdEvent e)
    {
        // Ad has loaded, can now be displayed. 
        // This method will also be called when a banner is refreshed every 30 seconds even if the banner is already in view. Calling show again is safe, alternatively TDAdEvent message property will note either "LOADED" or "REFRESH"

        if (e.adType == "BANNER")
        {
            //AdManager.ShowBanner(bannerPosition);
            Debug.Log("BANNER Ad is Avaiable");
        }
        if (e.adType == "INTERSTITIAL" && e.tag == "main_menu")
        {
            //AdManager.ShowInterstitial("main_menu");
            Debug.Log("INTERSTITIAL Ad is Avaiable");
        }
        if (e.adType == "VIDEO" && e.tag == "main_menu")
        {
            //AdManager.ShowVideo("main_menu");
            Debug.Log("VIDEO Ad is Avaiable");
        }
        if (e.adType == "REWARD_AD" && e.tag == "game_over")
        {
            // Add code here to display or enable your Rewarded Video button.
            Debug.Log("REWARD_AD Ad is Avaiable");
        }
    }

    public void OnAdNotAvailable(TDAdEvent adEvent)
    {
        // Ad has failed to load/refresh
    }

    public void OnAdClicked(TDAdEvent adEvent)
    {
        // Ad has been clicked
    }

    private void OnAdClosed(TDAdEvent e)
    {
        if (e.IsInterstitialEvent())
        {
            AdManager.LoadInterstitial(e.tag);
        }
        if (e.IsVideoEvent())
        {
            AdManager.LoadVideo(e.tag);
        }
        if (e.IsRewardedVideoEvent())
        {
            AdManager.LoadRewardedVideo(e.tag);
            RefObject.SendMessage(RefFunction);
        }
    }

    private void OnRewardVideoValidated(TDVideoReward videoReward)
    {
        Debug.Log("I got " + videoReward.RewardAmount + " of " + videoReward.RewardName
                + "   tag=" + videoReward.Tag + " IsRewardValid " + videoReward.RewardValid + " CustomJson: " + videoReward.RewardJson);

        if (videoReward.RewardValid)
        {
            //Give Reward
            RefObject.SendMessage(RefFunction,SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            //Reward is invalid, video may not have completed or an ad network may have refused to the provide reward
        }
    }

    public void TapdaqBannerShow()
    {
        if (AdManager.IsBannerReady())
        {
            AdManager.ShowBanner(bannerPosition);
        }
    }

    public void TapdaqBannerHide()
    {
        AdManager.HideBanner();
    }

    public void TapdaqInterstital()
    {
        if (AdManager.IsInterstitialReady("main_menu"))
        {
            AdManager.ShowInterstitial("main_menu");
        }
    }

    public void TapdaqVideo()
    {
        if (AdManager.IsVideoReady("main_menu"))
        {
            AdManager.ShowVideo("main_menu");
        }
    }

    public void TappedReward()
    {
        if (AdManager.IsRewardedVideoReady("game_over"))
        {
            AdManager.ShowRewardVideo("game_over");
        }
    }

    public void functioncalling(GameObject obj, string Name)
    {
        RefObject = obj;
        RefFunction = Name;
    }

    public void MediationAd()
    {
        if (AdManager.IsVideoReady("main_menu"))
        {
            AdManager.ShowVideo("main_menu");
        }
        else if (AdManager.IsInterstitialReady("main_menu"))
        {
            AdManager.ShowInterstitial("main_menu");
        }
    }
}

