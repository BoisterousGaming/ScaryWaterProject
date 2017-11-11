using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCollectableType
{
    None = 0,
    StartCoin,
    Butterfly,
    Magnet,
    Poison,
    AirWing
}

public enum eMoveTowards
{
	None = 0,
	Player
}

public class CollectableHandler : MonoBehaviour 
{
    int miCoinValue = 1;
    int miButterflyValue = 1;
    bool mbSkipChecking = false;
    bool mbShouldDestroy = false;

	public eCollectableType _eCollectableType = eCollectableType.None;
    public eMoveTowards _eMoveTowards = eMoveTowards.None;
	public float _fMoveTowardsMagnetSpeed = 30.0f;
	public float _fMoveTowardsPlayerSpeed = 30.0f;
    public float _xAxisPosition;
	public Transform _Transform;

	public Transform GetTransform
	{
		get
		{
			if (_Transform == null)
				_Transform = transform;

			return _Transform;
		}
	}

    void Awake()
    {
        _Transform = transform;
    }

    void OnEnable()
    {
        CollectableAndFoodManager.Instance._listOfCollectableHandlers.Add(this);
    }

    void Start()
    {
        miCoinValue = DataManager.GetCoinValue();
		_xAxisPosition = transform.position.x;
    }

    void Update()
	{
		ElementMoveTowardsHandler();

        if (mbShouldDestroy)
            Destroy(this.gameObject);
	}

    void OnDestroy()
    {
        CollectableAndFoodManager.Instance._listOfCollectableHandlers.Remove(this);
    }

    void ElementMoveTowardsHandler()
    {
		switch (_eMoveTowards)
		{
			case eMoveTowards.None:
				break;

			case eMoveTowards.Player:
				MoveTowardsPlayer(PlayerManager.Instance._playerHandler.transform.position);
				break;
		}
    }

	void CoinCount()
	{
		//CRdl Play particle effect
		CollectableAndFoodManager.Instance.PlayerCollectedCoin(transform);

        DataManager.AddToTotalCoin(miCoinValue);
        DataManager.AddToCSessionCoin(miCoinValue);

        GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
        if (tCanvas != null)
            tCanvas.GetComponent<GameplayAreaUIHandler>().DisplayCoinCount();

		if (MiniGameManager.Instance._eMiniGameState == eMiniGameState.CollectCoins)
			MiniGameManager.Instance._iCoinsCollected += miCoinValue;
	}

	void ButterflyCount()
	{
        DataManager.AddToTotalButterfly(miButterflyValue);
        DataManager.AddToCSessionButterfly(miButterflyValue);

		GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");

		if (tCanvas != null)
            tCanvas.GetComponent<GameplayAreaUIHandler>().DisplayButterflyCount();
        
		if (MiniGameManager.Instance._eMiniGameState == eMiniGameState.CollectButterflies)
			MiniGameManager.Instance._iButterfliesCollected += miButterflyValue;
	}

	void MoveTowardsPlayer(Vector3 CurrentPosition)
	{
		transform.position = Vector3.MoveTowards(transform.position, CurrentPosition, _fMoveTowardsMagnetSpeed * Time.deltaTime);

		if (Vector3.Distance(transform.position, CurrentPosition) < 2f)
		{
			CoinCount();
			_eMoveTowards = eMoveTowards.None;
            mbShouldDestroy = true;
		}
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!mbSkipChecking)
            {
                mbSkipChecking = true;
                CollectableAndFoodManager.Instance.CompareCollectableElements(this);

                if (this._eCollectableType == eCollectableType.StartCoin)
                {
                    mbShouldDestroy = true;

                    CoinCount();
                    CEffectsPlayer.Instance.Play("CoinCollection");
					GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
					if (tCanvas != null)
						tCanvas.GetComponent<GameplayAreaUIHandler>().InstantiateCoin(this.transform);

                }

				if (this._eCollectableType == eCollectableType.Butterfly)
				{
                    mbShouldDestroy = true;

                    ButterflyCount();
                    CEffectsPlayer.Instance.Play("ButterflyCollection");
					GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("HUDCanvas");
					if (tCanvas != null)
						tCanvas.GetComponent<GameplayAreaUIHandler>().InstantiateButterfly(this.transform);
				}
            }
        }
    }
}
