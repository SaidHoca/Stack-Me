using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class PlayServices : MonoBehaviour
{
	public static PlayServices instance;

	private void Awake()
	{
		if (instance == null) instance = this;
	}

	private void Start()
	{
		DontDestroyOnLoad(this);
		try
		{
			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
			PlayGamesPlatform.InitializeInstance(config);
			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate();
			Social.localUser.Authenticate((bool success) => { });
		}
		catch(Exception exception)
		{
			Debug.Log(exception);
		}
	}

	public void AddScoreToLeaderBoard(int playerScore)
	{
		if (Social.localUser.authenticated)
		{
			Social.ReportScore(playerScore, "CgkI8Pu21vweEAIQAA", success => { });
		}
	}

	public void ShowLeaderBoard()
	{
		if (Social.localUser.authenticated)
		{
			Social.ShowLeaderboardUI();
		}
	}
}
