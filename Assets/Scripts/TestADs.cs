using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestADs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showbannerAd()
    {
        AdsManager.Instance.TapdaqBannerShow();
    }

    public void HidebannerAd()
    {
        AdsManager.Instance.TapdaqBannerHide();
    }

    public void ShowIntersitialAd()
    {
        AdsManager.Instance.TapdaqInterstital();
    }
    public void ShowUnityAd()
    {
        AdsManager.Instance.TapdaqInterstital();
    }

    //public void ShowIronIntersitialAd()
    //{
    //    AdsManager.Instance.IronSourceInterstitial.ShowRewardedVideoButtonClicked();
    //}
    //public void loadIronIntersitialAd()
    //{
    //    AdsManager.Instance.IronSourceInterstitial.ShowRewardedVideoButtonClicked();
    //}

    public void ShowFacebookIntersitialAd()
    {
        AdsManager.Instance.TapdaqInterstital();
    }
    public void loadFacebookIntersitialAd()
    {
        AdsManager.Instance.TapdaqInterstital();
    }
    public void DestoryThisObject()
    {
        Destroy(this.gameObject);
    }
}
