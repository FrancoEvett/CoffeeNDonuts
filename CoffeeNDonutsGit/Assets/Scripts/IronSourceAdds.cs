using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// The calls will handle the ad intergation with ironsource, bind this to interface and logic cues, will have references in other classes that needs to initate ad events
/// </summary>


// Example for IronSource Unity.
public class IronSourceAdds : MonoBehaviour
{
    public MenuController menuController;
    private string appKey = "19037f705";//one of my test project apps, just configured to show test ads from IronSource
    public bool rewardGained;

    public void Start()
    {

        //Ad Init Event
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

        //Ad Rewarded Video Events
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;

        //Ad Interstitial Events
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        //Ad Banner Events
        IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
        IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
        IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
        IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
        IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
        IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;

        IronSource.Agent.validateIntegration();

        //SDK inititisation - use app key to initalise the connection with ironscource
        IronSource.Agent.init(appKey);
    }



    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);//let iron know when teh app is paused
    }



    #region ad loading/displaying

    //rewarded video
    public void LoadRewardedVideo()
    {
        IronSource.Agent.loadRewardedVideo();
    }
    public void ShowRewardedVideo()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            //will not happen with only test ads enabled, but put exception handling here if it were to be put live
        }
    }

    //interstatial
    public void LoadInterstitialAd()
    {
        IronSource.Agent.loadInterstitial();
    }
    public void ShowInterstitialAd()
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            //will not happen with only test ads enabled, but put exception handling here if it were to be put live
        }
    }


    //banner ad
    public void LoadBannerAd()
    {
        //decide if top or bottom banner added, possable add a bool to decided between the two
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);//this will imediatly show a banner add unlike the otehr add types wich go to cache
    }
    public void RemoveBannerAd()
    {
        IronSource.Agent.destroyBanner();
    }


    #endregion

    
    #region Init callback handlers

    void SdkInitializationCompletedEvent()
    {
        Debug.Log("unity-script: I got SdkInitializationCompletedEvent");
    }

    #endregion

    #region RewardedAd callback handlers

    void RewardedVideoAvailabilityChangedEvent(bool canShowAd) { }
    void RewardedVideoAdOpenedEvent() { }
    public void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp) 
    {
        //for some reason callign methods fromm this odes not work?? , set value and initaie a check for it 
        rewardGained = true;
    }
    void RewardedVideoAdClosedEvent() { }
    void RewardedVideoAdStartedEvent() {  }
    void RewardedVideoAdEndedEvent() {  }
    void RewardedVideoAdShowFailedEvent(IronSourceError error) { }
    void RewardedVideoAdClickedEvent(IronSourcePlacement ssp) { }


    #endregion

    #region Interstitial callback handlers

    void InterstitialAdReadyEvent() { }
    void InterstitialAdLoadFailedEvent(IronSourceError error) { }
    void InterstitialAdShowSucceededEvent() { }
    void InterstitialAdShowFailedEvent(IronSourceError error) { }
    void InterstitialAdClickedEvent() { }
    void InterstitialAdOpenedEvent() { }
    void InterstitialAdClosedEvent() { }

    #endregion

    #region Banner callback handlers

    void BannerAdLoadedEvent() { }
    void BannerAdLoadFailedEvent(IronSourceError error) { }
    void BannerAdClickedEvent() { }
    void BannerAdScreenPresentedEvent() { }
    void BannerAdScreenDismissedEvent() { }
    void BannerAdLeftApplicationEvent() { }

    #endregion



}
