using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public InterstitialAd interstitial;
    private string yetremId = "ca-app-pub-3345994290034136~3575736324";
    private string interstitialId = "ca-app-pub-3345994290034136/1024405233";
    private string testId = "";

    private bool isExiting = false;

    private void Awake()
    {
#if UNITY_ANDROID
        string appId = "ca-app-pub-3940256099942544~3347511713";
#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        RequestInterstitial();
    }


    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);

        this.interstitial.OnAdClosed += Interstitial_OnAdClosed;
    }

    private void Interstitial_OnAdClosed(object sender, System.EventArgs e)
    {
        if (isExiting)
            Application.Quit();
        RequestInterstitial();
    }

    public void ShowInterstitial(bool exit)
    {
        this.isExiting = exit;
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }
}
