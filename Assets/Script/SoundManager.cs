using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;

	private AudioSource audioSource;

	private int rewardedStackCounter = 12;

	[SerializeField]
	private AudioClip[] sounds;

	[SerializeField]
	private AudioClip[] rubbleSounds;

	

	private void Awake()
	{
		if (instance == null) instance = this;
		audioSource = GetComponent<AudioSource>();
	}

	
	public void ComboSounds( int no)
	{
		if(PlayerPrefs.GetInt("sound") == 1)
		{
			if (no < sounds.Length) audioSource.clip = sounds[no];
			else audioSource.clip = sounds[sounds.Length - 1];

			audioSource.PlayOneShot(audioSource.clip);
		}
		
	}


	public void RubbleSounds()
	{
		if (PlayerPrefs.GetInt("sound") == 1)
		{
			int rnd = Random.Range(0, 6);
			audioSource.clip = rubbleSounds[rnd];
			audioSource.PlayOneShot(audioSource.clip);
		}
	}

	public void RewardedStacksSound()
	{
		if (PlayerPrefs.GetInt("sound") == 1)
		{
			
			audioSource.clip = sounds[rewardedStackCounter];//12-13-14-15
			audioSource.PlayOneShot(audioSource.clip);
			rewardedStackCounter++;//13-14-15-16
			if (rewardedStackCounter > 16) rewardedStackCounter = 12;
		}
	}




}
