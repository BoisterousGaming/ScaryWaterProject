using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UICanvasHandler : MonoBehaviour {

	static UICanvasHandler _Instance;

	public Camera _RenderingCamera;
	public float _PlaneDistance = 100;
	public List<GameObject> _ActiveCanvas = new List<GameObject>();
	public string _DefaultCanvas = "Default";

	//Get instance of UICanvasHandler
	public static UICanvasHandler Instance
	{
		get
		{
			return _Instance;
		}
	}
		
	void Awake()
    {
		_Instance = this;

		if(_RenderingCamera == null)
		{
			_RenderingCamera = Camera.main;
		}
	}

	//Load default canvas
	void Start()
	{
		if(_DefaultCanvas != "")
			LoadScreen(_DefaultCanvas,null);
	}

	//Creating canvas from prefab
	public GameObject LoadScreen(string pCanvasName,Action callback = null,bool animated = true)
	{
		GameObject tNewCanvasObject =  Instantiate (Resources.Load ("UI/"+pCanvasName)) as GameObject;
		tNewCanvasObject.name = pCanvasName;
		Canvas tCanvasScr = tNewCanvasObject.GetComponent<Canvas>();
		tCanvasScr.worldCamera = _RenderingCamera;
		tCanvasScr.planeDistance = _PlaneDistance;
		tNewCanvasObject.transform.SetParent(transform);
		_ActiveCanvas.Add(tNewCanvasObject);

		if(animated)
		{
			GUIItemsManager tItemsManager = tNewCanvasObject.GetComponent<GUIItemsManager>();
			tItemsManager.BeginEntryAnimation(callback);
		}
		else
		{
			if(callback != null)
				callback();
		}
		return tNewCanvasObject;
	}

	public GameObject GetActiveCanvasByName(string pCanvasName)
	{
		GameObject tRequiredCanvas = null;
		for(int i = 0; i < _ActiveCanvas.Count ; i++)
		{
			if(_ActiveCanvas[i].name == pCanvasName)
			{
				tRequiredCanvas = _ActiveCanvas[i];
				break;
			}
		}
		return tRequiredCanvas;
	}

	//Remove canvas by name
	public void CloseScreen(string pCanvasName,Action callback,bool animated = true)
	{
		GameObject tRequiredCanvas = null;
		for(int i = 0; i < _ActiveCanvas.Count ; i++)
		{
			if(_ActiveCanvas[i].name == pCanvasName)
			{
				tRequiredCanvas = _ActiveCanvas[i];
				break;
			}
		}

		if(tRequiredCanvas != null)
		{
			if(animated)
			{
				GUIItemsManager tItemsManager = tRequiredCanvas.GetComponent<GUIItemsManager>();
				tItemsManager.BeginExitAnimation(callback);
			}
			else
			{
				if(callback != null)
					callback();
			}
		}
	}

	//Remove a canvas by object
	public void DestroyScreen(GameObject pCanvas)
	{
		if(_ActiveCanvas.Contains(pCanvas))
		{
			_ActiveCanvas.Remove(pCanvas);
			Destroy(pCanvas);
		}
	}

	//Remove all canvas objects
	public void RemoveAllActiveCanvas()
	{
		for(int i = 0; i < _ActiveCanvas.Count ; i++)
		{
			Destroy(_ActiveCanvas[i]);
		}
		_ActiveCanvas.Clear();
	}

	//Activate or deactivate a canvas
	public void CanvasSetActive(string pCanvasName,bool pActive)
	{
		GameObject tRequiredCanvas = null;
		for(int i = 0; i < _ActiveCanvas.Count ; i++)
		{
			if(_ActiveCanvas[i].name == pCanvasName)
			{
				tRequiredCanvas = _ActiveCanvas[i];
				break;
			}
		}

		if(tRequiredCanvas != null)
			tRequiredCanvas.SetActive(pActive);
	}

	//Set rendering order of a canvas
	public void SetCanvaseOrderInLayer(string pCanvasName,int pOrder)
	{
		//Check if a canvas is present in the active list first
		GameObject tRequiredCanvas = null;
		for(int i = 0; i < _ActiveCanvas.Count ; i++)
		{
			if(_ActiveCanvas[i].name == pCanvasName)
			{
				tRequiredCanvas = _ActiveCanvas[i];
				break;
			}
		}

		if(tRequiredCanvas != null)
			tRequiredCanvas.GetComponent<Canvas>().sortingOrder = pOrder;
	}
}
