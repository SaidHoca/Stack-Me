using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    private BannerView reklam;    
    private InterstitialAd interReklam;
    private RewardedAd rewardReklam;

	//GERÇEK IDLER
	private string bannerId = "ca-app-pub-9298179864504105/9899470356";
	private string interId = "ca-app-pub-9298179864504105/3334062000";
	private string rewardId = "ca-app-pub-9298179864504105/6246556990"; // şimdilik rewarded istemiyoruz...

	//private string bannerId = "ca-app-pub-3940256099942544/6300978111";  // TEST ID  
	//   private string interId = "ca-app-pub-3940256099942544/1033173712";// TEST ID
	//   private string rewardId = "ca -app-pub-3940256099942544/5224354917"; // TEST ID


	private void Awake()
	{
        if (instance == null) instance = this;
	}

	private void Start()
    {
        MobileAds.Initialize(reklamver => { });
        BannerReklam();
		RewardedReklam();

	}

	private void Update()
	{
		RewardEvents();
	}

	public void BannerReklam()
	{
        reklam = new BannerView(bannerId,AdSize.SmartBanner,AdPosition.Bottom);
        AdRequest yeniReklam = new AdRequest.Builder().Build();
        reklam.LoadAd(yeniReklam);
	}

    public void InterstitialReklam()
	{
        interReklam = new InterstitialAd(interId);
        AdRequest yeniReklam = new AdRequest.Builder().Build();
        interReklam.LoadAd(yeniReklam);
        
    }

    public void ShowInsterstitial()
	{
        interReklam.Show();
    }

	public void RewardedReklam()
	{
		rewardReklam = new RewardedAd(rewardId);
		AdRequest yeniReklam = new AdRequest.Builder().Build();
		rewardReklam.LoadAd(yeniReklam);
	}

	public void ShowRewardedReklam()
	{
		if(rewardReklam.IsLoaded()) this.rewardReklam.Show();

	}

	public void RewardEvents()
	{
		// Reklam başarıyla yüklendiğinde...
		this.rewardReklam.OnAdLoaded += HandleRewardedAdLoaded;

		// Reklam başarısız olduğunda çalışır.
		this.rewardReklam.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;

		// Reklam gösterildiğinde çalışır.
		this.rewardReklam.OnAdOpening += HandleRewardedAdOpening;

		// Reklam isteği gösterilirken hata olursa çalışır
		this.rewardReklam.OnAdFailedToShow += HandleRewardedAdFailedToShow;

		// Ödül almaya hak kazanılınca çalışır..
		this.rewardReklam.OnUserEarnedReward += HandleUserEarnedReward;

		// reklam kapatıldığında çalışır..
		this.rewardReklam.OnAdClosed += HandleRewardedAdClosed;
	}

	public void HandleRewardedAdLoaded(object sender, EventArgs args)
	{
		Debug.Log("BAŞARISIZ...");
	}

	public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
	{
		MonoBehaviour.print(
			"HandleRewardedAdFailedToLoad event received with message: "
							 + args.Message);
	}

	public void HandleRewardedAdOpening(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdOpening event received");
	}

	public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
	{
		MonoBehaviour.print(
			"HandleRewardedAdFailedToShow event received with message: "
							 + args.Message);
	}

	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdClosed event received");
	}

	public void HandleUserEarnedReward(object sender, Reward args)
	{
		
		TheStack.instance.RewardedStacks();
	}


}
