using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
 

	public static UIControl instance;

	[SerializeField]
	private Text scoreText;
	[SerializeField]
	private Text highscoreText;

	[SerializeField]
	private Animator settingAnim, menuAnim;

	[SerializeField]
	private Image soundImg, vibrationImg, fogImg;

	[SerializeField]
	private Sprite onSprite, offSprite;

	[SerializeField]
	private GameObject fogObj;

	private void Awake()
	{
		if (instance == null) instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	private void Start()
	{
		StartSettings();
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void RestartGame()
	{
		// SCORLARI V.S. SIFIRLAMAYI UNUTMA... HIHG SCORE KONTROLÜ YAPMAYI UNUTMA. YADA ONU END GAME DE YAPALIM.. 
		menuAnim.SetTrigger("cik");
		//UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		//TheStack.instance.SetGameOver();
		
		TheStack.instance.StartGame();
		
	}

	public void OpenMenu()
	{
		menuAnim.SetTrigger("gir");
	}

	public void setScore(int score)
	{
		scoreText.text = score.ToString();
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

	public void Fog()
	{
		if (PlayerPrefs.GetInt("fog") == 1)
		{
			// sisi kapatma işlemleri
			PlayerPrefs.SetInt("fog", 0);
			fogImg.sprite = offSprite;
			fogObj.SetActive(false);
		}
		else if (PlayerPrefs.GetInt("fog") == 0)
		{
			// sisi açma işlemleri
			PlayerPrefs.SetInt("fog", 1);
			fogImg.sprite = onSprite;
			fogObj.SetActive(true);
		}
	}

	private void StartSettings()
	{
		if (PlayerPrefs.GetInt("fog") == 1) fogImg.sprite = onSprite;
		else if (PlayerPrefs.GetInt("fog") == 0) fogImg.sprite = onSprite;

		if (PlayerPrefs.GetInt("vibration") == 1) vibrationImg.sprite = onSprite;
		else if (PlayerPrefs.GetInt("vibration") == 0) vibrationImg.sprite = offSprite;

		if (PlayerPrefs.GetInt("sound") == 1) soundImg.sprite = onSprite;
		else if (PlayerPrefs.GetInt("sound") == 0) soundImg.sprite = offSprite;


	}

	public void Like()
	{

	}

	public void Info()
	{

	}

	
}
