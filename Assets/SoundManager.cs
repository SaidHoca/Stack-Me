using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;

	private AudioSource audioSource;

	[SerializeField]
	private AudioClip[] sounds;

	

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



}
