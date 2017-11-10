using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour 
{
    static AudioManager mInstance;
    int miCurrentSceneIndex;
    bool mbPlayAmbience = false;

    public float _fMainMenuBGVolume = 1f;
    public float _fJungleBGVolume = 1f;
    public float _fDesertBGVolume = 1f;
    public float _fMountainsBGVolume = 1f;
    public float _fChallengeBGVolume = 1f;

    public static AudioManager Instance
    {
        get { return mInstance; }
    }
    void Awake()
    {
        if (mInstance == null)
            mInstance = this;
        
        else if (mInstance != this)
            Destroy(this.gameObject);

        InitializeAudioOnAwake();
    }

    void InitializeAudioOnAwake()
    {
        miCurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (miCurrentSceneIndex)
        {
            case 0:
                PlayAudio("MainMenuBG", _fMainMenuBGVolume);
                break;

            case 1:
                PlayAudio("JungleBG", _fJungleBGVolume, true);
                break;

            case 2:
                PlayAudio("DesertBG", _fDesertBGVolume, true);
                break;

            case 3:
                PlayAudio("MountainsBG", _fMountainsBGVolume, true);
                break;

            case 4:
                PlayAudio("ChallengeBG", _fChallengeBGVolume, true);
                break;
        }
    }

    void PlayAudio(string audioSource = "MainMenuBG", float audioVolume = 1f, bool ambienceState = false)
    {
        CMusicPlayer.Instance.Play(audioSource);
        CMusicPlayer.Instance.Volume = audioVolume;

        if (!ambienceState)
            return;

        // else play ambience
    }
}
