﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	static PlayerManager mInstance;
    int miEquipedSkinID;
    bool mbPlayerIsDead = false;

    public GameObject _playerPrefab;
    public GameObject _poisonPrefab;
    public PlayerHandler _playerHandler;
    public Texture[] _arrOfPlayerTexture;
    public BarProgressSprite _BarProgressSpriteScr;
    public GamePlayCameraScr _CameraControllerScr;
    public EnvironmentManager _EnvironmentManagerScr;
    public EnemyAndObstacleManager _EnemyAndObstacleManagerScr;
    //public MiniGameManager _MiniGameManagerScr;
    public ParticleSystem _BrokenHeart;
    public BZCObjectPath _BZCPlayerPathScr;
    public BZCObjectPath _BZCCameraPathScr;
    public DayNightHandler _DayNightHandlerScr;

    public static PlayerManager Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
		if (mInstance == null)
			mInstance = this;

		else if (mInstance != this)
			Destroy(this.gameObject);

        if (_playerPrefab != null)
            PlayerSpawnLoc(new Vector3(0f, -1f, 0f));
	}

    void Start()
    {
        _BZCPlayerPathScr._ArrivedAtTheDropPointCallback += InitiateJump;
        Invoke("AfterStart", 2);
    }

    void PlayerSpawnLoc(Vector3 SpawnPos)
    {
        GameObject goPlayer = Instantiate(_playerPrefab);
        goPlayer.transform.SetParent(this.transform);
        goPlayer.transform.position = SpawnPos;
        _playerHandler = goPlayer.GetComponent<PlayerHandler>();
        _playerHandler._playerManager = this;
        _BarProgressSpriteScr = _playerHandler._BarProgressSpriteScr;
        EquipSkinOnStart(goPlayer);
        _BZCPlayerPathScr._SelectedObject = goPlayer;
    }

    void EquipSkinOnStart(GameObject player)
    {
        miEquipedSkinID = DataManager.GetEquipedSkinID();

		for (int i = 0; i < _arrOfPlayerTexture.Length; i++)
		{
			if (miEquipedSkinID == i)
				player.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.mainTexture = _arrOfPlayerTexture[i];
		}
    }

    public void SetupPlayerForFirstJump()
    {
        _BZCPlayerPathScr._bDropPoint = true;
        _BZCPlayerPathScr._iDropPointIndex = 4;
    }

    void InitiateJump(Vector3 vPos)
    {
        UICanvasHandler.Instance.LoadScreen("HUDCanvas");
        _EnemyAndObstacleManagerScr._SnakeScr.StopDOTweenSequence();
        _playerHandler.ReadySteadyGo();
        _BZCPlayerPathScr.StopPath();
        _BZCCameraPathScr.ResumePath();
        _DayNightHandlerScr._bEnableDayNightHandler = true;
    }

    public void PlayerDeathHandler()
    {
        _playerHandler._jumpActionScr.StopJump("death");
        StartCoroutine(IDelayGameOverScreen());
    }

    IEnumerator IDelayGameOverScreen()
    {
        yield return new WaitForSeconds(2f);
        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
        if (tCanvas != null)
            UICanvasHandler.Instance.DestroyScreen(tCanvas);

        UICanvasHandler.Instance.LoadScreen("GameOverCanvas");
    }

    public void PlayerMaxHealthLimit()
    {
        Debug.Log("You have reached the max limit of health bar count!");
    }

    void AfterStart()
    {
        _BarProgressSpriteScr._FillsCountReducedCallback += BrokenHeartCallback;
    }

    void BrokenHeartCallback(int val)
    {
        Vector3 tPlayerPos;
        tPlayerPos.x = _playerHandler._tPlayerTransform.position.x;
        tPlayerPos.y = _playerHandler._tPlayerTransform.position.y + 1.5f;
        tPlayerPos.z = _playerHandler._tPlayerTransform.position.z;
        _BrokenHeart.transform.position = tPlayerPos;
        _BrokenHeart.Play();
    }

    public void SetPlayerDeadState(bool state = true)
    {
        mbPlayerIsDead = state;
    }

    public bool GetPlayerDeadState()
    {
        return mbPlayerIsDead;
    }

    void OnDisable()
    {
        if (_BZCPlayerPathScr._ArrivedAtTheDropPointCallback != null)
            _BZCPlayerPathScr._ArrivedAtTheDropPointCallback -= InitiateJump;
        if (_BarProgressSpriteScr._FillsCountReducedCallback != null)
            _BarProgressSpriteScr._FillsCountReducedCallback -= BrokenHeartCallback;
    }
}
