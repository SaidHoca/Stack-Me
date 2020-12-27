using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
 

	public static UIControl instance;

	[SerializeField]
	private Text scoreText,endScoreText,restartButtonText;
	[SerializeField]
	private Text highscoreText;
	[SerializeField]
	private Text comboText;

	[SerializeField]
	private Button restartButton,rewardedButton;

	[SerializeField]
	private Animator settingAnim, menuAnim,noAdsAnim,comboTextAnim, rewardedBtnAnim, biggerAnim;

	[SerializeField]
	private Image soundImg, vibrationImg, restartImg;

	[SerializeField]
	private Sprite onSprite, offSprite,startBtnSprite,restartBtnSprite;

	private int maxFill, minFill;

	private bool firstGame = true;


	private void Awake()
	{
		if (instance == null) instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	private void Start()
	{
		maxFill = 400;
		minFill = 1;
		StartSettings();
		if (firstGame)
		{
			rewardedButton.gameObject.SetActive(false);
			firstGame = false;
			restartImg.sprite = startBtnSprite;
		}
		else
		{
			rewardedButton.gameObject.SetActive(true);
			restartImg.sprite = restartBtnSprite;
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void RestartGame()
	{
		menuAnim.SetTrigger("cik");		
		TheStack.instance.StartGame();	
	}

	public void OpenMenu()
	{
		menuAnim.SetTrigger("gir");
		setMenuPanel();
	}

	public void CloseMenu()
	{
		menuAnim.SetTrigger("cik");
	}

	public void setScore(int score)
	{
		scoreText.text = score.ToString();
	}

	public void setEndScore(int score)
	{
		endScoreText.text = "Score : " + score.ToString();
	}

	public void setMenuPanel()
	{
		if (firstGame)
		{
			rewardedButton.gameObject.SetActive(false);
			firstGame = false;
			restartImg.sprite = startBtnSprite;

		}
		else if(TheStack.instance.getRewardState())
		{

			rewardedButton.gameObject.SetActive(true);
			restartImg.sprite = restartBtnSprite;
			rewardedBtnAnim.SetTrigger("gir");
		}
		else
		{
			rewardedButton.gameObject.SetActive(false);
			restartImg.sprite = restartBtnSprite;
			
		}
	}

	public void setGameState()
	{
		firstGame = false;
	}

	public void Settings()
	{
		settingAnim.SetTrigger("gir");
		menuAnim.SetTrigger("cik");
	}

	public void CloseSettingPanel()
	{
		settingAnim.SetTrigger("cik");
		menuAnim.SetTrigger("gir");
	}

	public void Sound()
	{
		if(PlayerPrefs.GetInt("sound") == 1)
		{
			// sesi kapatma işlemleri
			PlayerPrefs.SetInt("sound", 0);
			soundImg.sprite = offSprite;
		} 
		else if(PlayerPrefs.GetInt("sound") == 0)
		{
			// sesi açma işlemleri
			PlayerPrefs.SetInt("sound", 1);
			soundImg.sprite = onSprite;
		}
	}

	public void Vibration()
	{
		if (PlayerPrefs.GetInt("vibration") == 1)
		{
			// titreşim kapatma işlemleri
			PlayerPrefs.SetInt("vibration", 0);
			vibrationImg.sprite = offSprite;
		}
		else if (PlayerPrefs.GetInt("vibration") == 0)
		{
			// titreşim açma işlemleri
			PlayerPrefs.SetInt("vibration", 1);
			vibrationImg.sprite = onSprite;
		}
	}


	private void StartSettings()
	{
		if (PlayerPrefs.GetInt("vibration") == 1) vibrationImg.sprite = onSprite;
		else if (PlayerPrefs.GetInt("vibration") == 0) vibrationImg.sprite = offSprite;

		if (PlayerPrefs.GetInt("sound") == 1) soundImg.sprite = onSprite;
		else if (PlayerPrefs.GetInt("sound") == 0) soundImg.sprite = offSprite;

		SetHighScore(PlayerPrefs.GetInt("highscore"));
	}

	public void SetHighScore(int score)
	{
		highscoreText.text = "Best : " + score.ToString();
	}

	public void Like()
	{

	}

	public void Info()
	{

	}
	
	public void Combo(int combo)
	{
		comboText.text = "x " + combo;
		comboTextAnim.SetTrigger("gir");
	}

	public void LeaderBoard()
	{
		PlayServices.instance.ShowLeaderBoard();
	}

	public void NoAds()
	{
		noAdsAnim.SetTrigger("gir");
	}

	public void RestartButtonActive()
	{
		if (TheStack.instance.getRewardState())
		{
			restartImg.fillAmount = 0;
			restartButton.enabled = false;
			StartCoroutine(FillRestartImg());
		}
		else
		{
			restartImg.fillAmount = 1;
			restartButton.enabled = true;
		}
	}



	IEnumerator FillRestartImg()
	{
		
		float fill = (float)minFill / (float)maxFill;
		restartImg.fillAmount = fill;
		minFill++;
		if(minFill == 400)
		{
			minFill = 1;
			restartButton.enabled = true;
		}
		else
		{
			yield return new WaitForSeconds(0.01f);
			StartCoroutine(FillRestartImg());
		}		
	}

	public void BiggerAnim()
	{
		biggerAnim.SetTrigger("gir");
	}



	
}
