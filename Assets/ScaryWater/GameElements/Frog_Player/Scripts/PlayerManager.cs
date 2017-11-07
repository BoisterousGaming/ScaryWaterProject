using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
	static PlayerManager mInstance;
    int miEquipedSkinID;

    public GameObject _playerPrefab;
    public GameObject _poisonPrefab;
    public PlayerHandler _playerHandler;
    public Texture[] _arrOfPlayerTexture;
    public BarProgressSprite _BarProgressSpriteScr;
    public CameraController _CameraControllerScr;
    public EnvironmentManager _EnvironmentManagerScr;
    public MiniGameManager _MiniGameManagerScr;

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
	}

    void Start()
    {
        if (_playerPrefab != null)
            PlayerSpawnLoc(new Vector3(0f, 0.5f, 0f));

		else
            Debug.LogWarning("Player prefab dose not exist!");
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

    public void PlayerDeathHandler()
    {
        _playerHandler._jumpActionScr.StopJump("death");

        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
		if (tCanvas != null)
			UICanvasHandler.Instance.DestroyScreen(tCanvas);

		UICanvasHandler.Instance.LoadScreen("GameOverCanvas", null, true);
    }

    public void PlayerMaxHealthLimit()
    {
        Debug.Log("You have reached the max limit of health bar count!");
    }
}
