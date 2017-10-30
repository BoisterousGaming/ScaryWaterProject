using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEffectsPlayer : MonoBehaviour 
{
	static CEffectsPlayer mInstance;

	public AudioSource _AudioSource;
	Hashtable mClipTable = new Hashtable();
	string mStorageName = "EffectsPlayer";
	float mfVolume = 1;
	bool mbMute = false;

	//Get singleton of Music Player
	public static CEffectsPlayer Instance {
		get {
			if (mInstance == null) 
			{
				GameObject tObj = new GameObject();
				tObj.name = "MusicPlayer";
				mInstance = tObj.AddComponent<CEffectsPlayer>();
				AudioSource tSource = tObj.AddComponent<AudioSource> ();
				mInstance.Initialize(tSource);
				DontDestroyOnLoad(tObj);
			}
			return mInstance;
		}
	}

	//Getter and setter for volume
	public float Volume
	{
		//GET OR SET VOLUME
		get{return mfVolume;}
		set
		{
			mfVolume = value;
			_AudioSource.volume = mfVolume;
			if(mStorageName != "")
				PlayerPrefs.SetFloat(mStorageName+"_Volume",mfVolume);
		}
	}

	//Getter and setter for mute
	public bool Mute
	{
		//GET OR SET MUTE STATUS
		get{return mbMute;}
		set
		{
			mbMute = value;
			_AudioSource.mute = mbMute;
			PlayerPrefs.SetInt (mStorageName+"_Mute", mbMute ? 1 : 0);
		}
	}

	void Initialize(AudioSource pSource)
	{
		_AudioSource = pSource;
		_AudioSource.playOnAwake = false;
		_AudioSource.loop = false;

		_AudioSource.volume = PlayerPrefs.GetFloat(mStorageName+"_Volume",1);
		_AudioSource.mute = PlayerPrefs.GetInt (mStorageName + "_Mute", 0) == 1 ? true : false;
	}

	//Play Audio Clip
	public void Play(string clipName,float pvolume = 1)
	{
		if(mClipTable.Contains(clipName))
		{
			AudioClip tClip = mClipTable[clipName] as AudioClip;
			_AudioSource.PlayOneShot(tClip,mfVolume*pvolume);
		}
		else
		{
			AudioClip tClip = Resources.Load ("Sounds/" + clipName) as AudioClip;
			if (tClip != null) {
				mClipTable.Add(clipName,tClip);
				_AudioSource.PlayOneShot(tClip,mfVolume*pvolume);
			} else {
				Debug.LogWarning ("The audio clip could not be found: " + clipName);
			}
		}
	}

	//Preload from list of clips (Clips are stored under Resources/Sounds/...)
	public void PreLoadClips(AudioClip[] pClips)
	{
		foreach(AudioClip tClip in pClips)
		{
			if(!mClipTable.Contains(tClip.name))
				mClipTable.Add(tClip.name,tClip);
		}
	}

	//Preload from list of clip names (Clips are stored under Resources/Sounds/...)
	public void PreLoadClips(string[] pClipNames)
	{
		for(int i = 0 ; i < pClipNames.Length ; i++)
		{
			string tClipName = pClipNames [i];
			AudioClip tClip = Resources.Load ("Sounds/" + tClipName) as AudioClip;
			if (tClip != null) {
				if (!mClipTable.Contains (tClipName))
					mClipTable.Add (tClipName, tClip);
			}
		}
	}

	//Pause audio source
	public void Pause()
	{
		_AudioSource.Pause();
	}

	//Resume audio source
	public void Resume()
	{
		if(_AudioSource.clip != null)
			_AudioSource.Play();
	}

	//Stop audio source
	public void Stop()
	{
		if(_AudioSource.isPlaying)
		{
			_AudioSource.Stop();
		}
	}

	//Clear audio source and table
	public void Clear()
	{
		Stop();
		_AudioSource.clip = null;
		mClipTable.Clear();
	}
}