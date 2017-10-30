using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreTabUIHandler : GUIItemsManager
{
    static StoreTabUIHandler mInstance;

	public Color _ActiveColor;
	public Color _DeactiveColor;
    public List<Button> _listOfBtn = new List<Button>();

    public static StoreTabUIHandler Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        base.Init();

        if (mInstance == null)
            mInstance = this;

        else if (mInstance != this)
            Destroy(this.gameObject);
    }

    void Start()
    {
        UICanvasHandler.Instance.LoadScreen("StorePlayerSkinCanvas", null, true);    
    }

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);
        ChangeButtonColor(item);

        switch (item.gameObject.name)
        {
            case "PlayerSkinBtn":
                for (int i = 0; i < UICanvasHandler.Instance._ActiveCanvas.Count; i++)
                {
                    GameObject tCanvas = UICanvasHandler.Instance._ActiveCanvas[i];

                    if (tCanvas.name.Equals("StoreUpgradeCanvas") | tCanvas.name.Equals("StoreBoosterCanvas") | tCanvas.name.Equals("StoreMoneyCanvas"))
                        UICanvasHandler.Instance.DestroyScreen(tCanvas);
                }

                UICanvasHandler.Instance.LoadScreen("StorePlayerSkinCanvas", null, true);
                break;

            case "UpgradeBtn":
				for (int i = 0; i < UICanvasHandler.Instance._ActiveCanvas.Count; i++)
				{
					GameObject tCanvas = UICanvasHandler.Instance._ActiveCanvas[i];

                    if (tCanvas.name.Equals("StorePlayerSkinCanvas") | tCanvas.name.Equals("StoreBoosterCanvas") | tCanvas.name.Equals("StoreMoneyCanvas"))
                        UICanvasHandler.Instance.DestroyScreen(tCanvas);
				}

				UICanvasHandler.Instance.LoadScreen("StoreUpgradeCanvas", null, true);
                break;

            case "BoosterBtn":
				for (int i = 0; i < UICanvasHandler.Instance._ActiveCanvas.Count; i++)
				{
					GameObject tCanvas = UICanvasHandler.Instance._ActiveCanvas[i];

					if (tCanvas.name.Equals("StorePlayerSkinCanvas") | tCanvas.name.Equals("StoreUpgradeCanvas") | tCanvas.name.Equals("StoreMoneyCanvas"))
						UICanvasHandler.Instance.DestroyScreen(tCanvas);
				}

				UICanvasHandler.Instance.LoadScreen("StoreBoosterCanvas", null, true);
                break;

            case "CoinBtn":
				for (int i = 0; i < UICanvasHandler.Instance._ActiveCanvas.Count; i++)
				{
					GameObject tCanvas = UICanvasHandler.Instance._ActiveCanvas[i];

					if (tCanvas.name.Equals("StorePlayerSkinCanvas") | tCanvas.name.Equals("StoreUpgradeCanvas") | tCanvas.name.Equals("StoreBoosterCanvas"))
						UICanvasHandler.Instance.DestroyScreen(tCanvas);
				}

				UICanvasHandler.Instance.LoadScreen("StoreMoneyCanvas", null, true);
                break;
        }
    }

    void ChangeButtonColor(GUIItem item)
    {
		foreach (Button element in _listOfBtn)
		{
			if (element.gameObject.name.Equals(item.gameObject.name))
			{
				ColorBlock tCB = element.colors;
				tCB.normalColor = _ActiveColor;
				element.colors = tCB;
			}

			else
			{
				ColorBlock tCB = element.colors;
				tCB.normalColor = _DeactiveColor;
				element.colors = tCB;
			}
		}
    }

    public override void OnBackButton() 
    {
        for (int i = 0; i < UICanvasHandler.Instance._ActiveCanvas.Count; i++)
        {
            GameObject tCanvas = UICanvasHandler.Instance._ActiveCanvas[i];

            if (tCanvas.name.Equals("StorePlayerSkinCanvas") | tCanvas.name.Equals("StoreUpgradeCanvas") | tCanvas.name.Equals("StoreBoosterCanvas") | tCanvas.name.Equals("StoreMoneyCanvas"))
                UICanvasHandler.Instance.DestroyScreen(tCanvas);
        }

        UICanvasHandler.Instance.DestroyScreen(this.gameObject);

        if (SceneManager.GetActiveScene().buildIndex == 0)
            UICanvasHandler.Instance.LoadScreen("MainMenuCanvas", null, true);

        else
            UICanvasHandler.Instance.LoadScreen("GameOverCanvas", null, true);
    }
}
