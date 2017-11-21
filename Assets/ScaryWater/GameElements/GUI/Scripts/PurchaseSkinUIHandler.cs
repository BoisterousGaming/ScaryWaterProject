using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseSkinUIHandler : GUIItemsManager 
{
    static PurchaseSkinUIHandler mInstance;

    public static PurchaseSkinUIHandler Instance
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

    public override void OnButtonCallBack(GUIItem item)
    {
        //Debug.Log("Button Pressed: " + item.gameObject.name);

        switch (item.gameObject.name)
        {
            case "YesBtn":
                //CEffectsPlayer.Instance.Play("PostitiveSound");
                GameObject tCanvas = UICanvasHandler.Instance.GetActiveCanvasByName("StorePlayerSkinCanvas");
                if (tCanvas != null)
                {
                    StorePlayerSkinUIHandler tStorePlayerSkinUIScr = tCanvas.GetComponent<StorePlayerSkinUIHandler>();

                    foreach (KeyValuePair<ePlayerSkinID, int> element in tStorePlayerSkinUIScr._dictOfSkinPrice)
                    {
                        if (tStorePlayerSkinUIScr._SelectedSkinID == (int)element.Key)
                        {
                            if (element.Value <= DataManager.GetTotalCoinAmount())
                            {
                                tStorePlayerSkinUIScr.OnPurchaseFinished();
                                DataManager.SubstractFromTotalCoin(element.Value);
                                UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                            }

                            else
                            {
                                UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                                UICanvasHandler.Instance.LoadScreen("CoinWarningCanvas");
                            }
                        }
                    }
                }
                break;

            case "NoBtn":
                //CEffectsPlayer.Instance.Play("NegativeSound");
                UICanvasHandler.Instance.DestroyScreen(this.gameObject);
                break;
        }
    }
}
